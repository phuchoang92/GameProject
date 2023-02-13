using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Stats
{
    public class BaseStats : MonoBehaviour, ISaveable
    {
        [Range(1,10)]
        [SerializeField] int currentLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;

        public event Action OnLevelUp;
        bool isRestored = false;
        Experience experience = null;
        private void Awake()
        {
            experience = GetComponent<Experience>();
        }
        private void Start()
        {
            if(!isRestored && GetComponent<Experience>()!= null)
            {
                currentLevel = CalculateLevel();
            }
        }
        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null) 
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
        }
        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stats stats)
        {
            return GetBaseStat(stats)*(1+GetPercentageModifier(stats)/100) + GetAdditiveModifier(stats);
        }

        public float GetPercentageModifier(Stats stats)
        {
            float total = 0;
            foreach (IModifier modifier in GetComponents<IModifier>())
            {
                foreach (float mod in modifier.GetPercentageModifiers(stats))
                {
                    total += mod;
                }
            }
            return total;
        }

        private float GetBaseStat(Stats stats)
        {
            return progression.GetStat(stats, characterClass, currentLevel);
        }

        public float GetAdditiveModifier(Stats stats)
        {
            float total = 0;
            foreach(IModifier modifier in GetComponents<IModifier>())
            {
                foreach( float mod in modifier.GetAdditiveModifiers(stats))
                {
                    total += mod;
                }
            }
            return total;
        }

        public int GetLevel()
        {
            return currentLevel;
        }
        public int CalculateLevel()
        {
            float currentEXP = GetComponent<Experience>().GetEXP();
            int penultimateLevel = progression.GetLevels(Stats.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float EXPToLevelUp = progression.GetStat(Stats.ExperienceToLevelUp, characterClass, level);
                if(EXPToLevelUp > currentEXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
        public bool ProgressionCheck()
        { 
            return progression != null; 
        }

        public object CaptureState()
        {
            return currentLevel;
        }

        public void RestoreState(object state)
        {
            currentLevel = (int)state;
            isRestored = true;
        }
    }
}
