using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        public float GetStat(Stats stats)
        {
            return progression.GetStat(stats, characterClass, startingLevel); 
        }

        public int GetLevel()
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
