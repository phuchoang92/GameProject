using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stat/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stats, float[]>> lookupTable = null;
        public float GetStat(Stats stats,CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels = lookupTable[characterClass][stats];

            if (levels.Length < level)
            {
                return lookupTable[characterClass][stats][levels.Length-1];
            }

            return lookupTable[characterClass][stats][level-1];
        }

        public int GetLevels(Stats stat, CharacterClass characterClass)
        {
            BuildLookup();

            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }
        private void BuildLookup()
        {
            if (lookupTable == null)
            {
                lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();

                foreach (ProgressionCharacterClass progressionClass in characterClasses)
                {
                    var statLookupTable = new Dictionary<Stats, float[]>();

                    foreach (ProgressionStat progressionStat in progressionClass.stats)
                    {
                        statLookupTable[progressionStat.stat] = progressionStat.levels;
                    }
                    lookupTable[progressionClass.characterClass] = statLookupTable;
                }
            }
        }

        
        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            //public float[] health;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stats stat;
            public float[] levels;
        }
    }
}