using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarCutsceneAction : AGameAction
{
    [SerializeField] private EStarSpawnType _starSpawnType;

    [Header("- EYES SPAWN PARAMETERS -")]
    [ShowIf("_starSpawnType", EStarSpawnType.EYE), SerializeField] private GameObject _eyePrefab;
    [MinMaxSlider(5.0f, 100.0f)]
    [ShowIf("_starSpawnType", EStarSpawnType.EYE), SerializeField] private Vector2 _minMaxEyeSphereRadius = new(20,70);
    [ShowIf("_starSpawnType", EStarSpawnType.EYE), SerializeField] private int _totalEyeCount = 20;
    [ShowIf("_starSpawnType", EStarSpawnType.EYE), SerializeField] private float _eyeApparitionDuration = 10f;
    [ShowIf("_starSpawnType", EStarSpawnType.EYE), SerializeField] private AnimationCurve _eyeInvervalCurve;

    [Header("- SHOOTING STARS SPAWN PARAMETERS -")]
    [ShowIf("_starSpawnType", EStarSpawnType.SHOOTING_STAR), SerializeField] private GameObject _shootingStarPrefab;
    [ShowIf("_starSpawnType", EStarSpawnType.SHOOTING_STAR), SerializeField] private int _totalShootingStarCount = 20;
    [MinMaxSlider(5.0f, 100.0f)]
    [ShowIf("_starSpawnType", EStarSpawnType.SHOOTING_STAR), SerializeField] private Vector2 _minMaxShootingStarSphereRadius = new(20, 70);
    [ShowIf("_starSpawnType", EStarSpawnType.SHOOTING_STAR), SerializeField] private float _shootingStarApparitionDuration = 10f;
    [ShowIf("_starSpawnType", EStarSpawnType.SHOOTING_STAR), SerializeField] private AnimationCurve _shootingStarInvervalCurve;
    [ShowIf("_starSpawnType", EStarSpawnType.SHOOTING_STAR), SerializeField] private Transform _shootingStarTarget;
    
    [Header("- SPHERE FRACTAL SPAWN PARAMETERS -")] 
    [ShowIf("_starSpawnType", EStarSpawnType.SPHERE_FRACTAL), SerializeField] private GameObject _circleFractalPrefab;
    [ShowIf("_starSpawnType", EStarSpawnType.SPHERE_FRACTAL), SerializeField] private int _sphereFractalCount = 40;
    [ShowIf("_starSpawnType", EStarSpawnType.SPHERE_FRACTAL), SerializeField] private Transform _sphereSpawnPoint;

    private bool _isShootingStars;
    private bool _isExecuted;

    private enum EStarSpawnType{
        EYE,
        SHOOTING_STAR,
        SPHERE_FRACTAL
    }

    public void StartShootingStarApparition(GameObject prefab, int count)
    {
        StartCoroutine(_ShootingStarApparition(prefab, count));
    }

    public void StopShootingStarApparition() => _isShootingStars = false;

    IEnumerator _ShootingStarApparition(GameObject prefab, int count)
    {
        float startTime = Time.time;
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = Random.onUnitSphere * Random.Range(_minMaxShootingStarSphereRadius.x, _minMaxShootingStarSphereRadius.y);
            Instantiate(prefab, pos, Quaternion.identity);
            yield return new WaitForSeconds(_shootingStarApparitionDuration / _totalEyeCount * _shootingStarInvervalCurve.Evaluate((Time.time - startTime) / _shootingStarApparitionDuration));
        }
        _isExecuted = true;
    }

    public void SpawnSphereFractal(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            float y = 360.0f * ((float)i / (float)count);
            GameObject circle =  Instantiate(prefab, _sphereSpawnPoint.position, Quaternion.identity);
            circle.transform.SetParent(_sphereSpawnPoint, true);
            circle.transform.rotation = Quaternion.Euler(0, y, 0);
        }
    }

    public void StartEyeApparition(GameObject prefab, int count)
    {
        StartCoroutine(_EyeApparition(prefab, count));
    }

    IEnumerator _EyeApparition(GameObject prefab, int count)
    {
        var points = _GetEvenPointsOnSphere(count);
        float startTime = Time.time;
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = points[i];
            Instantiate(prefab, pos, Quaternion.identity);
            yield return new WaitForSeconds(_eyeApparitionDuration/_totalEyeCount * _eyeInvervalCurve.Evaluate((Time.time - startTime) / _eyeApparitionDuration));
        }
        _isExecuted = true;
    }

    Vector3[] _GetEvenPointsOnSphere(int nPoints)
    {
        float fPoints = (float)nPoints;

        Vector3[] points = new Vector3[nPoints];

        float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
        float off = 2 / fPoints;

        for (int k = 0; k < nPoints; k++)
        {
            float y = k * off - 1 + (off / 2);
            float r = Mathf.Sqrt(1 - y * y);
            float phi = k * inc;

            points[k] = new Vector3(Mathf.Cos(phi) * r, y, Mathf.Sin(phi) * r) * Random.Range(_minMaxEyeSphereRadius.x, _minMaxEyeSphereRadius.y);
        }

        return points;
    }

    protected override void ExecuteSpecific()
    {
        switch (_starSpawnType)
        {
            case EStarSpawnType.EYE:
                StartEyeApparition(_eyePrefab, _totalEyeCount);
                break;
            case EStarSpawnType.SHOOTING_STAR:
                StartShootingStarApparition(_shootingStarPrefab, _totalShootingStarCount);
                break;
            case EStarSpawnType.SPHERE_FRACTAL:
                SpawnSphereFractal(_circleFractalPrefab, _sphereFractalCount);
                break;
            default:
                break;
        }

    }
    protected override bool IsFinishedSpecific()
    {
        return _isExecuted;
    }

    public override string BuildGameObjectName()
    {
        return $"STAR SPAWN : {_starSpawnType}";
    }
}
