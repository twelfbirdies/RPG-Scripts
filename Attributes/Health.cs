using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;
using UnityEngine.Events;
using System;

namespace RPG.Attributes
    {
    public class Health: MonoBehaviour, ISaveable
    {
        bool isDead = false;
        [SerializeField] float regenPercentage = 70f;
        [SerializeField] TakeDamageEvent takeDamage = null;
        [SerializeField] UnityEvent onDie = null;
        LazyValue<float> healthPoints;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> 
        { 
        }

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }
        private void Start()
        {
            healthPoints.ForceInit();
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Max(healthPoints.value + healthToRestore, GetMaxHealthPoints());
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenHealth;
        }
        public bool IsDead()
        {
            return isDead;
        }

        public float GetHealthPoints() 
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        public float GetFraction() 
        { 
            return (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            if (healthPoints.value == 0)
            {
                onDie.Invoke();
                AwardExperience(instigator);
                Death();
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperiencePoints(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public void Death()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();

        }

        private void RegenHealth() 
        {
           float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health)*(regenPercentage/100);
           healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value == 0)
            {
                Death();
            }
        }
    }
}
