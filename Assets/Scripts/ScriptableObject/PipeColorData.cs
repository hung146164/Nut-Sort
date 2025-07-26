using UnityEngine;

[CreateAssetMenu(fileName = "PipeColorData", menuName = "Color Mapping/PipeColorData")]
public class PipeColorData : ScriptableObject
{
	[System.Serializable]
	public class PipeColorEntry
	{
		public PipeColor colorType;
		public Color colorValue;
	}

	public PipeColorEntry[] colorMappings;

	public Color GetColor(PipeColor pipeColor)
	{
		foreach (var entry in colorMappings)
		{
			if (entry.colorType == pipeColor)
				return entry.colorValue;
		}
		Debug.LogWarning("PipeColor not found! Using white.");
		return Color.white;
	}
}

public enum PipeColor
{
	Red,
	Blue,
	Green,
	Yellow,
	Purple,
	Orange,
	Gray,
	White,
	Diamond,
	Brown,
}