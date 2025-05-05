using System.Collections;
using UnityEngine;

public class ArtRubiksAnimator : MonoBehaviour
{
    [SerializeField] float _delay = .1f;
    Animator animator;

    void Awake()
    {
        //EventManager.OnEndCubeRotation += StartAnimIdle;

        animator = GetComponent<Animator>();
        StartCoroutine(waitforXToStartIdle(_delay));
    }

    ///Try to Reync da idle anim but failed miserably
    ///*private void OnDisable()
    //{
    //    EventManager.OnEndCubeRotation -= StartAnimIdle;
    //}*/

    public void StartAnimRota()
    {
        animator.speed = 1/GameManager.Instance.Settings.RubikscCubeAxisRotationDuration;
        animator.SetTrigger("DoRotation");
    }

    void StartAnimIdle()
    {
        animator.SetTrigger("StartAnim");
    }
    IEnumerator waitforXToStartIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartAnimIdle();
    }

}
