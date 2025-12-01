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
            
            if (obj.layer != floorLayer)
                continue;
            
            EditorUtility.SetDirty(obj);
            
            if (obj.GetComponent<FloorType>() != null)
                continue;
            
            obj.AddComponent<FloorType>();
            
            if (obj.GetComponentInChildren<Collider>() != null)
                continue;
            
            BoxCollider col = obj.AddComponent<BoxCollider>();
            col.isTrigger = true;
            
            AssetDatabase.SaveAssetIfDirty(obj);
        }
    }
}
