using UnityEngine;

[CreateAssetMenu(fileName = "New Pressure Plate Gate Brush", menuName = "Brushes/Pressure Plate Gate Brush")]
[CustomGridBrush(false, true, false, "Pressure Plate Door")]
public class PressurePlateGateBrush : LayerObjectBrush<PressurePlateGate> {
	public GameObject PressurePlatePrefab;
	public Vector3 PressurePlatePrefabOffset = Vector3.zero;
	//public Color m_KeyColor;

	public override void Paint(GridLayout grid, GameObject layer, Vector3Int position) {
		if (activeObject != null) {
			if (activeObject.PressurePlate == null) {
				GameObject newPlate = BrushUtility.Instantiate(PressurePlatePrefab, grid.LocalToWorld(grid.CellToLocalInterpolated(position + PressurePlatePrefabOffset)), GetLayer());
				newPlate.GetComponent<PressurePlate>().Gate = activeObject;
				BrushUtility.SetDirty(newPlate);

				activeObject.PressurePlate = newPlate.GetComponent<PressurePlate>();
				//newKey.GetComponent<PressurePlate>().SetColor(m_KeyColor);
				//activeObject.SetColor(m_KeyColor);
				BrushUtility.SetDirty(activeObject);
			} else {
				BrushUtility.Select(BrushUtility.GetRootGrid(false).gameObject);
			}
		}
		base.Paint(grid, layer, position);
		if (activeObject) {
			//activeObject.SetColor(m_KeyColor);
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
