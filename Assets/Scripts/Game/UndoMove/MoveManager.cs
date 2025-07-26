using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveManager : MonoBehaviour
{
	public static MoveManager Instance;

	private Stack<Move> moves;

	[SerializeField] private Button undoButton;

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

		moves = new Stack<Move>();
		undoButton.onClick.AddListener(UndoMove);
		UpdateUndoButtonState(false);
	}

	public void UndoMove()
	{
		if (MoneyManager.Instance.Coin < ServicePrice.UndoPrice)
		{
			MoneyManager.Instance.NotifyNotEnoughCoins();
			return;
		}
		if (moves.TryPop(out Move move))
		{
			MoneyManager.Instance.RemoveCoin(ServicePrice.UndoPrice);

			for (int i = 0; i < move.number; i++)
			{
				Nut topNut = move.toScrew.GetTopNut();
				if (topNut == null) break;
				topNut.transform.SetParent(move.fromScrew.transform);
				move.toScrew.RemoveTopNut();
				move.fromScrew.AddNutNoAnimation(topNut);
				GameManager.Instance.AddMove(1);
			}
		}
		UpdateUndoButtonState(moves.Count > 0);
	}

	public void AddMove(Move move)
	{
		moves.Push(move);
		UpdateUndoButtonState(true);
	}

	public void ResetMoves()
	{
		moves.Clear();
		UpdateUndoButtonState(false);
	}

	public void UpdateUndoButtonState(bool interactable)
	{
		undoButton.interactable = interactable;
	}
}