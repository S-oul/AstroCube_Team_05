using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLACEHOLDER_DemoSceneLeopScript : MonoBehaviour
{

    [SerializeField] private GameObject buttonPlateformGameObject;
    [SerializeField] private GameObject laserGameObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Destroy(buttonPlateformGameObject);
        }

        if (Input.GetKey(KeyCode.X))
        {
            laserGameObject.transform.Rotate(0,0.05f,0);
        }

        if (Input.GetKey(KeyCode.C))
        {
            laserGameObject.transform.Rotate(0, -0.05f, 0);
        }
    }
}
