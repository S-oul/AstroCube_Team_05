using UnityEngine;

public class GameActionEndGame : AGameAction
{
    protected override void ExecuteSpecific()
    {
        AchievementManager.Instance.EndGame();
    }

    public override string BuildGameObjectName()
    {
        return "ENDGAME TRIGGERED";
    }
}
