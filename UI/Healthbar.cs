using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Health health = null;
    [SerializeField] RectTransform foreGround = null;
    [SerializeField] Canvas rootCanvas = null;

    void Update()
    {
        if (Mathf.Approximately(health.GetFraction(),0)
        ||  Mathf.Approximately(health.GetFraction(),1))
        {
            rootCanvas.enabled = false;
            return;
        }
        rootCanvas.enabled = true;
        foreGround.localScale = new Vector3 (health.GetFraction(),1,1);
    }
}
