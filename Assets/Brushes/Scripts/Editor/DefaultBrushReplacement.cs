using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DefaultBrushReplacement))]
public class DefaultBrushReplacementEditor : GridBrushEditor {
	public override void OnPaintInspectorGUI() {
		GUILayout.Space(5f);
		GUILayout.Label("This is the built-in default brush.");
		GUILayout.Label("It is generic brush for painting tiles and game objects.");
		if (!BrushEditorUtility.SceneIsPrepared()){
			BrushEditorUtility.UnpreparedSceneInspector();
		}
	}
}