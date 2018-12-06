using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LayerObjectBrush<>))]
public class LayerObjectBrushEditor<T> : GridBrushEditorBase {

	protected LayerObjectBrush<T> brush { get { return (target as LayerObjectBrush<T>); }}
    protected GameObject previewGO;

    private void OnEnable() {
        InstantiatePreviewGameObject(brush.m_Prefab);
    }

    private void OnDisable() {
        if(previewGO){
            BrushUtility.Destroy(previewGO);
        }
    }

	public override void OnPaintInspectorGUI() {
		if (BrushEditorUtility.SceneIsPrepared()){
			base.OnPaintInspectorGUI();
		}else{
			BrushEditorUtility.UnpreparedSceneInspector();
		}
	}

    public override void OnPaintSceneGUI(GridLayout gridLayout, GameObject brushTarget, BoundsInt bounds, GridBrushBase.Tool tool, bool executing) {
        base.OnPaintSceneGUI(gridLayout, brushTarget, bounds, tool, executing);
        DrawPrefabPreview(gridLayout, brushTarget, bounds, tool, executing, brush.offsetFromBottomLeft);
    }

    protected virtual void DrawPrefabPreview(GridLayout gridLayout, GameObject brushTarget, BoundsInt bounds, GridBrushBase.Tool tool, bool executing, Vector3 prefabOffset) {
        if(!previewGO) { return; }
        if(tool==GridBrushBase.Tool.Paint){
            previewGO.SetActive(true);
            previewGO.transform.position = gridLayout.CellToLocalInterpolated(bounds.position + prefabOffset);
            previewGO.transform.rotation = brush.m_PrefabRotation;
        }else{
            previewGO.SetActive(false);
        }
    }

    protected void InstantiatePreviewGameObject(GameObject prefab) {
        if(!prefab) { return; }
        if(previewGO) { BrushUtility.Destroy(previewGO); }
        previewGO = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        previewGO.hideFlags = HideFlags.HideAndDontSave;
        previewGO.SetActive(false);
    }

	public override void RegisterUndo(GameObject layer, GridBrushBase.Tool tool) {
		Undo.RegisterFullObjectHierarchyUndo(brush.GetLayer().gameObject, tool.ToString());
	}

	public override GameObject[] validTargets {
		get {
			Grid grid = FindObjectOfType<Grid>();
			return grid != null ? new GameObject[] { grid.gameObject } : null;
		}
	}
}
