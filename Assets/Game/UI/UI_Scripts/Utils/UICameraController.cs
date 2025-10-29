using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class UICameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] bool enableCameraTransition = true;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1.5f; 
    [SerializeField] private AnimationCurve easing = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine transitionCoroutine;
    private void OnEnable()
    {
        EventManager.OnViewShow += HandleViewShown;
    }

    private void OnDisable()
    {
        EventManager.OnViewShow -= HandleViewShown;
    }


    private void HandleViewShown(UIView view)
    {
        if (!enableCameraTransition || cinemachineVirtualCamera == null)
        {
            return;
        }

        if(view.CameraPosition!=null)
        {
            if(transitionCoroutine != null)
            {
                StopCoroutine(transitionCoroutine);
            }
            transitionCoroutine = StartCoroutine(SmoothTransition(view));
        }
    }

    private IEnumerator SmoothTransition(UIView view)
    {
        Transform cam = cinemachineVirtualCamera.transform;
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;

        Vector3 endPos = view.CameraPosition != null ? view.CameraPosition.position : startPos;
        Quaternion endRot = view.CameraPosition != null ? view.CameraPosition.rotation : startRot;

        if (view.CameraLookAt != null)
            cinemachineVirtualCamera.LookAt = view.CameraLookAt;

        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            float curvedT = easing.Evaluate(t);

            cam.position = Vector3.Lerp(startPos, endPos, curvedT);
            cam.rotation = Quaternion.Slerp(startRot, endRot, curvedT);

            yield return null;
        }

        cam.position = endPos;
        cam.rotation = endRot;

        view.Show();
    }
}

