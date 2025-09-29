using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropChildingTool : MonoBehaviour
{
    List<GameObject> OrganisableObjects;

    // return an array of game objects in the scene that contain an ACTIVE PropChildingToolOrganisable component. 
    // objects are stored in OrganisableObjects. 
    void GetAllScenePropsInScene()
    {

    }

    // Identify where each object (prop) should be childed & assigning parent.  
    void RunTool()
    {
        GetAllScenePropsInScene();

        foreach (GameObject prop in OrganisableObjects)
        {
            // identify position zone
            // identify parent assosiated with zone
            // assign parent to prop
        }
    }
}
