using UnityEngine;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;

	private MenuManager menuManager;

	private void Awake()
	{
		Instance = this;

		menuManager = GetComponent<MenuManager>();
	}

	public void OpenGameUI()
	{
		menuManager.OpenMenu("game");
	}
}