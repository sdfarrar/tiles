using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(DoorBrush))]
public class DoorBrushEditor : LayerObjectBrushEditor<DoorBrush> {
	public new DoorBrush brush { get { return (target as DoorBrush); } }

    private GameObject previewGO;

	public void OnSceneGUI() {
		Grid grid = BrushUtility.GetRootGrid(false);
		Door door = brush.activeObject;
		if (grid == null || door == null || door.Exit == null) { return; }

		Door exit = door.Exit;
		Vector3 exitPosition = grid.WorldToCellCentered(exit.transform.position);
		Vector3 doorPos = grid.WorldToCellCentered(door.transform.position);
		Color color = Color.red;
		BrushEditorUtility.DrawLine(grid, exitPosition, doorPos, new Color(color.r, color.g, color.b, 0.5f));
	}

    public override void OnPaintSceneGUI(GridLayout gridLayout, GameObject brushTarget, BoundsInt bounds, GridBrushBase.Tool tool, bool executing) {
        base.OnPaintSceneGUI(gridLayout, brushTarget, bounds, tool, executing);
        if(tool==GridBrushBase.Tool.Paint){
            previewGO.SetActive(true);
            previewGO.transform.position = gridLayout.CellToLocalInterpolated(bounds.position + brush.offsetFromBottomLeft);
        }else{
            previewGO.SetActive(false);
        }
    }

    private void OnEnable() {
        previewGO = PrefabUtility.InstantiatePrefab(brush.m_Prefab) as GameObject;
        previewGO.hideFlags = HideFlags.HideAndDontSave;
        previewGO.SetActive(false);
    }

    private void OnDisable() {
        if(previewGO){
            BrushUtility.Destroy(previewGO);
        }
    }

    public override void RegisterUndo(GameObject layer, GridBrushBase.Tool tool) {
        //base.RegisterUndo(layer, tool);
    }


}
