using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MyTile : Tile {

    public Sprite[] m_Sprites;
    public Sprite m_Preview;

    // This refreshes itself and other MyTiles that are orthogonally and diagonally adjacent
    public override void RefreshTile(Vector3Int location, ITilemap tilemap) {
        for(int yd = -1; yd <= 1; yd++) {
            for(int xd = -1; xd <= 1; xd++) {
                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if(HasMyTile(tilemap, position)){
                    tilemap.RefreshTile(position);
                }
            }
        }
    }

    // This determines which sprite is used based on the MyTiles that are adjacent to it and rotates it to fit the other tiles.
    // As the rotation is determined by the MyTile, the TileFlags.OverrideTransform is set for the tile.
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData) {
        int mask = HasMyTile(tilemap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;  // above
        mask += HasMyTile(tilemap, location + new Vector3Int(1, 0, 0)) ? 2 : 0;     // right
        mask += HasMyTile(tilemap, location + new Vector3Int(0, -1, 0)) ? 4 : 0;    // down
        mask += HasMyTile(tilemap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0;    // left

        int index = GetIndex((byte)mask);
        if(index >= 0 && index < m_Sprites.Length) {
            tileData.sprite = m_Sprites[index];
            tileData.color = Color.white;
            var m = tileData.transform;
            m.SetTRS(Vector3.zero, GetRotation((byte)mask), Vector3.one);
            tileData.transform = m;
            tileData.flags = TileFlags.LockTransform;
            tileData.colliderType = ColliderType.Grid;
        }

    }
	
    // This determines if the Tile at the position is the same MyTile.
    private bool HasMyTile(ITilemap tilemap, Vector3Int position) {
        return tilemap.GetTile(position) == this;
    }

    // The following determines which sprite to use based on the number of adjacent RoadTiles
    private int GetIndex(byte mask) {
        // Sprites[0] = Empty
        // Sprites[1] = Left Side
        // Sprites[2] = Upper Left Corner
        switch (mask) {
            case 1:
            case 2:
            case 3: // lower left corner
            case 4:
            case 6: // upper left corner
            case 8:
            case 9: // lower right corner
            case 12:// upper right corner
                return 2; // Corner Tile
            case 0: // no adjacent
            case 5: // vertical middle
            case 7: // left vertical middle
            case 10:// horizontal middle
            case 11:// bot horizontal middle
            case 13:// right vertical middle
            case 14:// top horizontal middle
                return 1; // Left Side
            case 15:// center
                return 0; // Empty
        }
        return -1;
    }

    // The following determines which rotation to use based on the positions of adjacent RoadTiles
    private Quaternion GetRotation(byte mask) {
        switch (mask) {
            case 8:
            case 10:// horizontal middle
            case 12:// upper right corner
            case 14:// top horizontal middle
                return Quaternion.Euler(0f, 0f, -90f);
            case 9: // lower right corner
            case 13:// right vertical middle
                return Quaternion.Euler(0f, 0f, -180f);
            case 1:
            case 3: // lower left corner
            case 11:// bot horizontal middle
                return Quaternion.Euler(0f, 0f, -270f);
        }
        return Quaternion.Euler(0f, 0f, 0f);
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/MyTile")]
    public static void CreateRoadTile() {
        string path = EditorUtility.SaveFilePanelInProject("Save My Tile", "New My Tile", "Asset", "Save My Tile", "Assets");
        if (path == ""){ return; }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<MyTile>(), path);
    }
#endif

}
