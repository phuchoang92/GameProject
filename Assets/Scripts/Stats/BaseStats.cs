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

        public float GetHealth()
        {
            if (progression != null)
            {
                return progression.GetHealth(characterClass, startingLevel);
            }
            return -1;
        }
    }
}
