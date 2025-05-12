using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFXManager : MonoBehaviour
{
    public GameObject starPrefab;
    public int _num;
    public float _radius;

    private void Start()
    {
        SpawnStarsInCircle();
    }
    [Button("Spawn Stars In Circle")]
    public void SpawnStarsInCircle() => SpawnStarsInCircle(_num, transform.position, _radius);
    public void SpawnStarsInCircle(int num, Vector3 point, float radius)
    {

        for (int i = 0; i < num; i++)
        {

            /* Distance around the circle */
            var radians = 2 * MathF.PI / num * i;

            /* Get the vector direction */
            var vertical = MathF.Sin(radians);
            var horizontal = MathF.Cos(radians);

            var spawnDir = new Vector3(horizontal, vertical, 0);

            /* Get the spawn position */
            var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

            /* Now spawn */
            StarFXMovement star = Instantiate(starPrefab, spawnPos, Quaternion.identity).GetComponent<StarFXMovement>();

            star.StartPos = spawnPos;
            //star.Tangeant = GetCircleTangents(spawnPos, transform.position, radius);
            //Debug.Log(star.Tangeant);
            star.transform.SetParent(transform, false);
        }
    }
}
