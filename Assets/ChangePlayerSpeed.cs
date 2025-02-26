using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerSpeed : MonoBehaviour
{

    [SerializeField] float newSpeedMultiplyer = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        if (other.gameObject.tag != "Player") return;
        PlayerMovement playerMovement =  other.gameObject.GetComponent<PlayerMovement>();
        playerMovement.SetSpeed(playerMovement.defaultSpeed *  newSpeedMultiplyer);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited");
        if (other.gameObject.tag != "Player") return;
        PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
        playerMovement.SetSpeedToDefault();
    }
}
