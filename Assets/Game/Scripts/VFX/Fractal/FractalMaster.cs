using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class FractalMaster : MonoBehaviour
{
    [Serializable]
    public class MandelbulbParameters // Inheriting MonoBehaviour makes values animation possible 
    {
        public MandelbulbParameters()
        {
            _fractalPower = 7;
            _alpha = 1f;
            _colorA = new Color(0.5f, 0F, 0.5f);
            _colorB = new Color(1f, 0.5f, 0f);
            _blackAndWhite = 0.7f;
            _darkness = 26f;
        }

        public float FractalPower { get => _fractalPower; set => _fractalPower = value; }
        public float Alpha { get => _alpha; set => _alpha = value; }
        public Color ColorA { get => _colorA; set => _colorA = value; }
        public Color ColorB { get => _colorB; set => _colorB = value; }
        public float BlackAndWhite { get => _blackAndWhite; set => _blackAndWhite = value; }
        public float Darkness { get => _darkness; set => _darkness = value; }

        [SerializeField, Range(1, 20)] private float _fractalPower;
        [SerializeField, Range(0f, 1f)] private float _alpha;
        [SerializeField] private Color _colorA;
        [SerializeField] private Color _colorB;
        [SerializeField, Range(0f, 1f)] private float _blackAndWhite;
        [SerializeField] private float _darkness;
    }


    // For Animation (yes this is not sexy, complex problems mean dirty fixes)
    public float FractalPower { get => _fractalPower; set => _fractalPower = value; }
    public float Alpha { get => _alpha; set => _alpha = value; }
    public Color ColorA { get => _colorA; set => _colorA = value; }
    public Color ColorB { get => _colorB; set => _colorB = value; }
    public float BlackAndWhite { get => _blackAndWhite; set => _blackAndWhite = value; }
    public float Darkness { get => _darkness; set => _darkness = value; }

    [SerializeField, Range(1, 20)] private float _fractalPower = 7f;
    [SerializeField, Range(0f, 1f)] private float _alpha = 1f;
    [SerializeField] private Color _colorA = new Color(0.5f, 0F, 0.5f);
    [SerializeField] private Color _colorB = new Color(1f, 0.5f, 0f);
    [SerializeField, Range(0f, 1f)] private float _blackAndWhite = 0.7f;
    [SerializeField] private float _darkness = 26f;

    public MandelbulbParameters CurrentMandelbulbParameters {  get => _currentMandelbulbParameters; set => _currentMandelbulbParameters = value; }
    private MandelbulbParameters _currentMandelbulbParameters = new();

    [HorizontalLine(color: EColor.Blue)]
    [SerializeField] private ComputeShader fractalShader;
    [SerializeField] private RenderTexture rt;

    [SerializeField] private float drawDistance = 2f;

    [Range(-50, 1)] public float _extAlpha;

    public Vector3 positionOffset;

    RenderTexture target;
    Camera cam;
    [SerializeField]
    Light directionalLight;

    [Header("Animation Settings")]
    public float powerIncreaseRate = 0.2f;
    public float oscillationRate = 0.2f;
    public float oscillationRange = 5f;
    private float t = 0;

    private int handleCSMain;

    public float[] groupMinData;
    public int groupMin;

    private ComputeBuffer groupMinBuffer;

    private int threadGroupsX;
    private int threadGroupsY;

    public float minDist;

    private int maxStepCount = 250;

    public int maxIterations;

    public bool LODChangeWithDist;

    Matrix4x4 cameraToWorldMatrix;
    Matrix4x4 projectionMatrixInverse;
    private Material _mat;


    void Start()
    {
        Application.targetFrameRate = 60;

        if (null == fractalShader)
        {
            Debug.Log("Shader missing.");
            return;
        }
        _mat = GetComponent<Renderer>().sharedMaterial;
    }

    void Init()
    {
        cam = Camera.main;

        cameraToWorldMatrix = cam.cameraToWorldMatrix;
        projectionMatrixInverse = cam.projectionMatrix.inverse;

        threadGroupsX = Mathf.CeilToInt(cam.pixelWidth / 64.0f);     //CREATING A THREAD FOR EACH PIXEL (/8 AS IT'S *8 IN THE SHADER)
        threadGroupsY = Mathf.CeilToInt(cam.pixelHeight / 1.0f);
    }

    void InitBuffer()
    {
        groupMinBuffer = new ComputeBuffer(threadGroupsX, (sizeof(uint) * 2) + (sizeof(float) * 1));
        groupMinData = new float[threadGroupsX * 3];
    }

    // Animate properties
    void Update()
    {
        /*
        if (Application.isPlaying)
        {
            if (powerIncreaseRate != 0)
            {
                fractalPower += powerIncreaseRate * Time.deltaTime;
            }

            else if (oscillationRate != 0)
            {
                t = (t + (Time.deltaTime * oscillationRate)) % (2 * Mathf.PI);

                fractalPower = 1f + oscillationRange * (1f + (Mathf.Cos(t + Mathf.PI)));
            }

            //fractalPower = Mathf.Lerp(2.5f, 9f, (Mathf.Sin(Time.time * .25f) + 1) / 2);
            /*
            redA = Mathf.Cos((Time.time +0f) *0.3f)/2 + .5f;
            greenA = Mathf.Sin((Time.time - 0.6f) * 0.3f) /2 + .5f;
            blueA = Mathf.Sin((Time.time + .9f) * 0.3f) /2 + .5f;
            redB = Mathf.Cos((Time.time + .8f) * 0.3f) /2 + .5f;
            greenB = Mathf.Cos((Time.time - 0f) * 0.3f) /2 + .5f;
            blueB = Mathf.Sin((Time.time+ .0f) * 0.3f) /2 + .5f;
        }
        */
        UpdateValues();
        UpdateTexture();
    }

    void UpdateValues()
    {
        _currentMandelbulbParameters.FractalPower = _fractalPower;
        _currentMandelbulbParameters.Alpha = _alpha;
        _currentMandelbulbParameters.ColorA = _colorA;
        _currentMandelbulbParameters.ColorB = _colorB;
        _currentMandelbulbParameters.BlackAndWhite = _blackAndWhite;
        _currentMandelbulbParameters.Darkness = _darkness;
    }

    void UpdateTexture()
    {
        handleCSMain = fractalShader.FindKernel("CSMain");
        Init();
        InitRenderTexture();

        InitBuffer();

        SetParameters();

        fractalShader.Dispatch(handleCSMain, threadGroupsX, threadGroupsY, 1);

        // get minima of groups
        groupMinBuffer.GetData(groupMinData);

        // find minimum of all groups
        groupMin = 0;
        for (int group = 1; group < threadGroupsX; group++)
        {
            if (groupMinData[3 * group + 2] < groupMinData[3 * groupMin + 2])
            {
                groupMin = group;
            }
        }

        // At the end, the relative luminance of the brightest pixel is at groupMinData[3 * groupMin + 2].
        // Its x coordinate is at groupMinData[3 * groupMin + 0] and 
        // its y coordinate is at groupMinData[3 * groupMin + 1]
        minDist = groupMinData[3 * groupMin + 2];

        //Graphics.Blit(target, destination);
        Graphics.Blit(target, rt);

        OnDestroy();
    }

    void SetParameters()
    {
        //source.enableRandomWrite = true;
        //RenderTexture src = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB2101010);
        //src.enableRandomWrite = true;
        //src.Create();

        //Graphics.Blit(source, src);
        //Graphics.CopyTexture(source, src);

        if(_mat)
            _mat.SetColor("_BaseColor", new Color(1, 1, 1, _currentMandelbulbParameters.Alpha));

        fractalShader.SetTexture(0, "Destination", target);
        //fractalShader.SetTexture(0, "Source", src);
        fractalShader.SetFloat("alpha", _extAlpha);
        fractalShader.SetFloat("power", Mathf.Max(_currentMandelbulbParameters.FractalPower, 1.01f));
        fractalShader.SetFloat("darkness", _currentMandelbulbParameters.Darkness);
        fractalShader.SetFloat("blackAndWhite", _currentMandelbulbParameters.BlackAndWhite);
        fractalShader.SetFloat("maxDst", drawDistance);
        fractalShader.SetVector("colourAMix", _currentMandelbulbParameters.ColorA);
        fractalShader.SetVector("colourBMix", _currentMandelbulbParameters.ColorB);
        fractalShader.SetVector("positionOffset", positionOffset);

        fractalShader.SetInt("maxStepCount", maxStepCount);

        if (LODChangeWithDist)
        {
            maxIterations = Mathf.FloorToInt(5f / minDist);
        }
        else maxIterations = 15;

        fractalShader.SetInt("maxIterations", maxIterations);

        fractalShader.SetMatrix("_CameraToWorld", cameraToWorldMatrix);
        fractalShader.SetMatrix("_CameraInverseProjection", projectionMatrixInverse);
        if(directionalLight)
            fractalShader.SetVector("_LightDirection", directionalLight.transform.forward);

        fractalShader.SetBuffer(handleCSMain, "GroupMinBuffer", groupMinBuffer);
    }

    void InitRenderTexture()
    {
        if (target == null || target.width != cam.pixelWidth || target.height != cam.pixelHeight)
        {
            if (target != null)
            {
                target.Release();
            }

            target = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            target.enableRandomWrite = true;
            target.Create();
        }
    }

    void OnDestroy()
    {
        if (null != groupMinBuffer)
        {
            groupMinBuffer.Release();
        }
    }
}

