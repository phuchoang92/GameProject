using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int currentLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;

        public event Action OnLevelUp;

        //int currentLevel = 1;

        private void Start()
        {
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                currentLevel = CalculateLevel();
                experience.onExperienceGained += UpdateLevel;
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
            return progression.GetStat(stats, characterClass, currentLevel); 
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
    }
}
