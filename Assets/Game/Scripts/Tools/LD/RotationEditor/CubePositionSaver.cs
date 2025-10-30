using System.Collections.Generic;
using UnityEngine;

public class CubePositionSaver : MonoBehaviour
{
    private List<GameObject> _allCubeGameObjects = new();
    private List<Transform> _startTransforms = new();
    private List<Transform> _completedTransforms = new();
}
