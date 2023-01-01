using UnityEngine;

public class Fighter : MonoBehaviour, IAction
{
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float timeBetweenAtk = 1f;
    Transform target;
    float timeSinceLastAttack = 0;
    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (target == null) return;

        if(!isInRange())
        {
            GetComponent<Mover>().MoveTo(target.position);
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

    private bool isInRange()
    {
        return Vector3.Distance(transform.position, target.position) < weaponRange;
    }
    public void Attack(CombatTarget combatTarget)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        target = combatTarget.transform;
    }
    public void Cancel()
    {
        target = null;
    }
}
