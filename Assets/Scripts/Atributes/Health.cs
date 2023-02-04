using Game.Stats;
using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;
        private bool isDying = false;

        private void Start()
        {
            float phealth = GetComponent<BaseStats>().GetHealth();
            if(phealth != -1) health = phealth;
        }
        public bool isDead()
        {
            return isDying;
        }
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            print(health);
            if (health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDying) return;
            isDying = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelAction();
        }
    }
}
