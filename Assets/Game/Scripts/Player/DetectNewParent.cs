using UnityEngine;

public class DetectNewParent : MonoBehaviour
{
    [SerializeField] LayerMask _detectableLayer;
    Vector3 currentRotationDir;

    [SerializeField] SelectionCube OldTilePlayerPos;
    Transform OldTilePlayerPosTransform;


    private bool _doGravityRotation;




    public bool DoGravityRotation { get => _doGravityRotation; set => _doGravityRotation = value; }



    private void Update()
    {
        RaycastHit _raycastInfo;
        if (Physics.Raycast(transform.position, -transform.up, out _raycastInfo, 10, _detectableLayer))
        {
            OldTilePlayerPosTransform = _raycastInfo.collider.transform;

            //if (OldTilePlayerPos) OldTilePlayerPos.SetIsTileLock(false);



            OldTilePlayerPos = _raycastInfo.transform.GetComponentInParent<SelectionCube>();
            if (OldTilePlayerPos)
            {
                print(OldTilePlayerPos);
                transform.SetParent(OldTilePlayerPos.transform, true);
            }
            //if(!OldTilePlayerPos) _raycastInfo.transform.parent.parent.TryGetComponent(out OldTilePlayerPos);

        }
        else
        {
            transform.SetParent(null, true);
        }

    }

    private void Start()
    {
        currentRotationDir = transform.up;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!_doGravityRotation) return;

        var h = hit.collider.transform;
        var dif = transform.up - (-h.right);
        if (Mathf.Abs(dif.magnitude) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, -h.right) * transform.rotation, 1f);
        }
        transform.SetParent(hit.collider.gameObject.transform);
    }
}
