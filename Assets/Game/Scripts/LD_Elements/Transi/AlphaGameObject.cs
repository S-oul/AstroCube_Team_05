using NaughtyAttributes;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class AlphaGameObject : MonoBehaviour
{
    [SerializeField] GameObject _obj;
    [SerializeField] float timeToTransi = 3f;
    [SerializeField] float treshHoldForCollider = .2f;
    [SerializeField] bool MakeAppear = true;
    MeshRenderer objMat;
    Collider objCol;

    void Start()
    {
        objMat = _obj.GetComponent<MeshRenderer>();
        objCol = _obj.GetComponent<Collider>();
        
        
        objMat.materials[0].color = new Color(1, 1, 1, MakeAppear? 0:1);
        
        objCol.enabled = !MakeAppear;
        
    }


    Coroutine cor;
    [Button]
    public void FadeOut()
    {
        if (cor != null) return;
        objCol.enabled = false;

        cor = StartCoroutine(FadeOutObject());
    }

    public IEnumerator FadeOutObject()
    {
        float timeSinceStart = 0;
        float percent = 0;
        bool doOnce = false; // puisqu'on est sur unreal;

        while (timeSinceStart < timeToTransi)
        {
            timeSinceStart += Time.deltaTime;
            percent = timeSinceStart / timeToTransi;

            if (percent > treshHoldForCollider && !doOnce)
            {
                doOnce = true;
                objCol.enabled = MakeAppear;
            } 

            objMat.materials[0].color = new Color(1, 1, 1, MakeAppear? percent : 1 - percent);

            yield return new WaitForEndOfFrame();
        }
        
        objMat.materials[0].color = new Color(1, 1, 1, MakeAppear? 1:0);

        MakeAppear = !MakeAppear;   
        cor = null;
    }
}
