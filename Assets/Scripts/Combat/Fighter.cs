using Game.Core;
using System;
using UnityEngine;

namespace Game.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float timeBetweenAtk = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon weapon = null;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        bool isAttacking = false;
        Weapon currentWeapon= null;
        private void Start()
        {
            EquipWeapon(weapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;

            if (target.isDead()) return;

            if (!isInRange())
            {
                if(isAttacking) { return; }
                GetComponent<Movement.Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Movement.Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            currentWeapon.Spawn(rightHandTransform,leftHandTransform, animator);
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
            isAttacking = true;
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private void Hit()
        {
            if (target == null) return;

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            //if (isInRange())
            else
            {
                target.TakeDamage(currentWeapon.GetDamage());
            }
            //else
            //{
            //    Cancel();
            //}
            isAttacking= false;
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
            isAttacking = false;
        }
    }
}

