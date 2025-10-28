using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MemoryObject : MonoBehaviour, IInteractable
{
    [SerializeField] private MemoryCharacter _memoryCharacterPrefab;
    [SerializeField] private List<Vector3> _characterPositions = new();
    [SerializeField] private List<string> _subtitles = new();
    [SerializeField] float _subtitleDurationByLetter;
    
    private List<MemoryCharacter> _characters = new();

    private void Awake()
    {
        foreach (Vector3 position in _characterPositions)
        {
            MemoryCharacter memChar = Instantiate(_memoryCharacterPrefab, transform.position + position, Quaternion.identity);
            memChar.Init(transform.position, transform.position + position);
            memChar.gameObject.SetActive(false);
            _characters.Add(memChar);
        }
    }

    private IEnumerator StartMemory()
    {
        foreach (MemoryCharacter memChar in _characters)
        {
            memChar.gameObject.SetActive(true);
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
        foreach (Vector3 characterPosition in _characterPositions)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + characterPosition, 0.1f);
        }
    }

    public void OnInteract()
    {
        StartCoroutine(StartMemory());
    }
}
