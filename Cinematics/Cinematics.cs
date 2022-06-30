using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
namespace RPG.Cinematics { 

    public class Cinematics : MonoBehaviour, ISaveable
    {
        bool hasTriggered = false;

        public object CaptureState()
        {
            return hasTriggered;
        }

        public void RestoreState(object state)
        {
            hasTriggered = (bool)state;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.tag=="Player" && hasTriggered == false)
            { 
            GetComponent<PlayableDirector>().Play();
            hasTriggered = true;
            }
        }
    }
}
