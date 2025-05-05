using System.Linq;
using UnityEngine;

public class GameActionDebugLog : AGameAction {

    [Header("Message")]
    [TextArea]
    public string message = "";

    private void Awake() {
        waitTime = 0f;
    }

    public override string BuildGameObjectName() {
        string strMessage = "[MESSAGE]";
        if (!string.IsNullOrEmpty(message)) {
            strMessage = message.Split(new[] { '\r', '\n' }).FirstOrDefault();
        }
        return "DEBUG LOG " + strMessage;
    }

    protected override void ExecuteSpecific() {
        Debug.Log(message);
    }

}