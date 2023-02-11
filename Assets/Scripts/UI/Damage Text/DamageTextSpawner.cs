using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Ui
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab;
        
        public void Spawn(float damageAmount)
        {
            DamageText instance = Instantiate<DamageText>(damageTextPrefab,transform);
            instance.SetValue(damageAmount);
        }

    }
}