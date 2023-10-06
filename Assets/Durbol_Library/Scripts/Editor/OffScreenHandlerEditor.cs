#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Com.Durbol.Utility;

[CustomEditor(typeof(OffScreenHandler))]
public class OffScreenHandlerEditor : DurbolEditor
{
    private OffScreenHandler scriptReference;
    
    // All SerializedProperties
    #region ALL_PUBLIC_PROPERTIES
    private SerializedProperty camera;
    private SerializedProperty offset;
    private SerializedProperty iconOffset;
    private SerializedProperty rotateIcon;
    private SerializedProperty offScreenDisables;
	private SerializedProperty showOffScreenIndicator;
	private SerializedProperty offScreenUIIndicatorPrefab;
	private SerializedProperty canvasParent;
	private SerializedProperty gameManager;
	private SerializedProperty KolpoTools;
	private SerializedProperty gameplayData;
	private SerializedProperty gameState;
	#endregion ALL_PUBLIC_PROPERTIES

    bool drawProperties = true;
    public void OnEnable()
    {
        scriptReference = (OffScreenHandler)target;
        #region FINDER_ALL_PUBLIC_PROPERTIES_FINDER
        camera = serializedObject.FindProperty("camera");
        offset = serializedObject.FindProperty("offset");
        iconOffset = serializedObject.FindProperty("iconOffset");
        rotateIcon = serializedObject.FindProperty("rotateIcon");
        offScreenDisables = serializedObject.FindProperty("offScreenDisables");
		showOffScreenIndicator = serializedObject.FindProperty("showOffScreenIndicator");
        offScreenUIIndicatorPrefab = serializedObject.FindProperty("offScreenUIIndicatorPrefab");
		canvasParent = serializedObject.FindProperty("canvasParent");
		gameManager = serializedObject.FindProperty("gameManager");
		KolpoTools = serializedObject.FindProperty("KolpoTools");
		gameplayData = serializedObject.FindProperty("gameplayData");
		gameState = serializedObject.FindProperty("gameState");
		#endregion FINDER_ALL_PUBLIC_PROPERTIES
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawProperty(camera);
        DrawProperty(offset);
        DrawProperty(rotateIcon);
        DrawProperty(offScreenDisables);
        DrawProperty(showOffScreenIndicator);

        if (scriptReference.showOffScreenIndicator)
        {
            DrawProperty(offScreenUIIndicatorPrefab);
            DrawProperty(iconOffset);
            DrawProperty(canvasParent);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
