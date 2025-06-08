using UnityEngine;
using UnityEngine.Events;

public class InteractLine : MonoBehaviour, IHoldable
{
    [SerializeField] private EntitySequenceManager _entityOverlay;
    public UnityEvent onPlayerActivate;
    public void OnHold(Transform newParent)
    {
        print("caca");
        _entityOverlay.gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        return;
    }

    public Transform GetTransform()
    {
        return null;
    }

    public Transform GetOriginalParent()
    {
        return null;
    }
}
