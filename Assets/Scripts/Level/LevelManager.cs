using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;

	public Button[] levelButtons;
	public Level[] levels;

	public int levelPassed = 1;
	public int currentLevelIndex = 1;
	public Level currLevelObject;

	public Button skipLevel;
	public Button[] restartLevel;
	public Button nextLevel;

	[SerializeField] private TMP_Text levelText;
	[SerializeField] private TMP_Text levelTextWin;
	[SerializeField] private TMP_Text levelTextLose;

	public int CurrentLevel
	{
		get
		{
			return currentLevelIndex;
		}
		set
		{
			currentLevelIndex = value;
			OnLevelChanged?.Invoke(currentLevelIndex);
		}
	}

	//Event
	public UnityEvent<int> OnLevelChanged;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);

		for (int i = 0; i < levelButtons.Length; i++)
		{
			int index = i;
			levelButtons[i].onClick.AddListener(() => ChangeLevel(index + 1));
			levelButtons[i].onClick.AddListener(() => UIManager.Instance.OpenGameUI());
		}
		skipLevel.onClick.AddListener(SkipLevel);
		nextLevel.onClick.AddListener(NextLevel);

		foreach (Button but in restartLevel)
		{
			but.onClick.AddListener(Restart);
		}
		levelPassed = PlayerPrefs.GetInt("levelpassed");
	}

	private void Start()
	{
		UnlockLevelUI(0, levelPassed);
	}

	private void OnDestroy()
	{
		foreach (var btn in levelButtons)
		{
			btn.onClick.RemoveAllListeners();
		}
	}

	public void NextLevel()
	{
		ChangeLevelPassed();
		ChangeLevel(currentLevelIndex + 1);
	}

	public void Restart()
	{
		ChangeLevel(currentLevelIndex);
	}

	public void ChangeLevelPassed()
	{
		if (levelPassed < currentLevelIndex + 1)
		{
			UnlockLevelUI(levelPassed, currentLevelIndex + 1);
		}
		levelPassed = currentLevelIndex + 1;
	}

	private void UnlockLevelUI(int start, int end)
	{
		for (int i = start; i < end; i++)
		{
			if(i<levelButtons.Length)
				levelButtons[i].interactable = true;
		}
	}

	public void ChangeLevel(int level)
	{
		CloseGame();
		if (level > levels.Length)
		{
			GameManager.Instance.Victory();
			return;
		}
		//Set up cai moi
		currLevelObject = Instantiate(levels[level - 1], transform);
		currLevelObject.gameObject.SetActive(true);
		CurrentLevel = level;
		levelText.text = "Level " + level.ToString();
		levelTextWin.text = "Level " + level.ToString();
		levelTextLose.text = "Level " + level.ToString();

		GameManager.Instance.NewGame();

		levelPassed = Mathf.Max(levelPassed, level);
		Savegame();
	}

	public void CloseGame()
	{
		if (currLevelObject != null)
		{
			Destroy(currLevelObject.gameObject);
		}
	}

	public void CheckGameState()
	{
		if (currLevelObject.CheckWin())
		{
			GameManager.Instance.WinGame();
			return;
		}
		if (currLevelObject.CheckLose())
		{
			GameManager.Instance.LoseGame();
			return;
		}
	}

	public void Savegame()
	{
		PlayerPrefs.SetInt("levelpassed", levelPassed);
		PlayerPrefs.Save();
	}

	public void SkipLevel()
	{
		if (MoneyManager.Instance.Coin < ServicePrice.SkipLevelPrice)
		{
			MoneyManager.Instance.NotifyNotEnoughCoins();
			return;
		}
		MoneyManager.Instance.Coin -= ServicePrice.SkipLevelPrice;
		NextLevel();
	}
}