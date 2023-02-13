using Game.Stats;
using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Saving;
using UnityEngine.Events;
using GameDevTV.Utils;
using Unity.VisualScripting;
using System.Diagnostics;
using GameDevTV.Inventories;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

namespace Game.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;
        [SerializeField] float healthRegenPercentage = 5f;
        [SerializeField] float selfRegenTime = 3f;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] float expReward = 10f;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        // LazyValue<float> healthPoints;

        private bool isDying = false;
        private bool isRestored = false;
        private float maxHealth;
        float timeSinceSelfRegen = 0;
        Equipment equipment;
        BaseStats baseStats;

        private void Awake()
        {
            equipment = GetComponent<Equipment>();
            baseStats = GetComponent<BaseStats>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateHealth;
            }
            if (!baseStats.ProgressionCheck())
            {
                maxHealth = health;
            }
            else
            {
                maxHealth = baseStats.GetStat(Stats.Stats.Health);
                health = maxHealth;
            }

        }

        private void UpdateHealth()
        {
            maxHealth = baseStats.GetStat(Stats.Stats.Health); 
            
            health = Mathf.Min(health, maxHealth);
        }

        private void Start()
        {
            if (health <= 0)
            {
                isDying = true;
                SetCharacterActive(!isDying);
                return;
            }
            if (baseStats.ProgressionCheck())
            {
                maxHealth = baseStats.GetStat(Stats.Stats.Health);
            }
            if (!isRestored)
            {
                health = maxHealth;
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

        private void SetCharacterActive(bool active)
        {
            foreach( Transform child in transform)
            {
                child.gameObject.SetActive(active);
            }
        }
        public bool IsDead()
        {
            return isDying;
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health == 0)
            {
                onDie.Invoke();
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
        public void SelfRegen()
        {
            if (health < maxHealth && timeSinceSelfRegen>selfRegenTime)
            {
                health = MathF.Min(health + maxHealth*healthRegenPercentage / 100, maxHealth);
                timeSinceSelfRegen = 0;
            }
            else
            {
                timeSinceSelfRegen += Time.deltaTime;
            }
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
            health = (float)state;

            if (health > 0) 
            {
                isDying = false;
                SetCharacterActive(!isDying);
                Animator anime = GetComponent<Animator>();
                anime.Rebind();
                anime.Update(0f);
            }
        }

        public void Heal(float healthToRestore)
        {
            health = Mathf.Min(health + healthToRestore, GetMaxHealhPoints());
        }
    }
}
