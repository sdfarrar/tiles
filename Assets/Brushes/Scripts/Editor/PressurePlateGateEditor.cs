using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(PressurePlateGateBrush))]
public class PressurePlateGateBrushEditor : LayerObjectBrushEditor<PressurePlateGate> {
	public new PressurePlateGateBrush brush { get { return (target as PressurePlateGateBrush); } }

	public void OnSceneGUI() {
		Grid grid = BrushUtility.GetRootGrid(false);
		PressurePlateGate gate = brush.activeObject;
		if (grid == null || gate == null || gate.PressurePlate == null) { return; }

		PressurePlate plate = gate.PressurePlate;
		Vector3 pressurePlatePos = grid.WorldToCellCentered(plate.transform.position);
		Vector3 doorPos = grid.WorldToCellCentered(gate.transform.position);
		Color color = Color.red;
		BrushEditorUtility.DrawLine(grid, pressurePlatePos, doorPos, new Color(color.r, color.g, color.b, 0.5f));
	}

	public override void OnPaintInspectorGUI() {
		if (BrushEditorUtility.SceneIsPrepared()) {
			GUILayout.Label("Use this brush to place doors triggered by pressure plates");
			GUILayout.Label("First paint the door and then the corresponding pressure plate.");
		} else {
			BrushEditorUtility.UnpreparedSceneInspector();
		}
	}

    public override void RegisterUndo(GameObject layer, GridBrushBase.Tool tool) {
        //base.RegisterUndo(layer, tool);
    }


}
