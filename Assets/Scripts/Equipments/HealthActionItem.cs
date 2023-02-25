using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Attributes
{
    [CreateAssetMenu(menuName = ("Game/Inventory/Health Potion"), order =1)]
    public class HealthActionItem : ActionItem
    {
        [SerializeField] float healthToRestore;

        public override void Use(GameObject user)
        {
            user.GetComponent<Health>().Heal(healthToRestore);
        }
    }
}
