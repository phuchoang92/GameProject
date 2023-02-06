using Game.Control;
using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1f;

    private NavMeshAgent agent;

    Vector3 guardPosition;
    int currentWaypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        guardPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        PatrolBehaviour();
        UpdateAnimator();
    }

    private void PatrolBehaviour()
    {
        Vector3 nextPosition = guardPosition;
        if (patrolPath != null)
        {
            if (AtWayPoint())
            {
                CycleWaypoint();
            }
            nextPosition = GetCurrentWayPoint();
        }
        agent.SetDestination(nextPosition);
    }

    private Vector3 GetCurrentWayPoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private bool AtWayPoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
        return distanceToWaypoint < waypointTolerance;
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("Running", speed);
    }
}
