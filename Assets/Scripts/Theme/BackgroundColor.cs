using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundColor : MonoBehaviour
{
	public List<Button> buttonList;

	private Renderer render;

	private void Awake()
	{
		render = GetComponent<Renderer>();

		foreach (var button in buttonList)
		{
			Color color = button.GetComponent<Image>().color;
			button.onClick.AddListener(() => ChangeColor(color));
		}
	}

	private void OnDestroy()
	{
		foreach (var button in buttonList)
		{
			button.onClick.RemoveAllListeners();
		}
	}

	public void ChangeColor(Color color)
	{
		render.material.color = color;
	}
}