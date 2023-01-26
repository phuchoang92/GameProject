using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    TeleportTimer teleportTimer;
    [SerializeField] 
    private Transform player;
    [SerializeField]
    private Transform TeleportLocation;

    // Start is called before the first frame update
    void Start()
    {
        teleportTimer = FindObjectOfType<TeleportTimer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && teleportTimer.PortalTimer == teleportTimer.PortalTotalTimer && teleportTimer.PortalIsActive == false) 
        {
            player.transform.position = TeleportLocation.transform.position;
            teleportTimer.PortalIsActive = true;
        }
    }
}
