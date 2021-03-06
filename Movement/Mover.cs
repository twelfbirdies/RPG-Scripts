using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement { 
public class Mover : MonoBehaviour, IAction, ISaveable
{
    NavMeshAgent navMeshAgent;
    Health health;
    float maxSpeed = 6f;
    [SerializeField] float maxNavMeshPath = 20f;

        void Awake()
    {   
        health = GetComponent<Health>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        navMeshAgent.enabled = !health.IsDead();
        UpdateAnimator();
    }

    public void Cancel() 
    {
        navMeshAgent.isStopped = true;
    }
    void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }

    public void MoveTo(Vector3 destination, float fractionSpeed )
    {
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(fractionSpeed);
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
    }

    public bool CanMoveTo(Vector3 destination) 
    {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) { return false; }
            if (path.status != NavMeshPathStatus.PathComplete) { return false; }
            if (GetPathLength(path) > maxNavMeshPath) { return false; }
            return true;
    }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return total;
        }

        public void StartMoveTo(Vector3 destination, float fractionSpeed) 
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, fractionSpeed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().Warp(position.ToVector());
        }
    }
}