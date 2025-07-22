using System.Collections;
using UnityEngine;

public class Nut : MonoBehaviour
{
	public Color color;

	private Renderer render;
	private Animator animator;
	private void Awake()
	{
		render = GetComponent<Renderer>();
		animator = GetComponent<Animator>();
		ChangeColor(color);
		
	}
	public void ChangeColor(Color _color)
	{
		render.material.color = _color;
	}
	public void GotoPosition(Vector3 position)
	{
		StartCoroutine(MoveSmooth(position));
	}

	public IEnumerator MoveSmooth(Vector3 targetPosition)
	{
		animator.SetBool("rotate", true);

		Vector3 start = transform.localPosition;
		float elapsed = 0f;
		float duration = Vector3.Distance(start, targetPosition) / 0.1f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			transform.localPosition = Vector3.Lerp(start, targetPosition, elapsed / duration);
			yield return null;
		}

		transform.localPosition = targetPosition;
		animator.SetBool("rotate", false);
	}

}

