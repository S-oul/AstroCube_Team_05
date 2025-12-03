using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ArtAnimatorSync : MonoBehaviour
{
    List<ArtRubiksAnimator> AllAnimator = new();
    private void Awake()
    {
        AllAnimator = GetComponentsInChildren<ArtRubiksAnimator>().ToList();
        
    }

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
}
