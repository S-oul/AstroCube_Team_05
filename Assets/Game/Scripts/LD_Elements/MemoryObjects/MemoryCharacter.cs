using UnityEngine;

public class MemoryCharacter : MonoBehaviour
{

    [SerializeField] private MemoryObjectController _controller;
    [SerializeField] private Transform _originTransform;
    [SerializeField] private Transform _meshTransform;

    public void Init(Vector3 originPosition, Vector3 meshPosition)
    {
        _originTransform.position = originPosition;
        _meshTransform.position = meshPosition;
        
        gameObject.SetActive(true);
        _controller.LinkOriginToVFX();
    }

}
