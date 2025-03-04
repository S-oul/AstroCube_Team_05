using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DetectNewParent : MonoBehaviour
{
    [SerializeField] LayerMask _detectableLayer;
    Vector3 currentRotationDir;

    #region  raycast (old) 
    //private void Update()
    //{
    //    RaycastHit _raycastInfo;

    //    if (Physics.Raycast(transform.position, -transform.up, out _raycastInfo, 10, _detectableLayer))
    //    {

    //        var h = _raycastInfo.collider.transform;
    //        var dif = transform.parent.up - (-h.right);
    //        if (Mathf.Abs(dif.magnitude) > 0.1f)
    //        {
    //            Debug.Log(transform.parent.up + " is not " + -h.right);
    //            //Debug.Log("I'm tilted!");
    //            transform.parent.rotation =Quaternion.Lerp(transform.parent.rotation, Quaternion.FromToRotation(transform.parent.up, -h.right) * transform.parent.rotation,.2f);
    //        }
    //        transform.parent.SetParent(_raycastInfo.collider.gameObject.transform);
    //    }
    //}
    #endregion

    private void Start()
    {
        currentRotationDir = transform.up;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("collided with " + hit.gameObject.name);
        //transform.up = currentRotationDir;

        var h = hit.collider.transform;
        var dif = transform.up - (-h.right);
        if (Mathf.Abs(dif.magnitude) > 0.1f)
        {
            Debug.Log(transform.up + " is not " + -h.right);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, -h.right) * transform.rotation, 1f);
            //transform.rotation = Quaternion.FromToRotation(transform.up, -h.right);
            Debug.Log("new up is: " + transform.up);
        }
        transform.SetParent(hit.collider.gameObject.transform);
    }
}
