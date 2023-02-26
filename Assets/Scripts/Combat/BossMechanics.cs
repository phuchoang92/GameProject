using Game.Control;
using Game.Movement;
using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    public class BossMechanics : MonoBehaviour
    {
        [SerializeField] WeaponConfig rangedWeapon;
        [SerializeField] WeaponConfig meleeWeapon;

        Fighter fighter;
        private void Awake()
        {
            fighter= GetComponent<Fighter>();
            fighter.EquipWeapon(rangedWeapon);
        }

        private void ChangeWeapon()
        {
            if (isInMeleeRange())
            {
                fighter.EquipWeapon(meleeWeapon);
            }
            else
            {
                fighter.EquipWeapon(rangedWeapon);
            }
        }
        private bool isInMeleeRange()
        {
            return Vector3.Distance(transform.position, fighter.GetTarget().transform.position) < meleeWeapon.GetRange();
        }
        private void Update()
        {
            if(fighter.GetTarget()!=null)
            {
                ChangeWeapon();
            }
        }

    }
}
