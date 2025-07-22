using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ScrewEditorPopup : EditorWindow
{
	private Screw screw;
	private Nut nutPrefab;

	// Thứ tự nut hiện tại (dựa trên List<Nut>)
	private int selectedNutIndex = -1;

	private Editor screwInspector;
	private Editor nutInspector;

	private const int MaxNuts = 6;

	public static void Open(Screw screw, Nut nutPrefab)
	{
		var wnd = GetWindow<ScrewEditorPopup>("Screw Editor");
		wnd.minSize = new Vector2(400, 500);
		wnd.screw = screw;
		wnd.nutPrefab = nutPrefab;
		wnd.screwInspector = Editor.CreateEditor(screw);
		wnd.selectedNutIndex = -1;
	}

	private void OnGUI()
	{
		if (screw == null || nutPrefab == null)
		{
			EditorGUILayout.HelpBox("Missing Screw or Nut Prefab", MessageType.Error);
			return;
		}

		// ----- Nut Count Slider -----
		int currentCount = screw.nutlist.Count;
		EditorGUILayout.LabelField($"Number of Nuts: {currentCount}/{MaxNuts}", EditorStyles.boldLabel);
		int newCount = EditorGUILayout.IntSlider(currentCount, 0, MaxNuts);

		if (newCount != currentCount)
		{
			Undo.RecordObject(screw, "Change Nut Count");

			// THÊM NUT
			if (newCount > currentCount)
			{
				for (int i = currentCount; i < newCount; i++)
				{
					Nut newNut = (Nut)PrefabUtility.InstantiatePrefab(nutPrefab, screw.transform);
					newNut.transform.localPosition = Vector3.zero;
					screw.nutlist.Add(newNut);
					
				}
			}
			// BỚT NUT
			else
			{
				for (int i = currentCount - 1; i >= newCount; i--)
				{
					Nut toRemove = screw.nutlist[i];
					if (toRemove != null)
						DestroyImmediate(toRemove.gameObject);
					screw.nutlist.RemoveAt(i);
				}

				// Reset selection nếu slot đó đã bị xóa
				if (selectedNutIndex >= newCount)
				{
					selectedNutIndex = -1;
					nutInspector = null;
				}
			}

			EditorUtility.SetDirty(screw);
		}

		EditorGUILayout.Space(10);

		// ----- Screw Inspector -----
		EditorGUILayout.LabelField("Screw Inspector", EditorStyles.boldLabel);
		screwInspector?.OnInspectorGUI();

		EditorGUILayout.Space(10);

		// ----- Nut Selection -----
		EditorGUILayout.LabelField("Nut List", EditorStyles.boldLabel);
		for (int i = 0; i < screw.nutlist.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField($"Nut {i + 1}", GUILayout.Width(80));
			if (GUILayout.Button("Select", GUILayout.Width(70)))
			{
				selectedNutIndex = i;
				nutInspector = Editor.CreateEditor(screw.nutlist[i]);
			}
			EditorGUILayout.EndHorizontal();
		}

		// ----- Selected Nut Inspector -----
		if (selectedNutIndex >= 0 && selectedNutIndex < screw.nutlist.Count)
		{
			EditorGUILayout.Space(10);
			EditorGUILayout.LabelField($"Nut {selectedNutIndex + 1} Inspector", EditorStyles.boldLabel);
			nutInspector?.OnInspectorGUI();
		}
	}
}
