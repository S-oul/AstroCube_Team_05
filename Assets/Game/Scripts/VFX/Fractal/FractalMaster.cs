using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[ExecuteInEditMode]
public class FractalMaster : MonoBehaviour
{

    // For Animation
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

    [Range(-50, 1)] private float _extAlpha;
    [SerializeField] private Vector3 positionOffset;

    [Header("Simulation Settings")]
    [SerializeField] private float drawDistance = 2f;
    [SerializeField] private float minDist = 120;
    [SerializeField] private int maxStepCount = 120;
    [SerializeField] private int maxIterations = 10;
    [SerializeField] private Light directionalLight;
    [SerializeField] private bool useTargetFPS = false;
    [SerializeField] private float targetFPS = 20.0f;

    Camera cam;

    private int handleCSMain = -1;

    private int threadGroupsX;
    private int threadGroupsY;

    private int renderWidth = 1344;
    private int renderHeight = 756;

    Matrix4x4 cameraToWorldMatrix;
    Matrix4x4 projectionMatrixInverse;

    [SerializeField] Material _mandelbulbMat;
    private float fractalUpdateTimer;

    void Start()
    {
        if (null == fractalShader)
        {
            Debug.LogError("Fractal Shader missing!");
            return;
        }

        handleCSMain = fractalShader.FindKernel("CSMain");

        if (handleCSMain < 0)
        {
            Debug.LogError("Cannot find kernel 'CSMain' in compute shader!");
            enabled = false;
            return;
        }

        Init();
    }

    void Init()
    {
        cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("No main camera found!");
            return;
        }

        threadGroupsX = Mathf.CeilToInt(renderWidth / 8.0f);
        threadGroupsY = Mathf.CeilToInt(renderHeight / 8.0f);
    }

    void LateUpdate()
    {
        if (useTargetFPS)
        {
            fractalUpdateTimer += Time.deltaTime;

            if (fractalUpdateTimer >= 1.0f / targetFPS)
            {
                fractalUpdateTimer -= 1.0f / targetFPS;
                UpdateValues();
                UpdateTexture();
            }
        }
        else
        {
            UpdateValues();
            UpdateTexture();
        }
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
        if (fractalShader == null || handleCSMain < 0 || cam == null) return;

        SetParameters();

        fractalShader.Dispatch(handleCSMain, threadGroupsX, threadGroupsY, 1);
    }

    void SetParameters()
    {
        cameraToWorldMatrix = cam.cameraToWorldMatrix;
        projectionMatrixInverse = cam.projectionMatrix.inverse;

        fractalShader.SetTexture(handleCSMain, "Destination", rt);
        fractalShader.SetFloat("alpha", _extAlpha);
        _mandelbulbMat.SetFloat("_Alpha", _alpha);

        fractalShader.SetFloat("power", Mathf.Max(_currentMandelbulbParameters.FractalPower, 1.01f));
        fractalShader.SetFloat("darkness", _currentMandelbulbParameters.Darkness);
        fractalShader.SetFloat("blackAndWhite", _currentMandelbulbParameters.BlackAndWhite);
        fractalShader.SetFloat("maxDst", drawDistance);
        fractalShader.SetVector("colourAMix", _currentMandelbulbParameters.ColorA);
        fractalShader.SetVector("colourBMix", _currentMandelbulbParameters.ColorB);
        fractalShader.SetVector("positionOffset", transform.position);
        fractalShader.SetFloat("fractalScale", fractalScale);
        fractalShader.SetVector("fractalRotation", transform.eulerAngles);
        fractalShader.SetInt("maxIterations", maxIterations);
        fractalShader.SetInt("maxStepCount", maxStepCount);
        fractalShader.SetMatrix("_CameraToWorld", cameraToWorldMatrix);
        fractalShader.SetMatrix("_CameraInverseProjection", projectionMatrixInverse);

        if (directionalLight)
            fractalShader.SetVector("_LightDirection", directionalLight.transform.forward);
    }

}

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