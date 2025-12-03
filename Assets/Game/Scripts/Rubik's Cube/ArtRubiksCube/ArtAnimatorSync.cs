using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ArtAnimatorSync : MonoBehaviour
{
    List<ArtRubiksAnimator> AllAnimator = new();
    private void Awake()
    {
        AllAnimator = GetComponentsInChildren<ArtRubiksAnimator>().ToList();
        
    }


    public int NumberOfFrameToSync = 10;

    public float CheckMaxTime()
    {
        float max = 0;
        foreach (var item in AllAnimator)
        {
            max = Mathf.Max(item.TimeLeftBeforeEndAnim(), max);
        }

        print(max);
        return max;
    }

    public float CalculateMultValue(Animator Anim1, Animator Anim2)
    {
        float A = Loop(Anim1.GetCurrentAnimatorStateInfo(0).normalizedTime);
        float B = Loop(Anim2.GetCurrentAnimatorStateInfo(0).normalizedTime);

        float distance = LoopDistance(A, B);

        if (distance == 0f) return 1f;

        float speed = distance / (Time.deltaTime * NumberOfFrameToSync);

        return Mathf.Clamp(speed, 0.01f, 10f);
    }

    float Loop(float x)
    {
        x %= 1f;
        return (x < 0f) ? x + 1f : x;
    }

    float LoopDistance(float x, float y)
    {
        if (y >= x) return y - x;
        return (1f - x) + y;
    }

    public IEnumerator ChangeAnimatorSpeeds(List<Animator> anims, float amount) 
    {
        foreach (var a in anims)
        {
            a.speed = amount;
        }

        yield return new WaitForSeconds(NumberOfFrameToSync * Time.deltaTime);
        foreach (var a in anims)
        {
            a.speed = 1;
        }
        Debug.Log("Synced! :" + amount + " ... " + anims.Count);
    }
}
