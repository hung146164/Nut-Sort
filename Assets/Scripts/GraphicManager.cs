using TMPro;
using UnityEngine;

public class GraphicsSettingsUI : MonoBehaviour
{
	public TMP_Dropdown qualityDropdown;

	private void Awake()
	{
		SetQuality(0);
	}
	private void Start()
	{
		qualityDropdown.value = QualitySettings.GetQualityLevel();
		qualityDropdown.onValueChanged.AddListener(SetQuality);
	}

	private void SetQuality(int index)
	{
		QualitySettings.SetQualityLevel(index * 2, true);
	}
}