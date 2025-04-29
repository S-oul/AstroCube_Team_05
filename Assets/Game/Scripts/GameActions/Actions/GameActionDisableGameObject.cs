using UnityEngine;

public class GameActionDisableGameObject : AGameAction
{
    [SerializeField] private GameObject _targetGameObject;

    protected override void ExecuteSpecific()
    {
        if (_targetGameObject == null) return;
        _targetGameObject.SetActive(false);
    }

    public override string BuildGameObjectName()
    {
        string strGameObject = "[GameObject]";
        if (_targetGameObject != null) {
            strGameObject = _targetGameObject.name;
        }

        return $"DISABLE {strGameObject}";
    }
}