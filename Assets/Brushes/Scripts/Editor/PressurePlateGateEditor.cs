using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PressurePlateGateBrush))]
public class PressurePlateGateBrushEditor : LayerObjectBrushEditor<PressurePlateGate> {
	public new PressurePlateGateBrush brush { get { return (target as PressurePlateGateBrush); } }

	public void OnSceneGUI() {
		Grid grid = BrushUtility.GetRootGrid(false);
		if (grid != null) {
			if (brush.activeObject != null && brush.activeObject.PressurePlate != null) {
				Vector3Int pressurePlatePos = grid.WorldToCell(brush.activeObject.PressurePlate.transform.position);
				Vector3Int doorPos = grid.WorldToCell(brush.activeObject.transform.position);
				Color color = Color.red;
				BrushEditorUtility.DrawLine(grid, pressurePlatePos, doorPos, new Color(color.r, color.g, color.b, 0.5f));
			}
		}
	}

	public override void OnPaintInspectorGUI() {
		if (BrushEditorUtility.SceneIsPrepared()) {
			//brush.m_KeyColor = EditorGUILayout.ColorField("Key Color", brush.m_KeyColor);
			GUILayout.Space(5f);
			GUILayout.Label("Use this brush to place doors triggered by pressure plates");
			GUILayout.Label("First paint the door and then the corresponding pressure plate.");
		} else {
			BrushEditorUtility.UnpreparedSceneInspector();
		}
	}
}
