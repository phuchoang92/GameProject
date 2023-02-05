using UnityEngine;

namespace Game.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stat/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetStat(Stats stats,CharacterClass characterClass, int level)
        {
            foreach(ProgressionCharacterClass progressionClass in characterClasses)
            {
                if (progressionClass.characterClass != characterClass) continue;

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    if(progressionStat.stat !=stats) continue;

                    if(progressionStat.levels.Length < level) continue;
                    return progressionStat.levels[level - 1];
                }
            }
            return 0;
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