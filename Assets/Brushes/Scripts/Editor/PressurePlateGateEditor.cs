using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(PressurePlateGateBrush))]
public class PressurePlateGateBrushEditor : LayerObjectBrushEditor<PressurePlateGate> {
	public new PressurePlateGateBrush brush { get { return (target as PressurePlateGateBrush); } }

    private GameObject _currentPreview;
    private GameObject currentPreview {
        get { return _currentPreview; }
        set {
            _currentPreview = value;
            InstantiatePreviewGameObject(value);
        }
    }

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

    protected override void DrawPrefabPreview(GridLayout gridLayout, GameObject brushTarget, BoundsInt bounds, GridBrushBase.Tool tool, bool executing, Vector3 prefabOffset) {
        Vector3 offset = prefabOffset;
        if(!brush.activeObject) { // No gate yet
            if(currentPreview != brush.m_Prefab) { currentPreview = brush.m_Prefab; }
        }else if(brush.activeObject && !brush.activeObject.PressurePlate){ // Gate but no plate
            if(currentPreview != brush.PressurePlatePrefab) { currentPreview = brush.PressurePlatePrefab; }
            offset = brush.PressurePlatePrefabOffset;
        }else { // Gate and plate, reset
            if(currentPreview != brush.m_Prefab) { currentPreview = brush.m_Prefab; }
        }
        base.DrawPrefabPreview(gridLayout, brushTarget, bounds, tool, executing, offset);
    }

    private void SwapCurrentPreview(GameObject prefab) {
        InstantiatePreviewGameObject(brush.m_Prefab);
    }

    public override void RegisterUndo(GameObject layer, GridBrushBase.Tool tool) {
        //base.RegisterUndo(layer, tool);
    }


}
