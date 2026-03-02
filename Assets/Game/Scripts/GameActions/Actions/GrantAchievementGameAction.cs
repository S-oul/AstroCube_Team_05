using UnityEngine;

public class GrantAchievementGameAction : AGameAction
{
    [SerializeField] private string _achievementKey;
    
    protected override void ExecuteSpecific()
    {
        AchievementManager.Instance.UnlockAchievement(_achievementKey);
    }

    public override string BuildGameObjectName()
    {
        return "GRANT ACHIEVEMENT: " + _achievementKey;
    }
}
