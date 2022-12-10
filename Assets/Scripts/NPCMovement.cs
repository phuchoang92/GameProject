using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCMovement : MonoBehaviour
{

    private NavMeshAgent agent;
    public Transform destinationObject;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(destinationObject.transform.position);
    }

}
