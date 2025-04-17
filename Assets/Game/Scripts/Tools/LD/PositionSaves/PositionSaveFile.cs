using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Test", fileName = "Yippie")]
public class PositionSaveFile : ScriptableObject
{
    public List<GameObject> Objects;
    public List<Vector3> Positions;
    public List<Vector3> EulerAngles;
    public List<Vector3> Scales;
    public List<RightActionInfo> RightActionInfos;

    public class RightActionInfo
    {
        public RightActionInfo(GameObject objectRef, Pose pose)
        {
            _objectRef = objectRef;
            _pose = pose;
        }

        public GameObject ObjectRef => _objectRef;
        public Pose Pose => _pose;

        private GameObject _objectRef;
        private Pose _pose;
    }
}
