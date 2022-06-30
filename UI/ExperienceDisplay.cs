using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using RPG.Stats;

namespace RPG.UI { 
public class ExperienceDisplay : MonoBehaviour
{
    Experience experience;
    private void Awake()
    {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
    }
    void Update()
    {
            GetComponent<TextMeshProUGUI>().text = String.Format("EXP: {0}", experience.GetExperiencePoints());
    }
}
}
