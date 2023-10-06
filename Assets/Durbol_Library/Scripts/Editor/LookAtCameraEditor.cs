#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Com.Durbol.Utility;

[CustomEditor(typeof(LookAtCamera))]
public class LookAtCameraEditor : DurbolEditor
{
    private LookAtCamera scriptReference;

    // All SerializedProperties
    #region ALL_PUBLIC_PROPERTIES
    private SerializedProperty useCustomCamera;
	private SerializedProperty camera;
	#endregion ALL_PUBLIC_PROPERTIES

    bool drawProperties = true;
    public void OnEnable()
    {
        scriptReference = (LookAtCamera)target;
        #region FINDER_ALL_PUBLIC_PROPERTIES
        useCustomCamera = serializedObject.FindProperty("useCustomCamera");
		camera = serializedObject.FindProperty("camera");
		#endregion FINDER_ALL_PUBLIC_PROPERTIES
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawProperty(useCustomCamera);
        if (scriptReference.useCustomCamera)
        {
            Space();
            DrawHorizontalLine();
            DrawProperty(camera);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
