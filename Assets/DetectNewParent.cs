using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DetectNewParent : MonoBehaviour
{
    [SerializeField] LayerMask _detectableLayer;

    //private void OnTriggerEnter(Collider other)
    //{
    //    transform.parent.SetParent(other.transform);
    //}

    private void Update()
    {
        RaycastHit _raycastInfo;

        if (Physics.Raycast(transform.position, -transform.up, out _raycastInfo, 10, _detectableLayer))
        {

            var h = _raycastInfo.collider.transform;
            var dif = transform.parent.up - (-h.right);
            if (Mathf.Abs(dif.magnitude) > 0.1f)
            {
                Debug.Log(transform.parent.up + " is not " + -h.right);
                //Debug.Log("I'm tilted!");
                transform.parent.rotation =Quaternion.Lerp(transform.parent.rotation, Quaternion.FromToRotation(transform.parent.up, -h.right) * transform.parent.rotation,.2f);
            }
            transform.parent.SetParent(_raycastInfo.collider.gameObject.transform.parent);
        }
    }
}
