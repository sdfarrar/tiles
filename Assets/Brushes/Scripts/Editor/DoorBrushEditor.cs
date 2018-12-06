using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(DoorBrush))]
public class DoorBrushEditor : LayerObjectBrushEditor<Door> {

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

    protected override void DrawPrefabPreview(GridLayout gridLayout, GameObject brushTarget, BoundsInt bounds, GridBrushBase.Tool tool, bool executing, Vector3 prefabOffset) {
        if(!brush.activeObject){
            base.DrawPrefabPreview(gridLayout, brushTarget, bounds, tool, executing, prefabOffset);
        }else{
            DrawExitPreview(gridLayout, bounds, tool, prefabOffset);
        }
    }

    private void DrawExitPreview(GridLayout gridLayout, BoundsInt bounds, GridBrushBase.Tool tool, Vector3 prefabOffset) {
        if(!previewGO) { return; }
        if(tool==GridBrushBase.Tool.Paint){
            previewGO.SetActive(true);
            previewGO.transform.position = gridLayout.CellToLocalInterpolated(bounds.position + prefabOffset);
            previewGO.transform.rotation = brush.m_PrefabRotation * Quaternion.AngleAxis(180f, Vector3.forward);
        }else{
            previewGO.SetActive(false);
        }
    }


    public override void RegisterUndo(GameObject layer, GridBrushBase.Tool tool) {
        //base.RegisterUndo(layer, tool);
    }


}
