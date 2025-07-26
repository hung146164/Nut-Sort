using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour
{
	public static MoneyManager Instance;

	[SerializeField] private TMP_Text coinText;

	[SerializeField] private TMP_Text coinWarning;

	public UnityEvent<int> OnCoinChanged;

	private int coin = 1000;

	public int Coin
	{
		get
		{
			return coin;
		}
		set
		{
			coin = value;
			OnCoinChanged?.Invoke(value);
		}
	}

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

		OnCoinChanged.AddListener(ChangeCoinText);
		coin = PlayerPrefs.GetInt("coin");
	}

	private void Start()
	{
		ChangeCoinText(coin);
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.C))
		{
			AddCoin(100);
		}
	}
	public void SaveGame()
	{
		PlayerPrefs.SetInt("coin", coin);
		PlayerPrefs.Save();
	}

	public void AddCoin(int val)
	{
		Coin += val;
		SaveGame();
	}

	public void RemoveCoin(int val)
	{
		Coin -= val;
	}

	public void ChangeCoinText(int _coin)
	{
		coinText.text = _coin.ToString();

	}

	public void NotifyNotEnoughCoins()
	{
		coinWarning.gameObject.SetActive(true);
	}
}