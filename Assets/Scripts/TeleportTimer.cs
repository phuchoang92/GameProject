using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportTimer : MonoBehaviour
{

    public float PortalTimer;
    public float PortalTotalTimer;
    public bool PortalIsActive = true;

    private void Start()
    {
        PortalTimer = PortalTotalTimer;
    }

    private void Update()
    {
        if(PortalIsActive == true) { PortalTimer -= Time.deltaTime; }  
        if(PortalTimer <= 0 ) { PortalTimer = PortalTotalTimer;PortalIsActive = false; }
    }

}
