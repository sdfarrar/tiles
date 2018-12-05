using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class GridExtensions {

	public static Vector3 WorldToCellCentered(this Grid grid, Vector3 worldPosition){
		return grid.CellToLocalInterpolated(grid.WorldToCell(worldPosition));
	}

}

public static class VectorExtensions {

    // Vector3 Extensions
    public static Vector3 With(this Vector3 vec, float? x=null, float? y=null, float? z=null) {
        return new Vector3(x ?? vec.x, y ?? vec.y, z ?? vec.z);        
    }

    public static Vector3Int With(this Vector3Int vec, int? x=null, int? y=null, int? z=null) {
        return new Vector3Int(x ?? vec.x, y ?? vec.y, z ?? vec.z);        
    }

    // Vector2 Extensions
    public static Vector3 With(this Vector2 vec, float? x=null, float? y=null, float? z=null) {
        return new Vector3(x ?? vec.x, y ?? vec.y, z ?? 0);        
    }

    public static Vector2 xy(this Vector3 vec) {
        return new Vector2(vec.x,vec.y);
    }
    
    public static Vector2 xz(this Vector3 vec) {
        return new Vector2(vec.x,vec.z);
    }
    
    public static Vector2 yz(this Vector3 vec) {
        return new Vector2(vec.y,vec.z);
    }
    
    public static Vector2 yx(this Vector3 vec) {
        return new Vector2(vec.y,vec.x);
    }
    
    public static Vector2 zx(this Vector3 vec) {
        return new Vector2(vec.z,vec.x);
    }

    public static Vector2 zy(this Vector3 vec) {
        return new Vector2(vec.z,vec.y);
    }

}

public static class TilemapExtensions {
    public static void BoxFill(this Tilemap tilemap, Vector3Int lowerLeftCell, Vector3Int upperRightCell) {
#if UNITY_EDITOR
        Undo.RecordObject(tilemap, "Boxfill tiles");
#endif
        for (int x = lowerLeftCell.x; x < upperRightCell.x; ++x) {
            for (int y = lowerLeftCell.y; y < upperRightCell.y; ++y) {
                tilemap.SetTile(new Vector3Int(x, y, lowerLeftCell.z), null);
            }
        }
    }
}