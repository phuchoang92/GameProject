using Game.Core;
using Game.Attributes;
using RPG.Saving;
using System;
using UnityEngine;
using Game.Stats;
using System.Collections.Generic;

namespace Game.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifier
    {
        [SerializeField] float timeBetweenAtk = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        bool isAttacking = false;
        WeaponConfig currentWeapon= null;
        int numberOfHit = 0;
        private void Start()
        {
            if(currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
                currentWeapon = defaultWeapon;
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;

            if (target.isDead()) return;

            if (!isInRange())
            {
                if(isAttacking) 
                {
                    return; 
                }
                GetComponent<Movement.Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Movement.Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeapon = weapon;
            numberOfHit = 0;
            Animator animator = GetComponent<Animator>();
            currentWeapon.Spawn(rightHandTransform,leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAtk)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            SetAttacking(true);
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private void Hit()
        {
            if (target == null) return;

            float damage;
            if (GetComponent<BaseStats>().ProgressionCheck())
            {
                damage = GetComponent<BaseStats>().GetStat(Stats.Stats.Damage);
            }
            else
            {
                damage = currentWeapon.GetDamage()+10;
            }
           
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }

            if (currentWeapon.GetUsage() != -1)
            {
                numberOfHit = numberOfHit + 1;
                if (numberOfHit == currentWeapon.GetUsage())
                {
                    EquipWeapon(defaultWeapon);
                }
            }
        }

        private void Shoot()
        {
            Hit();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            Health target = combatTarget.GetComponent<Health>();
            return target != null && !target.isDead();
        }
        private bool isInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
            SetAttacking(false);
        }

        public void SetAttacking(bool isAttacking)
        {
            this.isAttacking = isAttacking;
        }

        public object CaptureState()
        {
            if (currentWeapon == null) return defaultWeapon.name;
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            String weaponName = (String)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stats.Stats stats)
        {
            if(stats == Stats.Stats.Damage)
            {
                yield return currentWeapon.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stats.Stats stats)
        {
            if (stats == Stats.Stats.Damage)
            {
                yield return currentWeapon.GetPercentageBuff();
            }
        }
    }
}

