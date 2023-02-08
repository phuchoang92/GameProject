using UnityEngine;
using Game.Combat;
using Game.Movement;
using Game.Attributes;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System;

namespace Game.Control
{
    public class PlayerController : MonoBehaviour
    {

        // Cursor------------------------------------
        [System.Serializable]
        public struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float raycastRadius = 1f;

        // bool movementStarted = false;
        //NMD--------------------------------------

        Health health;
        // Start is called before the first frame update
        public void Start()
        {
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        public void Update()
        {   
            // if (Input.GetMouseButtonUp(0))
            // {
            //     movementStarted = false;
            // }
            if (health.isDead()) return;
            if (InteractWithUI()) return;
            if (InteractWithComponent()) return;
            if (InteractWithCombat()) return;
            //if (InteractWithPickups())
            //{
                //return;
            //}
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null)  
                {
                    continue;
                }
                if (!target.GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                SetCursor(CursorType.Combat);
                return true;
            }
            return false;
        }
        /*
        private bool InteractWithPickups()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                WeaponPickup target = hit.transform.GetComponent<WeaponPickup>();
                if (target == null)
                {
                    continue;
                }
                
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().EquipWeapon(target.GetWeapon());
                    Destroy(target.gameObject);
                }
                return true;
            }
            return false;
        }*/

        //*****************************
        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }
        //*******************************************
        //--------------------------------------------
        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        // private bool InteractWithMovement()
        // {
        //     Vector3 target;
        //     bool hasHit = RaycastNavMesh(out target);
        //     if (hasHit)
        //     {
        //         if (!GetComponent<Mover>().CanMoveTo(target)) return false;

        //         if (Input.GetMouseButtonDown(0))
        //         {
        //             movementStarted = true;
        //         }
        //         if (Input.GetMouseButton(0) && movementStarted)
        //         {
        //             GetComponent<Mover>().StartMoveAction(target, 1f);
        //         }
        //         SetCursor(CursorType.Movement);
        //         return true;
        //     }
        //     return false;
        // }

        // private bool RaycastNavMesh(out Vector3 target)
        // {
        //     target = new Vector3();

        //     RaycastHit hit;
        //     bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
        //     if (!hasHit) return false;

        //     NavMeshHit navMeshHit;
        //     bool hasCastToNavMesh = NavMesh.SamplePosition(
        //         hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
        //     if (!hasCastToNavMesh) return false;

        //     target = navMeshHit.position;

        //     return true;
        // }
        //--------------------------------------------

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        // cursor method-----------------------------------------
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }
    }
}


