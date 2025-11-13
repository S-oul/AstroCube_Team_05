using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class MakeTileInvisible : MonoBehaviour
{
#if UNITY_EDITOR
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


        Camera sceneCam = sceneView.camera;
        if (sceneCam == null) { 
            return; 
        }

        Vector3 viewDirection = (sceneCam.transform.position - transform.position).normalized;

        Vector3 forwardDirect = transform.forward;

        float dot = Vector3.Dot(forwardDirect, viewDirection);

        if (tileParent == null) 
        {
            return;
        }

        if (dot > 0)
        {
            SceneVisibilityManager.instance.Show(tileParent, true);
        }
        else
        {
            SceneVisibilityManager.instance.Hide(tileParent, true);
        }
    }
#endif
}
