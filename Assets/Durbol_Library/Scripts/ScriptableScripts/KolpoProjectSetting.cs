#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public enum KV_Platforms
{
    ANDROID,
    IOS
}

[CreateAssetMenu(fileName = "Project Setting", menuName = "KolpoTools/Project Setup/KolpoProjectSetting")]
public class KolpoProjectSetting : ScriptableObject
{
    public KolpoProjectDefaultSettings kolpoProjectDefaultSettings;
    public KolpoProjectSettings_Android kolpoProjectAndroidSettings;
    public KolpoProjectSettings_IOS kolpoProjectIOSSettings;

    public void OnEnable()
    {
        kolpoProjectDefaultSettings = new KolpoProjectDefaultSettings();
        kolpoProjectAndroidSettings = new KolpoProjectSettings_Android();
        kolpoProjectIOSSettings = new KolpoProjectSettings_IOS();

        kolpoProjectDefaultSettings.companyName = PlayerSettings.companyName;
        kolpoProjectDefaultSettings.productName = PlayerSettings.productName;
        kolpoProjectDefaultSettings.icon = AssetDatabase.LoadAssetAtPath("Assets/KolpoLibrary/Sprites/KolpoVerseIcon.png", typeof(Texture2D)) as Texture2D;
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] {kolpoProjectDefaultSettings.icon});

        kolpoProjectDefaultSettings.bundleIdentifier = "com.kolpoverse." + Application.productName.ToLower().Replace(" ", "");
        kolpoProjectAndroidSettings.bundleIdentifier = kolpoProjectDefaultSettings.bundleIdentifier;
        kolpoProjectIOSSettings.bundleIdentifier = kolpoProjectDefaultSettings.bundleIdentifier;

        kolpoProjectDefaultSettings.defaultInterfaceOrientation = UIOrientation.Portrait;

        kolpoProjectAndroidSettings.androidArchitecture = ScriptingImplementation.IL2CPP;
    }

    public void SetProjectSetting()
    {
        PlayerSettings.companyName = kolpoProjectDefaultSettings.companyName;
        PlayerSettings.productName = kolpoProjectDefaultSettings.productName;
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] { kolpoProjectDefaultSettings.icon });
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.kolpoverse.projectname");
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, kolpoProjectAndroidSettings.androidArchitecture);

        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

        AndroidArchitecture aac = AndroidArchitecture.None;
        if(kolpoProjectAndroidSettings.ARMv7)
            aac |= AndroidArchitecture.ARMv7;
        if (kolpoProjectAndroidSettings.ARM64)
            aac |= AndroidArchitecture.ARM64;
        PlayerSettings.Android.targetArchitectures = aac;
    }
}

[Serializable]
public class KolpoProjectDefaultSettings
{
    public string companyName;
    public string productName;
    public string bundleIdentifier;
    public Texture2D icon;
    public bool unitySplashScreen;
    public UIOrientation defaultInterfaceOrientation;
}

[Serializable]
public class KolpoProjectSettings_Android
{
    public bool overrideBundleIdentifier;
    public bool ARMv7;
    public bool ARM64;
    public string bundleIdentifier;
    public float versionNumber;
    public int debugBuildNumber, producitonBuildNumber;
    public AndroidSdkVersions minimumSDKVersion = AndroidSdkVersions.AndroidApiLevel22;
    public AndroidSdkVersions targetSDKVersion = AndroidSdkVersions.AndroidApiLevelAuto;
    public ScriptingImplementation androidArchitecture;
}

[Serializable]
public class KolpoProjectSettings_IOS
{
    public bool overrideBundleIdentifier;
    public string bundleIdentifier;
    public float versionNumber;
    public int debugBuildNumber, producitonBuildNumber;
}
#endif