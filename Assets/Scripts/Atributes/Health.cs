using Game.Stats;
using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;
        [SerializeField] float expReward = 10f;
        private bool isDying = false;
        private float maxHealth;
        private void Start()
        {
            if (GetComponent<BaseStats>().ProgressionCheck())
            {
                health = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            }
            maxHealth = health;
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
        }

        public float GetPercentage() { 
            return health/ maxHealth * 100; 
        }

        private void Die()
        {
            if (isDying) return;
            isDying = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelAction();
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
    }
}
