using UnityEngine;

public class Fighter : MonoBehaviour, IAction
{
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float timeBetweenAtk = 1f;
    [SerializeField] float weaponDamage = 10f;
    Health target;
    float timeSinceLastAttack = 0;
    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (target == null) return;

        if (target.isDead()) return;

        if(!isInRange())
        {
            GetComponent<Mover>().MoveTo(target.transform.position);
        }
        else
        {
            GetComponent<Mover>().Cancel();
            AttackBehaviour();
        }
    }

    private void AttackBehaviour()
    {
        if(timeSinceLastAttack > timeBetweenAtk)
        {
            GetComponent<Animator>().SetTrigger("attack");
            timeSinceLastAttack = 0;
        }
    }

    private void Hit()
    {
        if (target != null)
        {
            target.TakeDamage(weaponDamage);
        }
    }

    private bool isInRange()
    {
        return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
    }
    public void Attack(CombatTarget combatTarget)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        target = combatTarget.GetComponent<Health>();
    }
    public void Cancel()
    {
        GetComponent<Animator>().SetTrigger("stopAttack");
        target = null;
    }
}
