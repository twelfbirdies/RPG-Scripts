using RPG.Saving;
using System;
using UnityEngine;
namespace RPG.Stats { 

public class Experience : MonoBehaviour, ISaveable
{
        [SerializeField] float experiencePoints = 0f;

        public event Action onExperienceGained;
 
        public void GainExperiencePoints(float experience) 
        {
            experiencePoints += experience;
            onExperienceGained();
        }

        public float GetExperiencePoints()
        {
            return experiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
