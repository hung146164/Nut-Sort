using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	private Screw currentScrew;
	private Nut currentNut;

	public bool canMove = true;

	[SerializeField] private GameUIManager gameUIManager;

	private int moveTimes;

	public int MoveTimes
	{
		get
		{
			return moveTimes;
		}
		set
		{
			moveTimes = value;
			OnMoveTimesChanged?.Invoke(value);
		}
	}

	public UnityEvent<int> OnMoveTimesChanged;
	public UnityEvent OnGameWin;
	public UnityEvent OnGameLose;

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
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && canMove)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100f))
			{
				if (hit.collider.gameObject.TryGetComponent<Screw>(out Screw screw))
				{
					//truong hop chua chon nut nao truoc do
					if (currentScrew == null)
					{
						FirstSelect(screw);
					}
					else
					{
						SecondSelect(screw);
					}
				}
			}
		}
	}

	private IEnumerator MoveAllNutToNewNut(Color moveColor, Screw screw)
	{
		MoveManager.Instance.UpdateUndoButtonState(false);
		canMove = false;
		int timesmove = 0;
		while (!screw.IsFull())
		{
			Nut topNut = currentScrew.GetTopNut();
			if (topNut == null || topNut.color != moveColor)
				break;

			currentScrew.DetachNut(topNut);

			yield return new WaitForSeconds(0.2f);
			topNut.transform.SetParent(screw.transform);
			currentScrew.RemoveTopNut();
			StartCoroutine(screw.MoveNutToThisScrew(topNut));

			yield return new WaitForSeconds(0.1f); // delay giữa các lần
			timesmove++;
		}

		yield return new WaitForSeconds(0.5f);
		//Add move to history de sau con undo

		MoveManager.Instance.AddMove(new Move(currentScrew, screw, timesmove));
		currentNut = null;
		currentScrew = null;
		canMove = true;
		MakeMove();
	}

	public void FirstSelect(Screw screw)
	{
		Nut top = screw.GetTopNut();
		if (top != null)
		{
			screw.DetachNut(top);
			currentNut = top;
			currentScrew = screw;
			SoundManager.Instance.PopSFX();
		}
	}

	public void SecondSelect(Screw screw)
	{
		if (screw == currentScrew)
		{
			currentScrew.InsertNut(currentNut);
			currentNut = null;
			currentScrew = null;
			return;
		}
		Nut top = screw.GetTopNut();
		if (top == null)
		{
			StartCoroutine(MoveAllNutToNewNut(currentNut.color, screw));
		}
		else
		{
			if (top.color != currentNut.color || screw.IsFull())
			{
				currentScrew.InsertNut(currentNut);
				currentNut = null;
				currentScrew = null;
			}
			else
			{
				StartCoroutine(MoveAllNutToNewNut(currentNut.color, screw));
			}
		}
	}

	public void MakeMove()
	{
		MoveTimes--;
		LevelManager.Instance.CheckGameState();
	}

	public void AddMove(int times)
	{
		MoveTimes += times;
	}

	public void NewGame()
	{
		gameUIManager.gameObject.SetActive(true);
		gameUIManager.CloseAll();
		MoveManager.Instance.ResetMoves();
		canMove = true;

		MoveTimes = LevelManager.Instance.currLevelObject.MoveTime;
	}

	public void Victory()
	{
		SoundManager.Instance.VictorySFX();
		gameUIManager.CloseAll();
		gameUIManager.Victory();
	}

	public void WinGame()
	{
		LevelManager.Instance.ChangeLevelPassed();
		SoundManager.Instance.WinSFX();
		MoneyManager.Instance.AddCoin(500);
		OnGameWin?.Invoke();
		canMove = false;
	}

	public void LoseGame()
	{
		SoundManager.Instance.LoseSFX();
		OnGameLose?.Invoke();
		canMove = false;
	}

	public void MainMenu()
	{
		LevelManager.Instance.CloseGame();
	}
}