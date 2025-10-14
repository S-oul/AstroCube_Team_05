using UnityEngine;
using UnityEditor;
using UnityEngine.WSA;
using System.Data;
using TMPro;

[ExecuteInEditMode]
public class MakeTileInvisible : MonoBehaviour
{
    [SerializeField] bool activated = true;
    [SerializeField] GameObject tileParent;
    [SerializeField] float updateInterval = 1f;
    float lastUpdated = 0;

    bool isReset = false;
    
    private void OnEnable()
    {
        SceneView.duringSceneGui += ManageTileVisibility;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= ManageTileVisibility;
    }

    void ManageTileVisibility(SceneView sceneView)
    {
        if (activated == false)
        {
            if (!isReset)
            {
                SceneVisibilityManager.instance.Show(tileParent, true);
                isReset = true;
            }
            return;
        }
        isReset = false;  

        if (EditorApplication.timeSinceStartup - lastUpdated < updateInterval) { return; }
        lastUpdated = (float)EditorApplication.timeSinceStartup;
        Debug.Log("updated");


        Camera sceneCam = sceneView.camera;
        if (sceneCam == null) { 
            return; 
        }

        Vector3 viewDirection = (sceneCam.transform.position - transform.position).normalized;

        Vector3 forwardDirect = transform.forward;

        float dot = Vector3.Dot(forwardDirect, viewDirection);

        if (dot > 0)
        {
            SceneVisibilityManager.instance.Show(tileParent, true);
        }
        else
        {
            SceneVisibilityManager.instance.Hide(tileParent, true);
        }
    }
}
