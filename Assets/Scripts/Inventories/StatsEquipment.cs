using Game.Stats;
using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Inventories
{
    public class StatsEquipment : Equipment, IModifier
    {
        public IEnumerable<float> GetAdditiveModifiers(Stats.Stats stats)
        {
            foreach(var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifier;
                if (item == null) continue;
                
                foreach (float modifier in item.GetAdditiveModifiers(stats))
                {
                    yield return modifier;
                }
                
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stats.Stats stats)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifier;
                if (item == null) continue;
                
                foreach (float modifier in item.GetPercentageModifiers(stats))
                {
                    yield return modifier;
                }
                
            }
        }
    }
}