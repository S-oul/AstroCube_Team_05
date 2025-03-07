using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricCircuitScript : MonoBehaviour
{
    [SerializeField] private GameObject previousElectricPoint;
    [SerializeField] private GameObject nextElectricPoint;

    [SerializeField] private GameObject Editor_InputVisual;
    [SerializeField] private GameObject Editor_OutputVisual;

    private void Awake()
    {
        if (Editor_InputVisual != null)
        {
            Editor_InputVisual.SetActive(false);
        }
        if (Editor_OutputVisual != null)
        {
            Editor_OutputVisual.SetActive(false);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ElectricPoint"))
        {
            Debug.Log("Collision avec un point electrique");
            if (previousElectricPoint != null)
            {
                if (other.gameObject == previousElectricPoint)
                {
                    Debug.Log("Conexion avec le previous");
                }
            }
            if (nextElectricPoint != null)
            {
                if (other.gameObject == nextElectricPoint)
                {
                    Debug.Log("Conexion avec le next");
                }
            }
        }
    }
}
