using NaughtyAttributes;
using System.Linq;
using UnityEngine;

public class RightActionObject : MonoBehaviour
{
    public Pose RightPose { get => _rightPose; set => _rightPose = value; }
    [SerializeField, ReadOnly] private Pose _rightPose;
    private SelectionCube _selection;
    bool _isAlreadyInRightPose;

    public Pose GetActualPose()
    {
        Pose pose = new()
        {
            position = transform.localPosition,
            rotation = transform.localRotation,
        };
        return pose;
    }

    private void Start()
    {
        _selection = GetComponent<SelectionCube>();
        var infos = GameManager.Instance.RightActions;
        if (infos == null || infos.RightActionInfos == null)
        {
            return;
        }
        else if (infos.RightActionInfos == null)
        {
            Debug.LogWarning("You must save object in their final positions");
            return;
        }
        var info = infos.RightActionInfos.FirstOrDefault(x => x.ObjectName == gameObject.name);
        if (info != null)
        {
            _rightPose = info.Pose;
        }
        if (_IsTheRightPose())
        {
            _isAlreadyInRightPose = true;
        }
    }

    private void OnEnable()
    {
        EventManager.OnEndCubeRotation += CheckIsTheRightPose;
    }
    private void OnDisable()
    {
        EventManager.OnEndCubeRotation -= CheckIsTheRightPose;
    }

    void CheckIsTheRightPose()
    {
        if (!_isAlreadyInRightPose &&_IsTheRightPose())
        {
            if (_selection)
                _selection.StartCorrectActionAnim();
        }
    }

    bool _IsTheRightPose()
    {
        if ((Vector3.Distance(_rightPose.position,transform.localPosition) < 0.01f) && (Mathf.Abs(Quaternion.Dot(_rightPose.rotation,transform.localRotation )) > 1 - 0.01f))
            return true;
        else
            return false;
    }
}
