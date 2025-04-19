using UnityEngine;

public class GameActionFlash : AGameAction
{
    [SerializeField] private float _flashDuration = 1f;

    protected override void ExecuteSpecific()
    {
        FlashOverlay.Instance.Play(_flashDuration);
    }

    protected override bool IsFinishedSpecific()
    {
        return FlashOverlay.Instance.IsPlaying;
    }

    public override string BuildGameObjectName()
    {
        return $"FLASH {_flashDuration} s";
    }
}