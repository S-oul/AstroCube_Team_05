using UnityEngine;

public class GameActionActivateGameObject : AGameAction
{
    [SerializeField] private GameObject _targetGameObject;

    protected override void ExecuteSpecific()
    {
        if (_targetGameObject == null) return;
        _targetGameObject.SetActive(true);
    }

    public override string BuildGameObjectName()
    {
        string strGameObject = "[GameObject]";
        if (_targetGameObject != null) {
            strGameObject = _targetGameObject.name;
        }

        return $"ACTIVATE {strGameObject}";
    }
}