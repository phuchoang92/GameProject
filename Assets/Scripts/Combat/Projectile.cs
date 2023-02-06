using Game.Attributes;
using UnityEngine;

namespace Game.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 7;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;

        Health target = null;
        GameObject instigator = null;
        float damage = 0;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }
        // Update is called once per frame
        void Update()
        {
            if (target == null) return;

            if (isHoming && !target.isDead()) transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            Destroy(gameObject, maxLifeTime);
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height * 2 / 3;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.isDead()) return;
            target.TakeDamage(instigator, damage);
            speed = 0;
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            foreach (GameObject gameObjectOnHit in destroyOnHit)
            {
                Destroy(gameObjectOnHit);
            }
            Destroy(gameObject, lifeAfterImpact);

        }
    }
}