using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public Menu[] menus;

	public void OpenMenu(string menuName)
	{
		foreach (var menu in menus)
		{
			if (menu.menuName == menuName)
			{
				menu.Open();
				continue;
			}
			menu.Close();
		}
	}

	public void CloseAllMenu(string menuName)
	{
		foreach (var menu in menus)
		{
			menu.Close();
		}
	}
}