using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
	[SerializeField] private GameObject winMenu;
	[SerializeField] private GameObject loseMenu;
	[SerializeField] private GameObject VictoryUI;

	[SerializeField] private TMP_Text moveTimesText;

	private void Awake()
	{
		GameManager.Instance.OnGameWin.AddListener(OpenWinMenu);
		GameManager.Instance.OnGameLose.AddListener(OpenLoseMenu);
		GameManager.Instance.OnMoveTimesChanged.AddListener(ChangeMoveTimes);
	}

	private void OpenWinMenu()
	{
		//len nhac

		winMenu.SetActive(true);
	}

	private void OpenLoseMenu()
	{
		//len nhac

		loseMenu.SetActive(true);
	}

	public void CloseAll()
	{
		winMenu.SetActive(false);
		loseMenu.SetActive(false);
		VictoryUI.SetActive(false);
	}

	public void Victory()
	{
		VictoryUI.SetActive(true);
	}

	private void ChangeMoveTimes(int times)
	{
		moveTimesText.text = $"YOUR MOVES: {times}";
	}
}