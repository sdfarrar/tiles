using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Door Brush", menuName = "Brushes/Door Brush")]
[CustomGridBrush(false, true, false, "Door")]
public class DoorBrush : LayerObjectBrush<Door> {
    public List<string> ClearTileLayers = new List<string>();

	public override void Paint(GridLayout grid, GameObject layer, Vector3Int position) {
        Undo.SetCurrentGroupName("Door Paint");
        int group = Undo.GetCurrentGroup();

		if (activeObject != null) {
			if (activeObject.Exit == null) {
                Vector3 exitPosition = grid.LocalToWorld(grid.CellToLocalInterpolated(position + m_PrefabOffset));
				GameObject exitDoor = BrushUtility.Instantiate(m_Prefab, exitPosition, GetLayer());
                exitDoor.transform.rotation = Quaternion.Euler(0, 0, activeObject.transform.rotation.eulerAngles.z + 180f);
				exitDoor.GetComponent<Door>().Exit = activeObject;
				BrushUtility.SetDirty(exitDoor);

				activeObject.Exit = exitDoor.GetComponent<Door>();
				BrushUtility.SetDirty(activeObject);
			} else {
				BrushUtility.Select(BrushUtility.GetRootGrid(false).gameObject);
			}
		}
		base.Paint(grid, layer, position);
        ClearTiles(grid, activeObject);
        ClearTiles(grid, activeObject.Exit);

        Undo.CollapseUndoOperations(group);
	}

    private void ClearTiles(GridLayout grid, Door door) {
        if (door == null) { return; }

        SpriteRenderer sprite = door.transform.GetComponentInChildren<SpriteRenderer>();
        Vector3Int gateCellPos = grid.WorldToCell(door.transform.position);
        Vector3Int lowerLeft = grid.WorldToCell(door.transform.position - (Vector3)(Vector3.one * sprite.size / 2));
        Vector3Int upperRight = grid.WorldToCell(door.transform.position + (Vector3)(Vector3.one * sprite.size / 2));

        foreach(string layerName in ClearTileLayers) {
            Transform layer = grid.transform.Find(layerName);
            if (layer == null) { continue; }

            Tilemap tilemap = layer.GetComponent<Tilemap>();
            if (tilemap == null) { continue; }

            tilemap.BoxFill(lowerLeft, upperRight);
        }
    }

	public override void Erase(GridLayout grid, GameObject layer, Vector3Int position) {
		foreach (var door in allObjects) {
			if (grid.WorldToCell(door.transform.position) == position) {
				DestroyDoor(door);
				BrushUtility.Select(BrushUtility.GetRootGrid(false).gameObject);
				return;
			}
			if (door.Exit != null && grid.WorldToCell(door.Exit.transform.position) == position) {
				DestroyImmediate(door.Exit.gameObject);
				door.Exit = null;
				BrushUtility.SetDirty(door);
			}
		}
	}

	private void DestroyDoor(Door door) {
		if (door.Exit != null){
			DestroyImmediate(door.Exit.gameObject);
		}
		DestroyImmediate(door.gameObject);
	}
}