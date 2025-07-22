using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;

	public AudioEntry[] sfx;
	public AudioEntry[] background;

	private Dictionary<string, AudioClip> sfxDict;
	private Dictionary<string, AudioClip> backgroundDict;

	private AudioSource sfxSource;
	private AudioSource bgmSource;

	[SerializeField] private Slider sfxSlider;
	[SerializeField] private Slider bgmSlider;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		sfxSource = transform.Find("sfx").GetComponent<AudioSource>();
		bgmSource = transform.Find("background").GetComponent<AudioSource>();

		sfxDict = new Dictionary<string, AudioClip>();
		foreach (var entry in sfx)
		{
			sfxDict[entry.name] = entry.clip;
		}

		backgroundDict = new Dictionary<string, AudioClip>();
		foreach (var entry in background)
		{
			backgroundDict[entry.name] = entry.clip;
		}
		sfxSlider.value = sfxSource.volume;
		bgmSlider.value = bgmSource.volume;

		sfxSlider.onValueChanged.AddListener(SetSFXVolume);
		bgmSlider.onValueChanged.AddListener(SetBGMVolume);
	}
	private void SetSFXVolume(float value)
	{
		sfxSource.volume = value;
	}

	private void SetBGMVolume(float value)
	{
		bgmSource.volume = value;
	}
	public void PlaySFX(string name)
	{
		if (sfxDict.TryGetValue(name, out AudioClip clip))
		{
			sfxSource.PlayOneShot(clip);
		}
		else
		{
			Debug.LogWarning($"SFX clip '{name}' not found!");
		}
	}

	public void ChangeSoundBackground(int index)
	{
		if (index >= 0 && index < background.Length)
		{
			bgmSource.clip = background[index].clip;
			bgmSource.loop = true;
			bgmSource.loop = true;
			bgmSource.Play();
		}
	}

	// Shortcut functions
	public void PointSfx() => PlaySFX("point");
	public void HitSfx() => PlaySFX("hit");
	public void DeadSfx() => PlaySFX("die");
	public void Wing() => PlaySFX("wing");
}
