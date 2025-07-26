using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SimpleGridTool : EditorWindow
{
	private int rows = 5;
	private int columns = 5;
	private float cellSize = 60f;
	private float spacingX = 0.15f;
	private float spacingY = 0.1f;


	private Screw screwPrefab;
	private Nut nutPrefab;
	private Transform parent;

	// Bản đồ lưới chứa danh sách object ở mỗi ô
	private Dictionary<Vector2Int, List<GameObject>> gridObjects = new Dictionary<Vector2Int, List<GameObject>>();

	// Inspector hiển thị object đang chọn
	private GameObject selectedObject;
	private Editor selectedEditor;

	[MenuItem("Tools/Simple Grid Tool")]
	public static void ShowWindow()
	{
		GetWindow<SimpleGridTool>("Simple Grid Tool");
	}

	private void OnGUI()
	{
		GUILayout.Label("Settings", EditorStyles.boldLabel);

		screwPrefab = (Screw)EditorGUILayout.ObjectField("Screw Prefab", screwPrefab, typeof(Screw), false);
		nutPrefab = (Nut)EditorGUILayout.ObjectField("Nut Prefab", nutPrefab, typeof(Nut), false);
		parent = (Transform)EditorGUILayout.ObjectField("Parent Object", parent, typeof(Transform), true);

		rows = EditorGUILayout.IntSlider("Rows", rows, 1, 5);
		columns = EditorGUILayout.IntSlider("Columns", columns, 1, 5);
		cellSize = EditorGUILayout.FloatField("Cell Size (UI)", cellSize);
		spacingX = EditorGUILayout.FloatField("SpacingX (World)", spacingX);
		spacingY = EditorGUILayout.FloatField("spacingY (World)", spacingY);


		GUILayout.Space(10);
		DrawGrid();
		GUILayout.Space(20);
	}
	private void DrawGrid()
	{
		Event e = Event.current;

		for (int r = 0; r < rows; r++)
		{
			for (int c = 0; c < columns; c++)
			{
				Rect cell = new Rect(c * cellSize, r * cellSize + 180, cellSize - 2, cellSize - 2);
				GUI.Box(cell, "");

				Vector2Int gridPos = new Vector2Int(r, c);

				if (gridObjects.ContainsKey(gridPos))
				{
					foreach (var obj in gridObjects[gridPos])
					{
						if (obj != null && obj.TryGetComponent<Screw>(out var screw))
						{
							GUI.Label(new Rect(cell.x + 4, cell.y + 4, cell.width - 8, 20), $"Screw", EditorStyles.miniBoldLabel);
						}
					}
				}

				// Chuột phải → menu
				if (e.type == EventType.ContextClick && cell.Contains(e.mousePosition))
				{
					Vector3 worldPos = GridToWorldPosition(r, c);
					ShowContextMenu(worldPos, gridPos);
					e.Use();
				}

				// Chuột trái → click vào screw
				if (e.type == EventType.MouseDown && e.button == 0 && cell.Contains(e.mousePosition))
				{
					if (gridObjects.TryGetValue(gridPos, out var list))
					{
						foreach (var obj in list)
						{
							if (obj != null && obj.TryGetComponent<Screw>(out var screw))
							{
								ScrewEditorPopup.Open(screw, nutPrefab,parent);
								e.Use();
							}
						}
					}
				}
			}
		}

		Repaint();
	}
	private Vector3 GridToWorldPosition(int row, int col)
	{
		return new Vector3(row * spacingX, -col * spacingY, 0); 
	}
	private void ShowContextMenu(Vector3 worldPos, Vector2Int gridPos)
	{
		GenericMenu menu = new GenericMenu();

		menu.AddItem(new GUIContent("Add Screw"), false, () => AddScrewToGrid(gridPos, worldPos));

		menu.ShowAsContext();
	}

	private void AddScrewToGrid(Vector2Int gridPos, Vector3 worldPos)
	{
		if (screwPrefab == null || parent == null) return;

		GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(screwPrefab.gameObject, parent);
		go.transform.localPosition = worldPos;

		if (!gridObjects.ContainsKey(gridPos))
			gridObjects[gridPos] = new List<GameObject>();

		gridObjects[gridPos].Add(go);
	}

}
