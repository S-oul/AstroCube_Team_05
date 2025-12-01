using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Path = DG.Tweening.Plugins.Core.PathCore.Path;
using FMODUnity;

public class MemoryObject : MonoBehaviour, IInteractable
{
    //[SerializeField] private MemoryCharacter _memoryCharacterPrefab;
    //[SerializeField] private List<Vector3> _characterPositions = new();
    [SerializeField] private List<MemoryVFXController> _memories = new();
    [SerializeField] private List<GameObject> _gameObjectsToActivate;
    [SerializeField] private List<SubtitleData> _subtitles = new();

    [Header("FMOD")]
    [SerializeField] private EventReference _startMemoryEvent;
    [SerializeField] private EventReference _stopMemoryEvent;
    [SerializeField] private float _delayBeforeStopEvent = 3f;

    [SerializeField] public UnityEvent OnMemoryInteracted, OnCharacterAnimationFinished, OnAnimationFinished;
    
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
        foreach (GameObject obj in _gameObjectsToActivate)
        {
            if (obj.TryGetComponent(out MeshRenderer mesh))
            {
                mesh.materials = new Material[] { AssetDatabase.LoadAssetAtPath<Material>("Assets/Game/Art/VFX/Memories/Materials/M_MemoryElement.mat") };
                mesh.enabled = true;
            }
        }
    }

    private IEnumerator StartMemory()
    {
        _wasPlayed = true;
        
        if (!_startMemoryEvent.IsNull) RuntimeManager.PlayOneShot(_startMemoryEvent, transform.position);

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
        }
        
        foreach (SubtitleData subtitle in _subtitles)
        {
            Vector3 soundPos = transform.position;
            if (_memories.Count > subtitle.characterIndex && _memories[subtitle.characterIndex] != null)
            {
                soundPos = _memories[subtitle.characterIndex].transform.position;
            }

            if (!subtitle._voiceLineEvent.IsNull) RuntimeManager.PlayOneShot(subtitle._voiceLineEvent, soundPos);
            LocalizationManager.Instance.PrintStringFromID(subtitle.csvName, subtitle.localizationID, subtitle.locutor, subtitle.color);
            yield return new WaitForSeconds(subtitle.duration);
        }
        LocalizationManager.Instance.ClearString();
        
        yield return new WaitForSeconds(_delayBeforeStopEvent);

        if (!_stopMemoryEvent.IsNull) RuntimeManager.PlayOneShot(_stopMemoryEvent, transform.position);
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
        if (!_wasPlayed)
        {
            StartCoroutine(StartMemory());
            OnMemoryInteracted?.Invoke();
        }
    }
}

[Serializable]
public struct SubtitleData
{
    public string locutor;
    public string csvName;
    public string localizationID;
    public float duration;
    public Color color;
    public EventReference _voiceLineEvent;
    public int characterIndex;
}
