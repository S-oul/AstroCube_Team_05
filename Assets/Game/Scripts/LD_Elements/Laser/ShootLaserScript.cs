using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaserScript : MonoBehaviour
{
    [SerializeField] private Material _material;
    private LaserBeam _beam;
    private bool _isLaserActive = false;

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

        if (_isLaserActive)
        {
            Destroy(GameObject.Find("Laser Beam"));
            _beam = new LaserBeam(gameObject.transform.position, gameObject.transform.right, _material);
        }
    }

    private void ActivateLaser()
    {
        _isLaserActive = true;
    }

    private void DeactivateLaser()
    {
        _isLaserActive = false;
        Destroy(GameObject.Find("Laser Beam"));
    }
}
