using UnityEngine;
using TMPro;
using System;
using RPG.Attributes;

namespace RPG.UI { 
public class HealthDisplay : MonoBehaviour
{
    Health health;
    private void Awake()
    {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
    }
    void Update()
    {
            GetComponent<TextMeshProUGUI>().text = String.Format("HP: {0:0}/{1:0}", health.GetHealthPoints(),health.GetMaxHealthPoints());
    }
} 
}
