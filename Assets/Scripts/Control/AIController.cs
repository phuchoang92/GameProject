using Game.Attributes;
using Game.Combat;
using Game.Core;
using Game.Movement;
using RPG.Saving;
using System;
using UnityEngine;

namespace Game.Control
{
    public class AIController : MonoBehaviour, ISaveable
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTIme = 2f;
        [SerializeField] float shoutDistance = 5f;
        [SerializeField] float agroCooldownTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 2f;
        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;
        Vector3 guardPosition;

        bool isAggrevated = false;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindGameObjectWithTag("Player");
            guardPosition = transform.position;
        }
        

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead()) return;
            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTIme)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimers();
        }
        public void Aggrevate()
        {
            isAggrevated = true;
            timeSinceAggrevated = 0;
        }
        public void AggrevateNearbyEnemies()
        {
            isAggrevated = true;
            timeSinceAggrevated = 0;
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController enemy = hit.collider.GetComponent<AIController>();
                if (enemy == null) continue;
                enemy.Aggrevate();
            }
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            if(!isAggrevated) AggrevateNearbyEnemies();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath!= null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWayPoint();
            }
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, 1f);
            }
            isAggrevated = false;
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

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelAction();
        }

        private bool IsAggrevated()
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (isAggrevated) 
            { 
                return timeSinceAggrevated < agroCooldownTime; 
            }
            else
            {
                return distance < chaseDistance;
            }
        }

        // override
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        public object CaptureState()
        {
            return new SerializableVector3(guardPosition);
        }

        public void RestoreState(object state)
        {
            guardPosition = ((SerializableVector3)state).ToVector();
            isAggrevated = false;
            timeSinceLastSawPlayer = Mathf.Infinity;
            timeSinceArrivedAtWaypoint = Mathf.Infinity;
            timeSinceAggrevated = Mathf.Infinity;
            currentWaypointIndex = 0;
        }
    }
}