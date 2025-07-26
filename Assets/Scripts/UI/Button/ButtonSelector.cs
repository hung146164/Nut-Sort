using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
	public List<Button> buttons;
	private Button selectedButton;

	private void Start()
	{
		foreach (var btn in buttons)
		{
			btn.onClick.AddListener(() => OnButtonClicked(btn));
		}

		if (buttons.Count > 0)
			OnButtonClicked(buttons[0]);
	}

	private void OnDestroy()
	{
		foreach (var btn in buttons)
		{
			btn.onClick.RemoveAllListeners();
		}
	}

	private void OnButtonClicked(Button clickedButton)
	{
		selectedButton = clickedButton;

		foreach (var btn in buttons)
		{
			bool isSelected = btn == clickedButton;
			var signed = btn.transform.Find("check");
			if (isSelected)
			{
				signed.gameObject.SetActive(true);
			}
			else
			{
				signed.gameObject.SetActive(false);
			}
		}
	}
}