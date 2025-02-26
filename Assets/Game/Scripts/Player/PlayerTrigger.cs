using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VictoryZone"))
        {
            EventManager.TriggerPlayerWin();
        }
        if (other.CompareTag("DeathZone"))
        {
            EventManager.TriggerPlayerLose();
        }
    }
}
