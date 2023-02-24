using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [SerializeField] float scatterDistance = 2;
        [SerializeField] InventoryItem[] dropLibrary;
        const int ATTEMPTS = 5;

        public void RandomDrop()
        {
            var item = dropLibrary[Random.Range(0, dropLibrary.Length)];
            DropItem(item);
        }
        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++) 
            {
                Vector3 randomPosition = transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPosition, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                } 
            }
            return transform.position;
        }
    }
}