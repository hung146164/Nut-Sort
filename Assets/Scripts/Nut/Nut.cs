using System.Collections;
using UnityEngine;

public class Nut : MonoBehaviour
{
	public PipeColor colorType;                 // Enum để chọn loại màu
	public PipeColorData colorData;             // ScriptableObject chứa ánh xạ enum -> Color
	public Color color;                         // Màu thực tế lấy từ colorData, dùng trong logic

	private Renderer render;
	private Animator animator;

	public bool isHidden;
	public GameObject hiddenObj;

	private void Awake()
	{
		render = GetComponent<Renderer>();
		animator = GetComponent<Animator>();

		// Gán color dựa trên enum & ScriptableObject
		color = colorData.GetColor(colorType);

		UpdateState();
	}

#if UNITY_EDITOR

	// Tự cập nhật màu mỗi khi chỉnh enum trong Editor
	private void OnValidate()
	{
		if (colorData != null)
		{
			color = colorData.GetColor(colorType);
		}
	}

#endif

	public void UpdateState()
	{
		if (isHidden)
		{
			render.material.color = Color.black;
			hiddenObj.SetActive(true);
		}
		else
		{
			hiddenObj.SetActive(false);
			render.material.color = color;
		}
	}

	public void ChangeColor(PipeColor newColorType)
	{
		colorType = newColorType;
		color = colorData.GetColor(colorType);
		render.material.color = color;
	}

	public void GotoPosition(Vector3 position, bool haveAnim)
	{
		if (haveAnim)
			StartCoroutine(MoveSmooth(position));
		else
			transform.localPosition = position;
	}

	public IEnumerator MoveSmooth(Vector3 targetPosition)
	{
		animator.SetBool("rotate", true);

		Vector3 start = transform.localPosition;
		Vector3 direction = targetPosition - start;
		float distance = direction.magnitude;

		float speed = 0.5f;
		float duration = distance / speed;

		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			transform.localPosition = Vector3.Lerp(start, targetPosition, elapsed / duration);
			yield return null;
		}

		transform.localPosition = targetPosition;
		transform.localRotation = Quaternion.identity;
		animator.SetBool("rotate", false);
	}
}