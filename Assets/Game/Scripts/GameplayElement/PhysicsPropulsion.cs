using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPropulsion: MonoBehaviour
{
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _upPropulsionForce;
    [SerializeField] private float _sidePropulsionForce;
    private Tile _currentFloorTile;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        RaycastHit hit;
        LayerMask maskCube = LayerMask.GetMask("Cube");
        LayerMask maskTile = LayerMask.GetMask("Tile");

        Debug.DrawRay(_groundCheck.position, Vector3.down, Color.green, 20);

        if (Physics.Raycast(_groundCheck.position, Vector3.down, out hit, 3, maskTile))
        {
            if(_currentFloorTile)
                _currentFloorTile.IsOccupied = false;
            _currentFloorTile = hit.transform.GetComponent<Tile>();
            _currentFloorTile.IsOccupied = true;

            _currentFloorTile.OnPropulsion += OnPropulse;
        }
    }

    private void OnPropulse(Vector3 dir)
    {
        StartCoroutine(WaitForPropulsion(dir * _sidePropulsionForce + 
                                        ((transform.position - _currentFloorTile.transform.position).normalized * _upPropulsionForce)));
    }

    IEnumerator WaitForPropulsion(Vector3 dir)
    {
        yield return null;
        _rb.AddForce(dir, ForceMode.VelocityChange);
    }

}
