using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using System.Linq;
using Unity.VisualScripting;


public class DoorBetweenCubes : MonoBehaviour
{
    [SerializeField] private float _waitRepeatDuration = 5.0f;
    [Header("Anim"), SerializeField] private float _durationMultiplier = 3f;
    /*
    [Header("Anim"), SerializeField] private float _inDuration = 0.5f;
    [SerializeField] private float _stayDuration = 2.0f;
    [SerializeField] private float _outDuration = 0.5f;
    */
    [SerializeField] private AnimationCurve _animationCurve;

    [Header("Detection"), SerializeField] private float _detectionDistance = 20f;
    [SerializeField] LayerMask _layerMask;
    private Coroutine _coroutine;
    private Renderer _currentRenderer;

    private void OnEnable()
    {
        EventManager.OnEndCubeRotation += TryFindWallBehindDoor;
        TryFindWallBehindDoor();
    }
    private void OnDisable()
    {
        EventManager.OnEndCubeRotation -= TryFindWallBehindDoor;
    }

    private void TryFindWallBehindDoor()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        RaycastHit _raycastInfo;
        Debug.DrawRay(transform.GetComponent<Renderer>().bounds.center, transform.forward * _detectionDistance, Color.magenta, 20);

        if (Physics.Raycast(transform.GetComponent<Renderer>().bounds.center, transform.forward, out _raycastInfo, _detectionDistance, _layerMask))
        {
            var floor = _raycastInfo.collider.transform.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.CompareTag("Floor"));
            if (floor == null) return;
            _currentRenderer = floor.GetComponent<Renderer>();
            _coroutine = StartCoroutine(DissolveAnim(_currentRenderer));
        }
    }

    private IEnumerator DissolveAnim(Renderer renderer)
    {
        DOTween.To(() => 0.0f, x => renderer.material.SetFloat("_DissolveDelta", x), 2.0f, _durationMultiplier).SetEase(_animationCurve);
        //yield return new WaitForSeconds(_stayDuration);
        //DOTween.To(() => renderer.material.GetFloat("_DissolveDelta"), x => renderer.material.SetFloat("_DissolveDelta", x), 0.0f, _outDuration).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(_waitRepeatDuration);
        _coroutine = StartCoroutine(DissolveAnim(renderer));
    }
}
