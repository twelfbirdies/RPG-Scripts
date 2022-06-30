using RPG.Attributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat { 

public class Projectiles : MonoBehaviour
{
        [SerializeField] float projectileSpeed = 2f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject collisonEffect = null;
        [SerializeField] float maxLifetime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 0.2f;
        [SerializeField] UnityEvent onHit = null;

        GameObject instigator = null;
        Health target = null;
        float damage = 0f;

        private void Start()
        {
            transform.LookAt(GetAimPosition());
        }
        void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimPosition());
                transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
            }
        }
        public void SetTarget(Health target,float damage, GameObject instigator) 
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifetime);
        }

        private Vector3 GetAimPosition()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) { return target.transform.position;}
            else { return target.transform.position + (Vector3.up * targetCapsule.height / 2);}
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamage(instigator,damage);
            projectileSpeed = 0;
            onHit.Invoke();
            if (collisonEffect != null)
            {
                Instantiate(collisonEffect, GetAimPosition(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit) 
            { 
            Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }

    }
}