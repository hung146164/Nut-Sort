using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
	public float detachHeight = 0.02f;
	public float initialHeight = -0.08f;
	public float distanceNut = 0.015f;
	public int maxNuts = 6;
	public List<Nut> nutlist = new List<Nut>();

	public Stack<Nut> nuts = new Stack<Nut>();

	private void Awake()
	{
		foreach(Nut nut in nutlist)
		{
			nuts.Push(nut);
		}
	}
	public void AddNut(Nut nut)
	{
		InsertNut(nut);
		nutlist.Add(nut);
		nuts.Push(nut);
		
	}
	public void RemoveTopNut()
	{
		nuts.Pop();
		nutlist.RemoveAt(nutlist.Count - 1);
	}
	public bool IsFull()
	{
		return nutlist.Count == maxNuts;
	}
	public Nut GetTopNut()
	{
		if (nuts.TryPeek(out Nut nut))
			return nut;
		return null;
	}
	public void DetachNut(Nut nut)
	{
		Vector3 targetMove = new Vector3(0, 0, detachHeight);
		nut.GotoPosition(targetMove);
		
	}

	public void InsertNut(Nut nut)
	{
		Vector3 targetMove = new Vector3(0, 0, initialHeight + nuts.Count * distanceNut);
		Debug.Log(targetMove);
		nut.GotoPosition(targetMove);
	}
}