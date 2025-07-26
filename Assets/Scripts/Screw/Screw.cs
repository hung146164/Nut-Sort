using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
	public float detachHeight = 0.02f;
	public float initialHeight = -0.095f;
	public float distanceNut = 0.015f;
	public int maxNuts = 6;
	public List<Nut> nutlist = new List<Nut>();

	public Stack<Nut> nuts = new Stack<Nut>();

	public float offsetTimeMove = 0.3f;

	public void CustomScrew()
	{
		float diff = maxNuts / 6f;
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, diff);
		float offset = -(0.1f - diff / 10f);
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, offset);
	}

	private void Start()
	{
		foreach (Nut nut in nutlist)
		{
			nuts.Push(nut);

			InsertNut(nut);
		}
	}

	public void RemoveTopNut()
	{
		if (nuts.Count == 0) return;
		nuts.Pop();
		nutlist.RemoveAt(nutlist.Count - 1);
		if (nuts.TryPeek(out Nut nut))
		{
			nut.isHidden = false;
			nut.UpdateState();
		}
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

	public IEnumerator MoveNutToThisScrew(Nut nut)
	{
		DetachNut(nut);
		Vector3 distance = nut.transform.position - transform.position;

		nutlist.Add(nut);

		yield return new WaitForSeconds(Mathf.Abs(distance.magnitude) - Mathf.Abs(offsetTimeMove * distance.magnitude));
		nuts.Push(nut);
		SoundManager.Instance.PushSFX();

		InsertNut(nut);
	}

	public void AddNut(Nut nut)
	{
		nutlist.Add(nut);
		nuts.Push(nut);
		InsertNut(nut);
	}

	public void DetachNut(Nut nut)
	{
		Vector3 targetMove = new Vector3(0, 0, detachHeight * 6 / maxNuts);
		nut.GotoPosition(targetMove, true);
	}

	public void AddNutNoAnimation(Nut nut)
	{
		nutlist.Add(nut);
		nuts.Push(nut);
		InsertNutNoAnimation(nut);
	}

	public void InsertNutNoAnimation(Nut nut)
	{
		Vector3 targetMove = new Vector3(0, 0, initialHeight + nuts.Count * distanceNut * 6 / maxNuts);
		nut.GotoPosition(targetMove, false);
		SoundManager.Instance.PushSFX();
	}

	public void InsertNut(Nut nut)
	{
		Vector3 targetMove = new Vector3(0, 0, initialHeight + nuts.Count * distanceNut * 6 / maxNuts);
		nut.GotoPosition(targetMove, true);
	}
}