using UnityEditor;
using UnityEngine;

public class FloorSFXTypeGenerator : MonoBehaviour
{

    [MenuItem("Tools/SFX/Regenerate Floor Types")]
    private static void GenerateFloorTypes()
    {
        foreach (GameObject obj in FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
        {
            if (obj.layer != LayerMask.NameToLayer("Floor"))
                continue;
            
            if (obj.GetComponent<FloorType>() != null)
                continue;
            
            obj.AddComponent<FloorType>();
            
            if (obj.GetComponentInChildren<Collider>() != null)
                continue;
            
            BoxCollider col = obj.AddComponent<BoxCollider>();
            col.isTrigger = true;
        }
    }
}
