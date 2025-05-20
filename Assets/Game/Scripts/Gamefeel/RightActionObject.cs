using NaughtyAttributes;
using System.Linq;
using UnityEngine;

public class RightActionObject : MonoBehaviour
{
    public Pose RightPose { get => _rightPose; set => _rightPose = value; }
    [SerializeField, ReadOnly] private Pose _rightPose;

    public Pose GetActualPose()
    {
        Pose pose = new()
        {
            position = transform.localPosition,
            rotation = transform.localRotation,
        };
        return pose;
    }

    private void Awake()
    {
        var infos = PlayModePositionSaver.PositionsSave;
        if (infos == null || infos.RightActionInfos == null)
        {
            //Debug.LogWarning("You must save object in their final positions");
            return;            
        }
        else if(infos.RightActionInfos == null)
        {
            Debug.LogWarning("You must save object in their final positions");
            return;
        }
        var info = infos.RightActionInfos.FirstOrDefault(x => x.ObjectRef == gameObject);
        if(info != null)
            _rightPose = info.Pose;
    }
    private void OnEnable()
    {
        EventManager.OnEndCubeRotation += CheckIsTheRightPose;
    }
    private void OnDisable()
    {
        EventManager.OnEndCubeRotation -= CheckIsTheRightPose;
    }
    void /*bool*/ CheckIsTheRightPose()
    {
        if((Vector3.Distance(_rightPose.position,transform.localPosition) < Vector3.kEpsilon) && (Quaternion.Dot(_rightPose.rotation,transform.localRotation ) > 1 - Quaternion.kEpsilon))
        {
            //print("SAMEPOSE");
            return;// true;
        }
        else
        {
            //print("NOPECONNARD");
            return;// false;
        }
    }
}
