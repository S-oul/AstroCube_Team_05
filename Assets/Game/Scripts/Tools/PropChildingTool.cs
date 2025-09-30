using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropChildingTool : MonoBehaviour
{
    [SerializeField] bool PrintInformativeMessage = true;
    [SerializeField] List<GameObject> CubeZoneObjects;

    // Identify where each object (prop) should be childed & assigning parent.  
    public void RunTool()
    {
        int NumOfProps = 0;
        int NumOfActiveProps = 0;
        int NumOfPropsMoved = 0;

        PropChildingToolOrganisable[] OrganisableObjects = Object.FindObjectsByType<PropChildingToolOrganisable>(FindObjectsSortMode.None);
        NumOfProps = OrganisableObjects.Length;

        foreach (PropChildingToolOrganisable prop in OrganisableObjects)
        {
            // skip inActive props
            if (prop.AllowPropChildingToolToOrganizeThisObject == false) continue;
            NumOfActiveProps++;

            // identify closest cube zone
            GameObject currentClosestObject = null;
            float currentShortestDistance = -1;

            foreach (GameObject cubeZone in CubeZoneObjects)
            {
                if (currentClosestObject == null ||
                    Vector3.Distance(prop.gameObject.transform.position, cubeZone.transform.position) < currentShortestDistance)
                {
                    currentClosestObject = cubeZone;
                    currentShortestDistance = Vector3.Distance(prop.gameObject.transform.position, cubeZone.transform.position);
                }
            }

            if (currentClosestObject == null) throw new System.Exception("Prop found no nearby cubezones. This should be imposible.");

            // assign parent to prop
            if (prop.transform.parent != currentClosestObject.transform)
            {
                prop.transform.SetParent(currentClosestObject.transform);
                NumOfPropsMoved++;
            }
        }
        if (PrintInformativeMessage)
        {
            Debug.Log("Number of props Found: " + NumOfProps
                + "\nNumber of Active props: " + NumOfActiveProps
                + "\nNumber of Organised props: " + NumOfPropsMoved);
        }
    }
}
