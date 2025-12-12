using System.Collections;
using UnityEngine;

public class ArtRubiksAnimator : MonoBehaviour
{
    [SerializeField] float _delay = .1f;
    public Animator animatorCube;

    [SerializeField] Animator animatorFx;

    [SerializeField] TypeFace _type;
    public enum TypeFace
    {
        Face,
        Edge,
        Coin
    }

    void Start()
    {
        animatorCube = GetComponent<Animator>();
    }
    public void StartAnimRota()
    {
        animatorCube.SetTrigger("DoRotation");

        switch (_type)
        {
            case TypeFace.Face:
                animatorFx.SetTrigger("FaceTrigger");
                break;
            case TypeFace.Edge:
                animatorFx.SetTrigger("EdgeTrigger");
                break;
            case TypeFace.Coin:
                animatorFx.SetTrigger("CoinTrigger");
                break;

        }
    }

    [SerializeField] bool isSelected = false;
    public float TimeLeftBeforeEndAnim()
    {
        var info = animatorCube.GetCurrentAnimatorStateInfo(0);
        //Switch might be more effective but idk idgaf ikms
        bool isName =
               info.IsName("Cube_Face_Selected")
            || info.IsName("Cube_Cote_Selected")
            || info.IsName("Cube_Coin_Selected");
        
        return isName ? info.normalizedTime%2f/2f: -1f;
    }

    public void LaunchAnimCoroutine(bool select, float timetoWait) => StartCoroutine(waitForToSelect(select, timetoWait));
    IEnumerator waitForToSelect(bool select, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SetSelectedBool(select);
    }
    public void SetSelectedBool(bool isIt)
    {
        if (!animatorCube)
            return;
        
        animatorCube.SetBool("IsSelected2", isIt);
        if (isIt && isSelected == false)
        {
            switch (_type)
            {
                case TypeFace.Face:
                    animatorCube.Play("Cube_Face_Selected");
                    break;
                case TypeFace.Edge:
                    animatorCube.Play("Cube_Cote_Selected");
                    break;
                case TypeFace.Coin:
                    animatorCube.Play("Cube_Coin_Selected");
                    break;
            }

        }
        isSelected = isIt;

    }
}
