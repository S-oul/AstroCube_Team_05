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
        StartCoroutine(waitforXToStartIdle(_delay));
    }

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }

    public void StartAnimRota()
    {
        animatorCube.speed = 1 / GameManager.Instance.Settings.RubikscCubeAxisRotationDuration;
        animatorFx.speed = 1 / GameManager.Instance.Settings.RubikscCubeAxisRotationDuration;
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

    void StartAnimIdle()
    {
        animatorCube.SetTrigger("StartAnim");
    }
    IEnumerator waitforXToStartIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartAnimIdle();
    }

    public void SetSelectedTrigger()
    {
        animatorCube.SetTrigger("IsSelected");
    }

}
