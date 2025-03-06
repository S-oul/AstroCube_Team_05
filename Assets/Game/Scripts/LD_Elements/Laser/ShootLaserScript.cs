using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaserScript : MonoBehaviour
{
    [SerializeField] private Material material;
    private LaserBeam beam;
    private bool isLaserActive = false;

    private void OnEnable()
    {
        EventManager.OnButtonPressed += ActivateLaser;
        EventManager.OnButtonReleased += DeactivateLaser;
    }

    private void OnDisable()
    {
        EventManager.OnButtonPressed -= ActivateLaser;
        EventManager.OnButtonReleased -= DeactivateLaser;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ActivateLaser();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            DeactivateLaser();
        }

        if (isLaserActive)
        {
            Destroy(GameObject.Find("Laser Beam"));
            beam = new LaserBeam(gameObject.transform.position, gameObject.transform.right, material);
        }
    }

    private void ActivateLaser()
    {
        isLaserActive = true;
    }

    private void DeactivateLaser()
    {
        isLaserActive = false;
        Destroy(GameObject.Find("Laser Beam"));
    }
}
