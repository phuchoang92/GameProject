using Game.Core;
using UnityEngine;

namespace Game.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAtk = 1f;
        [SerializeField] float weaponDamage = 10f;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        bool isAttacking = false;
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;

            if (target.isDead()) return;

            if (!isInRange())
            {
                if(isAttacking) { return; }
                GetComponent<Game.Movement.Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Game.Movement.Mover>().Cancel();
                AttackBehaviour();
            }
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
            if (isInRange())
            {
                target.TakeDamage(weaponDamage);
            }
            else
            {
                Cancel();
            }
            isAttacking= false;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            Health target = combatTarget.GetComponent<Health>();
            return target != null && !target.isDead();
        }
        private bool isInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
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

