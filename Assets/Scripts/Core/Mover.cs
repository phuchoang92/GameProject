using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Core;

namespace Game.Movement
{
    public class Mover : MonoBehaviour, IAction
    {

        [SerializeField] Transform target;
        NavMeshAgent navMeshAgent;
        //Health health;
        void Update()
        {
            //navMeshAgent.enabled = !health.isDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 des)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(des);
        }

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            //health = GetComponent<Health>();
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        public void MoveTo(Vector3 des)
        {
            GetComponent<NavMeshAgent>().destination = des;
            navMeshAgent.isStopped = false;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("speed", speed);
        }
    }
}


