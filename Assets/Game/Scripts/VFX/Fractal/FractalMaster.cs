using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[ExecuteInEditMode]
public class FractalMaster : MonoBehaviour
{
    [Serializable]
    public class MandelbulbParameters
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
    [Header("Fractal Transform")]
    [SerializeField, Range(0.1f, 10f)] private float fractalScale = 1f;
    public MandelbulbParameters CurrentMandelbulbParameters { get => _currentMandelbulbParameters; set => _currentMandelbulbParameters = value; }
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

    private int handleCSMain = -1;

    public float[] groupMinData;
    public int groupMin;

    private ComputeBuffer groupMinBuffer;

    private int threadGroupsX;
    private int threadGroupsY;

    public float minDist;

    private int maxStepCount = 500;

    public int maxIterations;

    public bool LODChangeWithDist;

    Matrix4x4 cameraToWorldMatrix;
    Matrix4x4 projectionMatrixInverse;

    [SerializeField] Material _mandelbulbMat;

    void Start()
    {
        Application.targetFrameRate = 60;

        if (null == fractalShader)
        {
            Debug.LogError("Fractal Shader missing!");
            return;
        }

        // Trouver le kernel une seule fois au démarrage
        handleCSMain = fractalShader.FindKernel("CSMain");

        if (handleCSMain < 0)
        {
            Debug.LogError("Cannot find kernel 'CSMain' in compute shader!");
            enabled = false;
            return;
        }
    }

    void Init()
    {
        cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("No main camera found!");
            return;
        }

        threadGroupsX = Mathf.CeilToInt(cam.pixelWidth / 64.0f);
        threadGroupsY = Mathf.CeilToInt(cam.pixelHeight / 1.0f);
    }

    void InitBuffer()
    {
        if (groupMinBuffer != null)
        {
            groupMinBuffer.Release();
        }

        groupMinBuffer = new ComputeBuffer(threadGroupsX, (sizeof(uint) * 2) + (sizeof(float) * 1));
        groupMinData = new float[threadGroupsX * 3];
    }

    void LateUpdate()
    {
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
        if (fractalShader == null || handleCSMain < 0)
        {
            return;
        }

        Init();

        if (cam == null)
        {
            return;
        }

        InitRenderTexture();
        InitBuffer();
        SetParameters();

        // Vérifier que tout est bien initialisé avant le dispatch
        if (groupMinBuffer == null || target == null)
        {
            Debug.LogError("Buffers not properly initialized!");
            return;
        }

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

        minDist = groupMinData[3 * groupMin + 2];

        Graphics.Blit(target, rt);

        CleanupBuffers();
    }

    void SetParameters()
    {
        cam.fieldOfView = cam.fieldOfView;
        //cam.fieldOfView = cam.fieldOfView;
        cameraToWorldMatrix = cam.cameraToWorldMatrix;
        projectionMatrixInverse = cam.projectionMatrix.inverse;


        if (Application.isPlaying)
        { //animation
            _fractalPower += Time.deltaTime * 0.2f;
        }

        fractalShader.SetTexture(handleCSMain, "Destination", target);
        fractalShader.SetFloat("alpha", _extAlpha);
        _mandelbulbMat.SetFloat("_Alpha", _alpha);

        fractalShader.SetFloat("power", Mathf.Max(_currentMandelbulbParameters.FractalPower, 1.01f));
        fractalShader.SetFloat("darkness", _currentMandelbulbParameters.Darkness);
        fractalShader.SetFloat("blackAndWhite", _currentMandelbulbParameters.BlackAndWhite);
        fractalShader.SetFloat("maxDst", drawDistance);
        fractalShader.SetVector("colourAMix", _currentMandelbulbParameters.ColorA);
        fractalShader.SetVector("colourBMix", _currentMandelbulbParameters.ColorB);
        //fractalShader.SetVector("positionOffset", positionOffset);
        fractalShader.SetVector("positionOffset", transform.position);
        fractalShader.SetFloat("fractalScale", fractalScale);
        fractalShader.SetVector("fractalRotation", transform.eulerAngles);

        // Calculer la distance entre la caméra et la fractale
        float distToFractal = Vector3.Distance(cam.transform.position, transform.position);
        // Adapter le nombre de pas en fonction de la distance
        maxStepCount = Mathf.Max(250, Mathf.CeilToInt(distToFractal * 100));
        fractalShader.SetInt("maxStepCount", maxStepCount);

        if (LODChangeWithDist)
        {
            maxIterations = Mathf.FloorToInt(5f / minDist);
        }
        else maxIterations = 15;

        fractalShader.SetInt("maxIterations", maxIterations);

        fractalShader.SetMatrix("_CameraToWorld", cameraToWorldMatrix);
        fractalShader.SetMatrix("_CameraInverseProjection", projectionMatrixInverse);

        if (directionalLight)
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

    void CleanupBuffers()
    {
        if (groupMinBuffer != null)
        {
            groupMinBuffer.Release();
            groupMinBuffer = null;
        }
    }

    void OnDestroy()
    {
        CleanupBuffers();

        if (target != null)
        {
            target.Release();
            target = null;
        }
    }

    void OnDisable()
    {
        CleanupBuffers();
    }
}