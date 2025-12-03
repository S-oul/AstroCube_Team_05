using UnityEngine;
using UnityEngine.Events;

public class ChangeReset : MonoBehaviour
{
    [SerializeField] Transform _newResetPos;

    [SerializeField] GameObject _arrow;

    public Transform NewResetPos { get => _newResetPos; set => _newResetPos = value; }

    private void Start()
    {
        _arrow.SetActive(false);
    }
}
