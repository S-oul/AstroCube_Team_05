using UnityEngine;

public class TransiTrigger : MonoBehaviour
{
    [SerializeField] AlphaGameObject _objectToFade;

    [SerializeField] bool _activateOnlyOnce = true;

    private bool _isActivable = true;
    private void OnEnable()
    {
        if (!_objectToFade) Debug.LogError("ObjectToFade (maybe Door) is not Set in the inspector");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActivable) return;

        _objectToFade.FadeFonction();

        if (_activateOnlyOnce) _isActivable = false;
    }
}
