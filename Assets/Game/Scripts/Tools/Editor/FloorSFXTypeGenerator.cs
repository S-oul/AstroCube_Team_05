using UnityEditor;
using UnityEngine;

public class FloorSFXTypeGenerator : MonoBehaviour
{

    [MenuItem("Tools/SFX/Regenerate Floor Types")]
    private static void GenerateFloorTypes()
    {
        foreach (GameObject obj in FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
        {
            int floorLayer = LayerMask.NameToLayer("Floor");
            int tileLayer = LayerMask.NameToLayer("Tile");
            
            if (obj.layer != floorLayer && obj.layer != tileLayer)
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
