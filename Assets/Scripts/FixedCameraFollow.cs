using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCameraFollow : MonoBehaviour
{

    [Header("Fixed Camera follow")]
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
 
    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset; 
    }
}
