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
                GameObject exitDoor = CreateExitDoor(grid, position);
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
		Door door = GetObject(grid, position);
		if (door.Exit && door.Exit.Exit==door) {
            door.Exit.Exit = null;
		}
        BrushUtility.Destroy(door.gameObject);
        BrushUtility.Select(GetLayer().gameObject);
	}

    private GameObject CreateExitDoor(GridLayout grid, Vector3Int position){
        GameObject door = BrushUtility.Instantiate(m_Prefab, grid.LocalToWorld(grid.CellToLocalInterpolated(position + offsetFromBottomLeft)), GetLayer());
        door.transform.rotation = m_PrefabRotation * Quaternion.AngleAxis(180f, Vector3.forward);
        return door;
    }
}