using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
	[SerializeField] private int moveTime;

	private ScrewManager screwManager;

	[SerializeField] private int numberWinNut;

	public int MoveTime
	{
		get
		{
			return moveTime;
		}
		set
		{
			moveTime = value;
			OnMoveTimeChanged?.Invoke(value);
		}
	}

	public UnityEvent<int> OnMoveTimeChanged;

	private void Awake()
	{
		screwManager = transform.GetComponentInChildren<ScrewManager>();
	}

	public bool CheckWin()
	{
		List<Screw> nutList = screwManager.GetScrewLists();
		nutList.Sort((a, b) =>
		{
			int cmp = b.maxNuts.CompareTo(a.maxNuts); // So sánh maxNuts decrease
			if (cmp == 0)
			{
				// if they equal sort for nutlist
				return b.nutlist.Count.CompareTo(a.nutlist.Count);
			}
			return cmp;
		});

		for (int i = 0; i < numberWinNut; i++)
		{
			if (!IsValidWinScrew(nutList[i]))
			{
				return false;
			}
		}

		return true;
	}

	public bool IsValidWinScrew(Screw screw)
	{
		if (screw == null)
			return false;

		List<Nut> nuts = screw.nutlist;

		if (nuts.Count < screw.maxNuts)
			return false;

		Color color = nuts[0].color;

		foreach (var nut in nuts)
		{
			if (nut.color != color)
				return false;
		}

		return true;
	}

	public bool CheckLose()
	{
		if (GameManager.Instance.MoveTimes <= 0) return true;
		return false;
	}
}