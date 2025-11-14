using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using Path = DG.Tweening.Plugins.Core.PathCore.Path;

public class MemoryObject : MonoBehaviour, IInteractable
{
    //[SerializeField] private MemoryCharacter _memoryCharacterPrefab;
    //[SerializeField] private List<Vector3> _characterPositions = new();
    [SerializeField] private List<MemoryVFXController> _memories = new();
    [SerializeField] private GameObject _gameObjectToActivate;
    [SerializeField] private List<string> _subtitles = new();
    [SerializeField] float _subtitleDurationByLetter;
    
    public GameObject GameObjectToActivate => _gameObjectToActivate;
    
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
        if (_gameObjectToActivate)
        {
            _gameObjectToActivate.transform.SetParent(_memories[^1].transform);
        }
    }

    private IEnumerator StartMemory()
    {
        _wasPlayed = true;
        foreach (MemoryVFXController mem in _memories)
        {
            mem.StartVFX(_gameObjectToActivate);
            //memChar.gameObject.SetActive(true);
        }
        TMP_Text text = GameObject.Find("Subtitles").GetComponent<TMP_Text>();
        foreach (string subtitle in _subtitles)
        {
            text.text = subtitle;
            yield return new WaitForSeconds(_subtitleDurationByLetter * subtitle.Length);
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
        if(!_wasPlayed)
            StartCoroutine(StartMemory());
    }
}
