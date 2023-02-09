using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Stats
{
    public class Leveldisplay : MonoBehaviour
    {
        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}", baseStats.GetLevel());
        }
    }
}