using Game.Attributes;
using UnityEngine;

namespace Game.Combat
{
    [CreateAssetMenu(fileName = "Weapon",menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject weapon = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        [SerializeField] int numberOfUsage = -1;

        const string WeaponName = "Weapon";
        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            DestroyOldWeapon(rightHandTransform, leftHandTransform);

            if (weapon != null)
            {
                Transform handTransform = GetTransform(rightHandTransform, leftHandTransform);
                GameObject weaponn = Instantiate(weapon, handTransform);
                weaponn.name = WeaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if ( animatorOverride!= null )
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if(overrideController != null )
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }
        private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform oldWeapon = rightHandTransform.Find(WeaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHandTransform.Find(WeaponName);
            }
            if (oldWeapon == null) { return; }

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = rightHandTransform;
            }
            else
            {
                handTransform = leftHandTransform;
            }

            return handTransform;
        }

        public bool HasProjectile()
        {
            return (projectile!= null);
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage); 
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        public float GetRange()
        {
            return weaponRange;
        }

        public int GetUsage()
        {
            return numberOfUsage;
        }
    }
}
