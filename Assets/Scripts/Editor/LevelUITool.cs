using UnityEditor;
using UnityEngine;

public class LevelUITool : EditorWindow
{
	string baseName = "hung";
	int from = 1;
	int to = 5;
	Transform parentTransform;
	GameObject prefab;

	[MenuItem("Tools/Level UI Tool")]
	public static void ShowWindow()
	{
		GetWindow<LevelUITool>("Level UI Generator");
	}

	void OnGUI()
	{
		GUILayout.Label("Create GameObject with name + i", EditorStyles.boldLabel);

		baseName = EditorGUILayout.TextField("Tên gốc", baseName);
		from = EditorGUILayout.IntField("Từ số", from);
		to = EditorGUILayout.IntField("Đến số", to);

		parentTransform = (Transform)EditorGUILayout.ObjectField("Object cha", parentTransform, typeof(Transform), true);
		prefab = (GameObject)EditorGUILayout.ObjectField("Prefab gốc", prefab, typeof(GameObject), false);

		if (GUILayout.Button("Generate"))
		{
			GenerateObjects();
		}
	}

	void GenerateObjects()
	{

		Undo.RegisterCompleteObjectUndo(parentTransform ?? null, "Generate Objects");

		for (int i = from; i <= to; i++)
		{
			string objName = baseName + i;
			GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
			newObj.name = objName;

			if (parentTransform != null)
				newObj.transform.SetParent(parentTransform, false);

			Undo.RegisterCreatedObjectUndo(newObj, "Create " + objName);
		}

		
	}
}
