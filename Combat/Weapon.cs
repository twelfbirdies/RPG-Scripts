using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat {

    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent onHit = null;
        internal void OnHit()
        {
            onHit.Invoke();
        }
    }
}