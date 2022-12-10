using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC1Des : MonoBehaviour
{
    public int pivotPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
        {
            if (pivotPoint == 4)
            {
                pivotPoint= 0;
            }
            if (pivotPoint == 3)
            {
                this.gameObject.transform.position = new Vector3(56,3,-86);
                pivotPoint = 4;
            }
            if (pivotPoint == 2)
            {
                this.gameObject.transform.position = new Vector3(56, 3, -55);
                pivotPoint = 3;
            }
            if (pivotPoint == 1)
            {
                this.gameObject.transform.position = new Vector3(63, 3, -55);
                pivotPoint = 2;
            }
            if (pivotPoint==0)
            {
                this.gameObject.transform.position = new Vector3(56, 3, -55);
                pivotPoint = 1;
            }
        }
    }
}
