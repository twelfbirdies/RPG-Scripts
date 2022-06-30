using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement { 

public class Portal : MonoBehaviour
{
        enum PortalIdentifier 
        { 
            A,B,C,D,E,F
        }


        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] PortalIdentifier destination;
        [SerializeField] float fadeOutTimer = 1f;
        [SerializeField] float fadeWaitTimer = 0.5f;
        [SerializeField] float fadeInTimer = 1f;
       
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") 
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() 
        {
            if (sceneToLoad < 0) 
            {
                Debug.LogError("Scene to load set to -1");
                yield break;
            }
            DontDestroyOnLoad(gameObject);

            //Disable player controller when using portal is triggered
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(fadeOutTimer);
            savingWrapper.Save();

            //Disable new player controller when loading new scene
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;
            savingWrapper.Load();
            Portal otherPortal = GetOtherPortal();
            MovePlayer(otherPortal);
            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTimer);
            fader.FadeIn(fadeInTimer);

            newPlayerController.enabled = true;
            Destroy(gameObject);
        }

        private void MovePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.transform.position);
            player.transform.rotation = otherPortal.spawnPoint.transform.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>()) 
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }
            return null;
        }
    }
}