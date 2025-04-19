using UnityEngine;

public abstract class AGameAction : MonoBehaviour {

    public const float DEFAULT_SHORT_TIME = 0.2f;

    //Wait
    public enum DURATION_MODE {
        UNTIL_ACTION_END = 0,
        CUSTOM_TIME,
        BOTH,
        DEFAULT_SHORT_TIME,
    }

    [Header("Duration")]
    public DURATION_MODE durationMode = DURATION_MODE.UNTIL_ACTION_END;
    public float waitTime = 1f;
    private float _waitCountdown = -1f;

    #region Functions Execute

    public void Execute() {
        if (durationMode == DURATION_MODE.DEFAULT_SHORT_TIME) {
            durationMode = DURATION_MODE.CUSTOM_TIME;
            waitTime = DEFAULT_SHORT_TIME;
        }

        ExecuteSpecific();
        _waitCountdown = waitTime;
    }

    protected abstract void ExecuteSpecific();

    #region Functions End

    public void End() {
        EndSpecific();
    }

    protected virtual void EndSpecific() { }

    #endregion

    #endregion

    #region Functions Update

    public void ActionUpdate(float dt) {

        switch (durationMode) {
            case DURATION_MODE.CUSTOM_TIME:
                if (_waitCountdown > 0f) {
                    _waitCountdown -= dt;
                }
                break;

            case DURATION_MODE.BOTH:
                if (IsFinishedSpecific()) {
                    if (_waitCountdown > 0f) {
                        _waitCountdown -= dt;
                    }
                }
                break;
        }

        ActionUpdateSpecific(dt);
    }

    protected virtual void ActionUpdateSpecific(float dt) { }

    #endregion

    #region Functions Check Finish

    public bool IsFinished() {
        switch (durationMode) {
            case DURATION_MODE.UNTIL_ACTION_END: return IsFinishedSpecific();
            case DURATION_MODE.CUSTOM_TIME: return _waitCountdown <= 0f;
            case DURATION_MODE.BOTH: return IsFinishedSpecific() && _waitCountdown <= 0f;
        }

        return true;
    }

    protected virtual bool IsFinishedSpecific() {
        return true;
    }

    #endregion

    #region Functions Game Object Name

    public abstract string BuildGameObjectName();

#if UNITY_EDITOR
    private void OnValidate() {
        string newGameObjectName = BuildGameObjectName();
        if (gameObject.name != newGameObjectName) {
            gameObject.name = newGameObjectName;
        }
    }
#endif

    #endregion
}