using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerSpeed : MonoBehaviour
{

    [SerializeField] float newSpeedMultiplyer = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        if (other.gameObject.tag != "Player") return;
        PlayerMovement playerMovement =  other.gameObject.GetComponent<PlayerMovement>();
        playerMovement.setSpeed(playerMovement.defaultSpeed *  newSpeedMultiplyer);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited");
        if (other.gameObject.tag != "Player") return;
        PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
        playerMovement.setSpeedToDefault();
    }
}
