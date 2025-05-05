using UnityEngine;

namespace Game.Scripts.GameActions.Actions
{
    public class GameActionPlayerMovement : AGameAction
    {
        public enum MovementAction
        {
            Enable,
            Disable,
        }

        [SerializeField] private MovementAction _movementAction;
        [SerializeField] private PlayerMovement _playerMovement;

        protected override void ExecuteSpecific()
        {
            switch (_movementAction) {
                case MovementAction.Enable:
                    _playerMovement.EnableMovement();
                    break;

                case MovementAction.Disable:
                    _playerMovement.DisableMovement();
                    break;
            }
        }

        public override string BuildGameObjectName()
        {
            string strAction = _movementAction.ToString();
            return $"{strAction} Player Movements";
        }
    }
}