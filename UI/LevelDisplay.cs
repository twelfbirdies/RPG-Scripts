using UnityEngine;
using TMPro;
using System;
using RPG.Stats;

namespace RPG.UI { 
public class LevelDisplay : MonoBehaviour
{
    BaseStats basestat;
    private void Awake()
    {
            basestat = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
    }
    void Update()
    {
            GetComponent<TextMeshProUGUI>().text = String.Format("Level: {0}", basestat.GetLevel());
    }
}
}
