using System.Collections;
using UnityEngine;

public class ArtRubiksAnimator : MonoBehaviour
{
    [SerializeField] float _delay = .1f;
    [SerializeField] Animator animatorCube;

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
        //StartCoroutine(waitforXToStartIdle(_delay));
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

    bool isSelected = false;
    public void SetSelectedBool(bool isIt)
    {
        animatorCube.SetBool("IsSelected2", isIt);
        if (isIt && isSelected == false)
        {
            isSelected = true;
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
        else
        {
            isSelected = false;
        }
    }
}
