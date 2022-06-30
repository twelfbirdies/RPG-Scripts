using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Control 
{ 
public class PatrolPath : MonoBehaviour
{
        private void OnDrawGizmos()
        {
            const float wayPointGizmoRadius = 0.3f;
            Gizmos.color = Color.red;
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextWaypoint(i);
                Gizmos.DrawSphere(GetWaypoint(i), wayPointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }

        }

        public int GetNextWaypoint(int i)
        {
            if (i + 1 == transform.childCount) 
            {
            return 0;
            }
            return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}