using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrewManager : MonoBehaviour
{
	private List<Screw> screwList;

	private void Awake()
	{
		screwList = transform.GetComponentsInChildren<Screw>().ToList();
	}

	public List<Screw> GetScrewLists()
	{
		return screwList;
	}
}