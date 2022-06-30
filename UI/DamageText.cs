using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace RPG.UI { 
public class DamageText : MonoBehaviour
{
        [SerializeField] TextMeshProUGUI text = null;
        void DestroyText() 
        { 
        Destroy(gameObject);
        }

        public void SetText(float damage) 
        {
            text.text = String.Format("{0:0}",damage);
        }
}
}
