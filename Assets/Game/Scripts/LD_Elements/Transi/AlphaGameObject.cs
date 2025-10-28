using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class AlphaGameObject : MonoBehaviour
{
    [SerializeField] GameObject ToDisappearObj;
    [SerializeField] GameObject ToAppearObj;

    [SerializeField] float TimeToTransi = 3f;

    MeshRenderer ToDisappear;
    MeshRenderer ToAppear;

    Collider ToDisappearCol;
    Collider ToAppearCol;

    void Start()
    {
        ToDisappear = ToDisappearObj.GetComponent<MeshRenderer>();
        ToAppear = ToAppearObj.GetComponent<MeshRenderer>();

        ToDisappearCol = ToDisappearObj.GetComponent<Collider>();
        ToAppearCol = ToAppearObj.GetComponent<Collider>();

        ToAppear.materials[0].color = new Color(1, 1, 1, 0);
        ToAppearCol.isTrigger = false;
    }

    [Button]
    void FadeOut()
    {
        StartCoroutine(FadeOutObject());
    }

    public IEnumerator FadeOutObject()
    {
        float timeSinceStart = 0;
        float percent = 0;


        while (timeSinceStart < TimeToTransi)
        {
            timeSinceStart += Time.deltaTime;
            percent = timeSinceStart / TimeToTransi;

            print(timeSinceStart + " / " + percent + "%");

            if (!ToDisappearCol.isTrigger && percent > 0.75f)
            {
                ToAppearCol.isTrigger = true;
                ToDisappearCol.isTrigger = false;
            } 

            
            ToDisappear.materials[0].color = new Color(1, 1, 1, 1 - percent);
            ToAppear.materials[0].color = new Color(1, 1, 1, percent);

            yield return new WaitForEndOfFrame();
        }

        ToAppear.materials[0].color = new Color(1, 1, 1, 1);
        ToDisappear.materials[0].color = new Color(1, 1, 1, 0);

    }
}
