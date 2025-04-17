using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewRubiksCube : MonoBehaviour
{
    [SerializeField] Material _previewMaterial;

    [Button("Prepare Preview RubiksCube")]
    public void PreparePreview()
    {
        RubiksMovement script = GetComponentsInChildren<RubiksMovement>()[0];
        script.IsPreview = true;

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var c in colliders)
        {
            c.enabled = false;
        }

        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (var m in meshes)
        {
            if(m.CompareTag("Floor"))
                m.gameObject.SetActive(false);
            else
            {
                m.material = _previewMaterial;
            }
        }
        foreach (Transform go in transform)
        {
            SelectionCube selection = go.GetComponent<SelectionCube>();
            if (selection == null) continue;
            selection.Select(SelectionCube.SelectionMode.DISABLE);
        }
    }
    private void Awake()
    {
        RubiksMovement script = GetComponentsInChildren<RubiksMovement>()[0];
        script.IsPreview = true;
    }
}
