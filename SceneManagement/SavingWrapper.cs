using UnityEngine;
using RPG.Saving;
using System.Collections;
using System;

namespace RPG.SceneManagement { 

public class SavingWrapper : MonoBehaviour
{
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTimer = 0.5f;

        public void Awake()
        {
            StartCoroutine(LoadLastScene());
        } 
            
        IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile); 
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTimer);
        }
        void Update()
    {
            if (Input.GetKeyDown(KeyCode.L))
            {
            Load();
            }
            if (Input.GetKeyDown(KeyCode.S)) 
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }   

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
        private void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}