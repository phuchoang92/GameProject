using Game.Attributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                GetComponent<TextMeshProUGUI>().text = "";
            }
            else
            {
                Health health = fighter.GetTarget();
                GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealhPoints());
            }
        }
    }
}
