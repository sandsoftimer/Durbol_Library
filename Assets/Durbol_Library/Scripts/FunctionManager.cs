/*
 * Developer Name: Md. Imran Hossain
 * E-mail: sandsoftimer@gmail.com
 * FB: https://www.facebook.com/md.imran.hossain.902
 * in: https://www.linkedin.com/in/md-imran-hossain-69768826/
 * 
 * This is a manager which will give common modular supports. 
 * like, 
 * Making a TextMeshPro(with text values) speach effect etc.
 *  
 */

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Com.Durbol.Utility
{
    public class FunctionManager : MonoBehaviour
    {
        Dictionary<int, IEnumerator> cacheEnumerator = new Dictionary<int, IEnumerator>();        

        public void StopCache(int id)
        {
            if (cacheEnumerator.ContainsKey(id))
            {
                IEnumerator enumerator = cacheEnumerator[id];
                StopCoroutine(enumerator);
                cacheEnumerator.Remove(id);
            }
        }

        public void ExecuteAfterSecond(float time, Action action = null)
        {
            StopCache(action.GetHashCode());

            IEnumerator enumerator = WaitForSecond(time, action);
            StartCoroutine(enumerator);
            cacheEnumerator.Add(action.GetHashCode(), enumerator);
        }

        IEnumerator WaitForSecond(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }

        public void SpeakThisTextMeshValue(TextMeshProUGUI textMeshProUGUI, float perWordDelay, AudioSource audioSource = null, Action action = null)
        {
            StopCache(textMeshProUGUI.GetHashCode());

            IEnumerator coroutine = SpeakThisText(textMeshProUGUI, perWordDelay, audioSource, action);
            StartCoroutine(coroutine);
            cacheEnumerator.Add(textMeshProUGUI.GetHashCode(), coroutine);
        }

        IEnumerator SpeakThisText(TextMeshProUGUI textMeshProUGUI, float perWordDelay, AudioSource audioSource, Action action = null)
        {
            string value = textMeshProUGUI.text;
            textMeshProUGUI.text = string.Empty;
            for (int i = 0; i < value.Length; i++)
            {
                textMeshProUGUI.text += value[i];

                if(audioSource != null)
                    audioSource.Play();

                if (!value[i].Equals(' '))
                    yield return new WaitForSeconds(perWordDelay);
                
            }
            action?.Invoke();
        }
    }
}
