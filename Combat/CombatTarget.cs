using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController CallingController)
        {
            if (!CallingController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false; 
            }
            if (Input.GetMouseButtonDown(0))
            {
                CallingController.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
    }
}
