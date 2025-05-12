using UnityEngine;

public class GameActionKaleidoscope : AGameAction
{
    public enum KaleidoscopeAction
    {
        Start = 0,
        Stop
    }

    [SerializeField] private KaleidoscopeAction _kaleidoscopeAction = KaleidoscopeAction.Start;

    protected override void ExecuteSpecific()
    {
        switch (_kaleidoscopeAction) {
            case KaleidoscopeAction.Start:
                PostProcessManager.Instance.SetEnabled(true);
                break;

            case KaleidoscopeAction.Stop:
                PostProcessManager.Instance.SetEnabled(false);
                break;
        }
    }

    public override string BuildGameObjectName()
    {
        return $"{_kaleidoscopeAction.ToString()} Kaleidoscope";
    }
}