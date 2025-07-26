using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundBinder : MonoBehaviour
{
	public Button[] buttons;

	private void Start()
	{
		foreach (Button btn in buttons)
		{
			btn.onClick.AddListener(() =>
			{
				if (SoundManager.Instance != null)
					SoundManager.Instance.ClickSFX();
			});
		}
	}
}