using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFXManager : MonoBehaviour
{
    [SerializeField] private GameObject _starPrefab;
    [SerializeField] private int _num;
    [SerializeField] private float _radius;

    private void Start()
    {
        transform.LookAt(Camera.main.transform);
        SpawnStarsInCircle();
    }

    [Button("Spawn Stars In Circle")]
    public void SpawnStarsInCircle() => SpawnStarsInCircle(_num, transform.position, _radius, _starPrefab);
    public void SpawnStarsInCircle(int num, Vector3 point, float radius, GameObject starPrefab)
    {
        for (int i = 0; i < num; i++)
        {
            var radians = 2 * MathF.PI / num * i;

            var vertical = MathF.Sin(radians);
            var horizontal = MathF.Cos(radians);

            var spawnDir = new Vector3(horizontal, vertical, 0);

            var spawnPos = point + spawnDir * radius; 

            spawnPos = RotatePointAroundPivot(spawnPos, point, transform.localEulerAngles);

            StarMovement star = Instantiate(starPrefab, spawnPos , Quaternion.identity).GetComponent<StarMovement>();
            star.StartPos = spawnPos;
            star.transform.SetParent(transform, true);
        }
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
