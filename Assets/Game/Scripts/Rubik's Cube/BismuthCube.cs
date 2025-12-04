using System.Collections.Generic;
using UnityEngine;

public class BismuthCube : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _outlineObject;
    [SerializeField] private List<GameActionsSequencer> _sequencers;
    
    public void OnInteract()
    {
        Destroy(_outlineObject);
        gameObject.layer = LayerMask.NameToLayer("Default");
        
        _sequencers.ForEach(seq => seq.Play());
    }

    public void SetOutline(bool state)
    {
        _outlineObject.SetActive(state);
    }
    
}