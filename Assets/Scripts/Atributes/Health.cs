using Game.Stats;
using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Saving;
using UnityEngine.Events;

namespace Game.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] float expReward = 10f;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        private bool isDying = false;
        private bool isRestored = false;
        private float maxHealth;
        private void Start()
        {
            if (isDying) 
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
                return;
            }

            if (!isRestored)
            {
                if (GetComponent<BaseStats>().ProgressionCheck())
                {
                    health = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
                }
                maxHealth = health;
            }
            else
            {
                if (GetComponent<BaseStats>().ProgressionCheck())
                {
                    maxHealth = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
                }
            }

        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }
        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }
        public bool isDead()
        {
            return isDying;
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health == 0)
            {
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public float GetHealthPoints()
        {
            return health;
        }

        public float GetMaxHealhPoints()
        {
            return maxHealth;
        }

        public float GetPercentage() { 
            return health/ maxHealth; 
        }

        private void Die()
        {
            if (isDying) return;
            isDying = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelAction();
        }

        private void RegenerateHealth()
        {
            health = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            maxHealth = health;
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience != null)
            {
                if (GetComponent<BaseStats>().ProgressionCheck())
                {
                    expReward = GetComponent<BaseStats>().GetStat(Stats.Stats.ExperienceReward);
                }
                experience.GainExperience(expReward);
            }
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            isRestored = true;
            maxHealth = health;
            health = (float)state;

            if (health <= 0)
            {
                Die();
            }
        }
    }
}
