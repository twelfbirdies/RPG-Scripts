using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using System;
using UnityEngine;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float aggroTimeLapse = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;
        LazyValue<Vector3> guardPosition;
        float timeSinceAggrevated = Mathf.Infinity;
        [SerializeField] float shoutDistance = 5f;

        void Awake()
        {
            guardPosition = new LazyValue<Vector3>(GetInitialGuardPosition);
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }
        private Vector3 GetInitialGuardPosition() 
        {
            return transform.position;
        }

        void Update()
        {
            if (health.IsDead()) { fighter.Cancel(); return; }
            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < aggroTimeLapse)
            {
                SuspisionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimer();
        }

        public void AggrevateOnHit() 
        {
            timeSinceAggrevated = 0f;
        }

        private void UpdateTimer()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0f;
            fighter.Attack(player);
            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies() 
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits) 
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai != null) 
                {
                    ai.AggrevateOnHit();
                }
            }
        
        }


        void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint()) 
                {
                   timeSinceArrivedAtWaypoint = 0f;
                   CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > waypointDwellTime) 
            {
                mover.StartMoveTo(nextPosition,patrolSpeedFraction);
            }
            
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextWaypoint(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        void SuspisionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggroTimeLapse;
        }

        //called by unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}