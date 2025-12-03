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

        if (stateA.length <= 0 || stateB.length <= 0 || syncInNSeconds <= 0)
        {
            return 1.0f;
        }

        float clipDuration = stateA.length;

        float PA = stateA.normalizedTime % 1.0f;
        float PB = stateB.normalizedTime % 1.0f;

        float normalizedDiff = PA - PB;

        if (normalizedDiff < 0)
        {
            normalizedDiff += 1.0f;
        }

        float T_diff = normalizedDiff * clipDuration;

        float S_B_new = T_diff / syncInNSeconds;

        return Mathf.Clamp(S_B_new, 0.01f, 10f);
    }

    public IEnumerator ChangeAnimatorSpeeds(List<Animator> anims, float amount)
    {
        foreach (var a in anims)
        {
            a.speed = amount;
        }

        yield return new WaitForSeconds(syncInNSeconds);

        foreach (var a in anims)
        {
            a.speed = 1f;
        }
        Debug.Log($"Animators synced! Speed: {amount:F2} over {syncInNSeconds:F2} seconds.");
    }
}