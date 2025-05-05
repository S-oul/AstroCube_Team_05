public class GameActionWait : AGameAction {

    public override string BuildGameObjectName() {
        return "WAIT " + waitTime + "s";
    }

    protected override void ExecuteSpecific() {
        durationMode = DURATION_MODE.CUSTOM_TIME;
    }

}