using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Pressure Plate Gate Brush", menuName = "Brushes/Pressure Plate Gate Brush")]
[CustomGridBrush(false, true, false, "Pressure Plate Door")]
public class PressurePlateGateBrush : LayerObjectBrush<PressurePlateGate> {
	public GameObject PressurePlatePrefab;
	public Vector3 PressurePlatePrefabOffset = Vector3.zero;
    public List<string> ClearTileLayers = new List<string>();

	public override void Paint(GridLayout grid, GameObject layer, Vector3Int position) {
        Undo.SetCurrentGroupName("PressurePlateGate Paint");
        int group = Undo.GetCurrentGroup();

		if (activeObject != null) {
			if (activeObject.PressurePlate == null) {
                Vector3 platePosition = grid.LocalToWorld(grid.CellToLocalInterpolated(position + PressurePlatePrefabOffset));
				GameObject newPlate = BrushUtility.Instantiate(PressurePlatePrefab, platePosition, GetLayer());
				newPlate.GetComponent<PressurePlate>().Gate = activeObject;
				BrushUtility.SetDirty(newPlate);

				activeObject.PressurePlate = newPlate.GetComponent<PressurePlate>();
				BrushUtility.SetDirty(activeObject);
			} else {
				BrushUtility.Select(BrushUtility.GetRootGrid(false).gameObject);
			}
		}
		base.Paint(grid, layer, position);
        ClearTiles(grid);

        Undo.CollapseUndoOperations(group);
	}

    private void ClearTiles(GridLayout grid) {
        if (activeObject == null) { return; }

        PressurePlateGate gate = activeObject;
        SpriteRenderer sprite = gate.transform.GetComponentInChildren<SpriteRenderer>();
        Vector3Int gateCellPos = grid.WorldToCell(gate.transform.position);
        Vector3Int lowerLeft = grid.WorldToCell(gate.transform.position - (Vector3)(Vector3.one * sprite.size / 2));
        Vector3Int upperRight = grid.WorldToCell(gate.transform.position + (Vector3)(Vector3.one * sprite.size / 2));

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
			if (door.PressurePlate != null && grid.WorldToCell(door.PressurePlate.transform.position) == position) {
				DestroyImmediate(door.PressurePlate.gameObject);
				door.PressurePlate = null;
				BrushUtility.SetDirty(door);
			}
		}
	}

	private void DestroyDoor(PressurePlateGate door) {
		if (door.PressurePlate != null){
			DestroyImmediate(door.PressurePlate.gameObject);
		}
		DestroyImmediate(door.gameObject);
	}
}
