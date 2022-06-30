using System;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Coroutine activeFade;
        void Awake()
    {
       canvasGroup = GetComponent<CanvasGroup>();
    }
        public void FadeOutImmediate() 
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }

        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }

        private Coroutine Fade(float target, float time)
        {
            if (activeFade != null) 
            { StopCoroutine(activeFade); }
            activeFade = StartCoroutine(FadeRoutine(target,time));
            return activeFade;            
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha,target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha,target, Time.deltaTime/time);
                yield return null;
            }
        }

    }
}