using UnityEngine;

public class ChangeReset : MonoBehaviour
{
    [SerializeField] Transform _newResetPos;

    public Transform NewResetPos { get => _newResetPos; set => _newResetPos = value; }
}
