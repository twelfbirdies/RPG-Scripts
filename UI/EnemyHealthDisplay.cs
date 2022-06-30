using UnityEngine;
using TMPro;
using System;
using RPG.Attributes;
using RPG.Combat;

namespace RPG.UI { 
public class EnemyHealthDisplay : MonoBehaviour
{
        Fighter fighter;
    private void Awake()
    {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
    }
    void Update()
    {
            if (fighter.GetTarget() == null) 
            {
                GetComponent<TextMeshProUGUI>().text = String.Format("Enemy Health: N/A");
                return;
            }
            Health health = fighter.GetTarget();
            GetComponent<TextMeshProUGUI>().text = String.Format("HP: {0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
}
}
