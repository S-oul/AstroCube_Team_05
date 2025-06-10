using NaughtyAttributes;
using System.Linq;
using UnityEngine;

public class RightActionObject : MonoBehaviour
{
    public string Name {  get; private set; }
    public int Index {  get; private set; } 

    private SelectionCube _selection;
    Pose _rightPose;

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
        Name = gameObject.name;
        Index = transform.GetSiblingIndex(); 
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
        var rightActionInfo = infos.RightActionInfos.FirstOrDefault(x => x.ObjectName == Name && x.SiblingIndex == Index); // This is NOT a viable long term solution for indentifying gameobjects
        if(rightActionInfo != null) 
            _rightPose = rightActionInfo.Pose;
    }

    public void Shine()
    {
        if (_selection)
            _selection.StartCorrectActionAnim();        
    }

    public bool IsTheRightPose()
    {
        if ((Vector3.Distance(_rightPose.position,transform.localPosition) < 0.01f) && (Mathf.Abs(Quaternion.Dot(_rightPose.rotation,transform.localRotation )) > 1 - 0.01f))
            return true;
        else
            return false;
    }
}
