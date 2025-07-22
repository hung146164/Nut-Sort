using UnityEngine;

public class GameManager : MonoBehaviour
{
	private Screw currentScrew;
	private Nut currentNut;

	// Update is called once per frame
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100f))
			{
				Debug.Log("Hit object: " + hit.collider.name);
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

	public void FirstSelect(Screw screw)
	{
		Nut top = screw.GetTopNut();
		if (top != null)
		{
			screw.DetachNut(top);
			currentNut = top;
			currentScrew = screw;
		}
	}

	public void SecondSelect(Screw screw)
	{
		Nut top = screw.GetTopNut();
		if (top == null)
		{
			//go cai cu 
			currentScrew.RemoveTopNut();

			// cho vao cai moi
			currentNut.transform.SetParent(screw.transform);
			screw.AddNut(currentNut);
		}
		else
		{
			if (top.color != currentNut.color || screw.IsFull()) 
			{
				currentScrew.InsertNut(currentNut);
			}
			else
			{
				//go cai cu 
				currentScrew.RemoveTopNut();

				// cho vao cai moi
				currentNut.transform.SetParent(screw.transform);
				screw.AddNut(currentNut);
			}
		}
		currentNut = null;
		currentScrew = null;
	}
}