using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/PositionSaveFile", fileName = "PositionSaveFile")]
public class PositionSaveFile : ScriptableObject
{
    public string SceneName;
    public List<GameObject> Objects = new();
    public List<Vector3> Positions = new();
    public List<Vector3> EulerAngles = new();
    public List<Vector3> Scales = new();
    public List<RightActionInfo> RightActionInfos = new();

    [Serializable]
    public class RightActionInfo
    {
        public RightActionInfo(GameObject objectRef, Pose pose)
        {
            _objectName = objectRef.name;
            _siblingIndex = objectRef.transform.GetSiblingIndex();
            _pose = pose;
        }

        public string ObjectName => _objectName;
        public int SiblingIndex => _siblingIndex;
        public Pose Pose => _pose;

        [SerializeField] private string _objectName;
        [SerializeField] private int _siblingIndex;
        [SerializeField] private Pose _pose;
    }
}
