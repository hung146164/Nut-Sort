using UnityEngine;

public class Menu : MonoBehaviour
{
	public string menuName;

	public void Open()
	{
		gameObject.SetActive(true);
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}
}