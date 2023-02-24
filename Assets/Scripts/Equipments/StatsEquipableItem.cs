using Game.Stats;
using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Inventories
{
    [CreateAssetMenu(menuName = ("Game/Inventory/Equipable Item"))]
    public class StatsEquipableItem : EquipableItem, IModifier
    {
        [SerializeField]
        Modifier[] additiveModifiers;
        [SerializeField]
        Modifier[] percentageModifiers;

        [System.Serializable]
        struct Modifier
        {
            public Stats.Stats stats;
            public float value;
        }
        public IEnumerable<float> GetAdditiveModifiers(Stats.Stats stats)
        {
            foreach(var modifier in additiveModifiers)
            {
                if(modifier.stats == stats)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stats.Stats stats)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stats == stats)
                {
                    yield return modifier.value;
                }
            }
        }
    }
}