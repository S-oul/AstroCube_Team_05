using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class TransformState
{
    public Vector3 localPosition;
    public Quaternion localRotation;
    
    public TransformState(Transform transform)
    {
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;
    }
}

public class CubePositionSaver : MonoBehaviour
{
    [SerializeField] private GameObject _cube;
    
    [SerializeField, ReadOnly] private List<GameObject> _allStartCubeGameObjects = new();
    [SerializeField, ReadOnly] private List<TransformState> _startTransformStates = new();
    [SerializeField, ReadOnly] private List<GameObject> _allCompletedCubeGameObjects = new();
    [SerializeField, ReadOnly] private List<TransformState> _completedTransformStates = new();
    
    public int StartPositionSavedCount => _allStartCubeGameObjects.Count;
    public int CompletedPositionSavedCount => _allCompletedCubeGameObjects.Count;

#if UNITY_EDITOR
    public void SaveStartCubeState()
    {
        _allStartCubeGameObjects = new();
        _startTransformStates = new();
        
        for (int i = 0; i < _cube.transform.childCount; i++)
        {
            var child = _cube.transform.GetChild(i);
            _allStartCubeGameObjects.Add(child.gameObject);
            _startTransformStates.Add(new TransformState(child));
            
            for (int j = 0; j < child.childCount; j++)
            {
                var subChild = child.GetChild(j);
                _allStartCubeGameObjects.Add(subChild.gameObject);
                _startTransformStates.Add(new TransformState(subChild));
            }
        }
        
        EditorUtility.SetDirty(this);
    }
    
    public void SaveCompletedCubeState()
    {
        _allCompletedCubeGameObjects = new();
        _completedTransformStates = new();
        
        for (int i = 0; i < _cube.transform.childCount; i++)
        {
            var child = _cube.transform.GetChild(i);
            _allCompletedCubeGameObjects.Add(child.gameObject);
            _completedTransformStates.Add(new TransformState(child));
            
            for (int j = 0; j < child.childCount; j++)
            {
                var subChild = child.GetChild(j);
                _allCompletedCubeGameObjects.Add(subChild.gameObject);
                _completedTransformStates.Add(new TransformState(subChild));
            }
        }
        
        EditorUtility.SetDirty(this);
    }
    
    public Dictionary<GameObject, TransformState> GetStartCubeState()
    {
        Dictionary<GameObject, TransformState> startState = new();
        for (int i = 0; i < _allStartCubeGameObjects.Count; i++)
        {
            startState.Add(_allStartCubeGameObjects[i], _startTransformStates[i]);
        }
        return startState;
    }
    
    public Dictionary<GameObject, TransformState> GetCompletedCubeState()
    {
        Dictionary<GameObject, TransformState> completedState = new();
        for (int i = 0; i < _allCompletedCubeGameObjects.Count; i++)
        {
            completedState.Add(_allCompletedCubeGameObjects[i], _completedTransformStates[i]);
        }
        return completedState;
    }
#endif
}