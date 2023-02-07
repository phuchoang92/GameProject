using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Core;
using RPG.Saving;

namespace Game.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
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
            navMeshAgent.destination = des;
            navMeshAgent.isStopped = false;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("speed", speed);
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}


