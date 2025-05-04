using UnityEngine;

public class GameActionSetPlayerSpeed : AGameAction
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private float _speedFactor = 1f;

    protected override void ExecuteSpecific()
    {
        _playerMovement.SetSpeedFactor(_speedFactor);
    }

    public override string BuildGameObjectName()
    {
        return "SET PLAYER SPEED";
    }
}