using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Path = DG.Tweening.Plugins.Core.PathCore.Path;

public class MemoryObject : MonoBehaviour, IInteractable
{
    //[SerializeField] private MemoryCharacter _memoryCharacterPrefab;
    //[SerializeField] private List<Vector3> _characterPositions = new();
    [SerializeField] private List<MemoryVFXController> _memories = new();
    [SerializeField] private List<GameObject> _gameObjectsToActivate;
    [SerializeField] private List<SubtitleData> _subtitles = new();
    
    //private List<MemoryCharacter> _characters = new();

    private bool _wasPlayed;
    
    private void Awake()
    {
        /*
        foreach (Vector3 position in _characterPositions)
        {
            MemoryCharacter memChar = Instantiate(_memoryCharacterPrefab, transform.position + position, Quaternion.identity);
            memChar.Init(transform.position, transform.position + position);
            memChar.gameObject.SetActive(false);
            _characters.Add(memChar);
        }
        */
    }

    private void OnValidate()
    {
        for (var index = 0; index < _gameObjectsToActivate.Count; index++)
        {
            var gameObjectToActivate = _gameObjectsToActivate[index];

            gameObjectToActivate.transform.SetParent(_memories.Count > index
                ? _memories[index].transform
                : _memories[^1].transform);
        }
    }

    private IEnumerator StartMemory()
    {
        _wasPlayed = true;
        for (var index = 0; index < _memories.Count; index++)
        {
            var mem = _memories[index];
            try
            {
                mem.StartVFX(_gameObjectsToActivate[index]);
            }
            catch
            {
                mem.StartVFX(null);
            }
            //memChar.gameObject.SetActive(true);
        }

        TMP_Text text = GameObject.Find("Subtitles").GetComponent<TMP_Text>();
        foreach (SubtitleData subtitle in _subtitles)
        {
            text.text = subtitle.text;
            text.color = subtitle.color;
            yield return new WaitForSeconds(subtitle.duration);
        }

        text.text = "";
    }

    private void OnDrawGizmos()
    {
        /*
        foreach (Vector3 characterPosition in _characterPositions)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + characterPosition, 0.1f);
        }
        */
    }

    public void OnInteract()
    {
        Debug.Log("orh samuel");
        if(!_wasPlayed)
            StartCoroutine(StartMemory());
    }
}

[Serializable]
public struct SubtitleData
{
    public string text;
    public float duration;
    public Color color;
}
