using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour, IAction
{
    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;
    //Fighter fighter;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void StartMoveAcion(Vector3 pos)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        MoveTo(pos);
    }
    public void MoveTo(Vector3 pos)
    {
        navMeshAgent.destination = pos;
        navMeshAgent.isStopped = false;
    }
    // Update is called once per frame  
    void Update()
    {
        //navMeshAgent.isStopped = false;
        if (InteractWithCombat()) 
        {
            return;
        }
        if (InteractWithMovement()) 
        {
            return;
        }
        UpdateAnimator();
    }
    public void Cancel()
    {
        navMeshAgent.isStopped = true;
    }
    private bool InteractWithCombat()
    {
        RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
        foreach (RaycastHit hit in hits)
        {
            CombatTarget target = hit.transform.GetComponent<CombatTarget>();
            if (target == null)
            {
                continue;
            }
            if(Input.GetMouseButtonDown(0))
            {
                GetComponent<Fighter>().Attack(target);
            }
            UpdateAnimator();
            return true;
        }
        return false;
    }
    private bool InteractWithMovement()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
        if (hasHit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartMoveAcion(hit.point);
            }
            UpdateAnimator();
            return true;
        }
        return false;
    }

    private static Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("speed", speed);
    }
}
