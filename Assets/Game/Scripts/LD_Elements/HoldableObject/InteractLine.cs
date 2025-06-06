using UnityEngine;

public class InteractLine : MonoBehaviour, IHoldable
{
    public void OnHold(Transform newParent)
    {
        print("caca");
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
