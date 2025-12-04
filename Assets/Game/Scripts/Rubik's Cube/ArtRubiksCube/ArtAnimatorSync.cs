using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArtAnimatorSync : MonoBehaviour
{
    public float syncInNSeconds = 0.2f;

    public float CalculateMultValue(Animator A, Animator B)
    {
        AnimatorStateInfo stateA = A.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo stateB = B.GetCurrentAnimatorStateInfo(0);

        print("####### " + stateA.normalizedTime + " " + stateB.normalizedTime);

        float PA = stateA.normalizedTime % 1.0f;
        float PB = stateB.normalizedTime % 1.0f;


        float normalizedDiff = Mathf.Abs(PA - PB);


        float T_diff = normalizedDiff + (PB + Time.deltaTime * syncInNSeconds);

        float S_B_new = T_diff / syncInNSeconds;

        return Mathf.Clamp(S_B_new, 0.01f, 10f);
    }

    public IEnumerator ChangeAnimatorSpeeds(List<Animator> anims, Animator animToFollow)
    {

        foreach (var a in anims)
        {
            a.speed = 1.8f;
        }

        while (Mathf.Abs(anims[0].GetCurrentAnimatorStateInfo(0).normalizedTime % 1 - animToFollow.GetCurrentAnimatorStateInfo(0).normalizedTime % 1) > 0.01)
        {
            yield return null;
        }

        foreach (var a in anims)
        {
            a.speed = 1f;
        }
    }
}