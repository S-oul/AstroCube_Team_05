using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCutsceneManager : MonoBehaviour
{
    [Header("Eye Apparition")]
    [SerializeField] private GameObject _eyePrefab;
    [SerializeField] private int _totalEyeCount = 20;
    [MinMaxSlider(5.0f, 100.0f)]
    [SerializeField] private Vector2 _minMaxEyeSphereRadius;
    [SerializeField] private float _eyeApparitionDuration = 10;
    [SerializeField] private AnimationCurve _eyeSpeedCurve;

    void Start()
    {
        StartEyeApparition(_eyePrefab, _totalEyeCount);
    }

    void StartEyeApparition(GameObject prefab, int count)
    {
        StartCoroutine(EyeApparition(prefab, count));
    }

    IEnumerator EyeApparition(GameObject prefab, int count)
    {
        var points = GetEvenPointsOnSphere(count);
        float startTime = Time.time;
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = points[i];
            Instantiate(prefab, pos, Quaternion.identity);
            yield return new WaitForSeconds(_eyeApparitionDuration/_totalEyeCount * _eyeSpeedCurve.Evaluate((Time.time - startTime) / _eyeApparitionDuration));
        }
    }

    Vector3[] GetEvenPointsOnSphere(int nPoints)
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
}
