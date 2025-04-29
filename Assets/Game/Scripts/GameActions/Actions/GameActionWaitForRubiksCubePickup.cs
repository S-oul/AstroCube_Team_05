using UnityEngine;

public class GameActionWaitForRubiksCubePickup : AGameAction
{
    [SerializeField] private HoldableRubiksCube _rubiksCube;

    private bool _isPickedUp = false;

    protected override void ExecuteSpecific()
    {
        _isPickedUp = false;
        if (_rubiksCube == null) return;

        _rubiksCube.PickUpDelegate += OnPickUp;
    }

    private void OnPickUp()
    {
        _rubiksCube.PickUpDelegate -= OnPickUp;
        _isPickedUp = true;
    }

    protected override bool IsFinishedSpecific()
    {
        return _isPickedUp;
    }

    public override string BuildGameObjectName()
    {
        return "WAIT FOR RUBIKS CUBE PICKUP";
    }
}