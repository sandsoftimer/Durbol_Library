/*
 * Developer Name: Md. Imran Hossain
 * E-mail: sandsoftimer@gmail.com
 * FB: https://www.facebook.com/md.imran.hossain.902
 * in: https://www.linkedin.com/in/md-imran-hossain-69768826/
 * 
 * Features: 
 * Saving gameplay data
 * Loading gameplay data  
 */

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Com.Durbol.Utility
{
    public class SavefileManager : MonoBehaviour
    {
        public void SaveGameData(GameplayData gameplayData)
        {
            ClassPropertyReader.SaveData(gameplayData);
        }

        public GameplayData LoadGameData()
        {
            Check_For_New_Update();

            GameplayData gameplayData = new GameplayData();
            ClassPropertyReader.LoadData(gameplayData);
            return gameplayData;
        }

        void Check_For_New_Update()
        {
            string result = "";
            bool flag = false;
#if UNITY_EDITOR
            if (!PlayerPrefs.GetString(ConstantManager.LAST_BUILD_NUMBER, "").Equals(PlayerSettings.bundleVersion))
            {
                flag = true;
                result = PlayerSettings.bundleVersion;
            }
#elif UNITY_ANDROID
                int buildNumber = 0;
                buildNumber = PlayerSettings.Android.bundleVersionCode;
                if(PlayerPrefs.GetInt(ConstantManager.LAST_BUILD_NUMBER, -1) != buildNumber)
                {
                    flag = true;
                    result = buildNumber.ToString();
                }
#elif UNITY_IOS
                int buildNumber = 0;
                buildNumber = int.Parse(PlayerSettings.iOS.buildNumber);
                if(PlayerPrefs.GetInt(ConstantManager.LAST_BUILD_NUMBER, -1) != buildNumber)
                {
                    flag = true;
                    result = buildNumber.ToString();
                }
#endif
            if (flag)
            {
                Debug.LogError("============= Install/Update is opening for first time =============");
                PlayerPrefs.SetString(ConstantManager.LAST_BUILD_NUMBER, result);
            }
        }
    }    
}