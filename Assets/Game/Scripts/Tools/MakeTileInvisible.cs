using UnityEngine;
using UnityEditor;
using UnityEngine.WSA;

[ExecuteInEditMode]
public class MakeTileInvisible : MonoBehaviour
{
    //[SerializeField] bool activated = false;
    
    private void OnEnable()
    {
        Debug.Log("on enable"); 
        SceneView.duringSceneGui += ManageTileVisibility;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= ManageTileVisibility;
    }

    void ManageTileVisibility(SceneView sceneView)
    {
        //if (activated == false) return;

        MeshRenderer mesh = GetComponent<MeshRenderer>();

        Debug.Log("managing Visibility");
        Camera sceneCam = sceneView.camera;
        if (sceneCam == null) { 
            return; 
        }

        Vector3 viewDirection = (sceneCam.transform.position - transform.position).normalized;

        Vector3 forwardDirect = transform.forward;

        float dot = Vector3.Dot(forwardDirect, viewDirection);

        if (dot > 0) mesh.enabled = true;
        else mesh.enabled = false; 
    }
}
