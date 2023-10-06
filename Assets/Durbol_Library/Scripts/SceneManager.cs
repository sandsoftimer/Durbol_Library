/*
 * Developer Name: Md. Imran Hossain
 * E-mail: sandsoftimer@gmail.com
 * FB: https://www.facebook.com/md.imran.hossain.902
 * in: https://www.linkedin.com/in/md-imran-hossain-69768826/
 * 
 * Features:
 * Scene FadeIn-Out Transition
 * Loading Next level
 * Reloading Current Level
 * Get level Index  
 */


using UnityEngine.SceneManagement;
using UnityEngine;
using System;

namespace Com.Durbol.Utility
{
    [RequireComponent(typeof(Animator))]
    public class SceneManager : MonoBehaviour
    {
        public DL_Scene_Transition_Type kV_Scene_Transition_Type;

        [HideInInspector]
        public DurbolTools owner;
        bool isBusy;
        Animator sceneFadeanimator;
        Animator SceneFadeanimator
        {
            get
            {
                if (sceneFadeanimator == null)
                    sceneFadeanimator = GetComponent<Animator>();
                return sceneFadeanimator;
            }
        }
        string levelToLoadByName;
        int levelToLoadByIndex;

        float lastPauseValue = 1;
        const string FadeOut = "FadeOut";
        const string FadeIn = "FadeIn";
        const string SlideOut = "SlideOut";
        const string SlideIn = "SlideIn";

        LoadSceneType loadLevelType;

        public void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLoadCallback;
        }

        public void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnLoadCallback;
        }

        void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
        {
            //Debug.LogError("Fade calling");
            string trigger = GetTransitionType(false);

            SceneFadeanimator.SetTrigger(trigger);
        }

        private string GetTransitionType(bool transitionIn)
        {
            string transitionType = "";
            switch (kV_Scene_Transition_Type)
            {
                case DL_Scene_Transition_Type.FADE:
                    transitionType = transitionIn ? FadeIn : FadeOut;
                    break;
                case DL_Scene_Transition_Type.HORIZONTAL_SLIDE:
                    transitionType = transitionIn ? SlideIn : SlideOut;
                    break;
                case DL_Scene_Transition_Type.VERTICAL_SLIDE:
                    Debug.LogError("Vertical Scene Transition effect is not implemented.");
                    transitionType = transitionIn ? SlideIn : SlideOut;
                    break;
                default:
                    break;
            }
            return transitionType;
        }

        public void OnFadeOutComplete()
        {
            isBusy = false;
        }

        public void OnFadeInComplete()
        {
            switch (loadLevelType)
            {
                case LoadSceneType.LOAD_BY_NAME:

                    UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoadByName);
                    break;
                case LoadSceneType.LOAD_BY_INDEX:
                    //Debug.LogError("From Complete: " + levelToLoadByIndex);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoadByIndex);
                    break;
                default:
                    break;
            }
        }

        public void LoadLevel(string levelName)
        {
            //owner.poolManager.ResetPoolManager();
            isBusy = true;
            levelToLoadByName = levelName;
            loadLevelType = LoadSceneType.LOAD_BY_NAME;
            string trigger = GetTransitionType(true);
            SceneFadeanimator.SetTrigger(trigger);
        }

        public void LoadLevel(int levelIndex)
        {
            //owner.poolManager.ResetPoolManager();
            isBusy = true;
            levelToLoadByIndex = levelIndex;
            loadLevelType = LoadSceneType.LOAD_BY_INDEX;
            string trigger = GetTransitionType(true);
            SceneFadeanimator.SetTrigger(trigger);
        }

        // This will re-load current level;
        public void ReLoadLevel()
        {
            if (isBusy)
                return;

            LoadLevel(GetLevelIndex());
        }

        // This will load next index scene
        // If not exist the it will open auto First scene of BuildIndex.
        public void LoadNextLevel()
        {
            if (isBusy)
                return;

            int loadedIndex = GetLevelIndex() + 1;

            //Debug.LogError("Count: "+ UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings);
            //if (loadedIndex <= ConstantManager.TOTAL_GAME_LEVELS)
            if (loadedIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings
                && loadedIndex <= ConstantManager.TOTAL_GAME_LEVELS + 1)
                LoadLevel(loadedIndex);
            else
            {
                //LoadLevel(1); // We skipping Boot Scene (This is not a gameplay level)

                LoadLevel(ConstantManager.TOTAL_GAME_LEVELS + 1);
            }
        }

        public int GetLevelIndex()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }

        public string GetLevelName()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.R))
                ReLoadLevel();
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = lastPauseValue;
                }
                else
                {
                    lastPauseValue = Time.timeScale;
                    Time.timeScale = 0;
                }
            }
#endif
        }
    }
}
