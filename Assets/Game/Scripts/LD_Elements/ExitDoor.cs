using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public bool _isDoorOpenAtStart = true;

    public static ExitDoor Instance => _instance;
    public static ExitDoor _instance;

    [SerializeField] private GameObject _door;

    private void Awake()
    {
        if (_instance) Destroy(this);
        else _instance = this;

        if (_isDoorOpenAtStart)
            OpenDoor();
        else
            CloseDoor();
    }

    public void OpenDoor()
    {
        _door.SetActive(true);
    }

    public void CloseDoor()
    {
        _door.SetActive(false);
    }
}
