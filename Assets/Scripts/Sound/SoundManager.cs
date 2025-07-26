using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance;

	public List<AudioEntry> sfxList;
	public List<AudioEntry> backgroundList;

	private AudioSource sfxSource;
	private AudioSource bgmSource;

	[SerializeField] private Slider sfxSlider;
	[SerializeField] private Slider bgmSlider;

	private int currentBGMIndex = 0;
	private Coroutine bgmLoopCoroutine;

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

		sfxSource = transform.Find("sfx").GetComponent<AudioSource>();
		bgmSource = transform.Find("background").GetComponent<AudioSource>();

		if (sfxSlider != null)
			sfxSlider.onValueChanged.AddListener(SetSFXVolume);

		if (bgmSlider != null)
			bgmSlider.onValueChanged.AddListener(SetBGMVolume);
	}

	private void Start()
	{
		float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 1f);
		float savedBGM = PlayerPrefs.GetFloat("BGMVolume", 1f);

		if (sfxSlider != null) sfxSlider.value = savedSFX;
		if (bgmSlider != null) bgmSlider.value = savedBGM;

		SetSFXVolume(savedSFX);
		SetBGMVolume(savedBGM);

		StartBGMSequence();
	}

	public void SetSFXVolume(float value)
	{
		sfxSource.volume = value;
		PlayerPrefs.SetFloat("SFXVolume", value);
		PlayerPrefs.Save();
	}

	public void SetBGMVolume(float value)
	{
		bgmSource.volume = value;
		PlayerPrefs.SetFloat("BGMVolume", value);
		PlayerPrefs.Save();
	}

	public float GetSFXVolume() => sfxSource.volume;

	public float GetBgmVolume() => bgmSource.volume;

	public void PlaySFX(string name)
	{
		AudioEntry entry = sfxList.Find(x => x.name == name);
		if (entry != null && entry.clip != null)
		{
			sfxSource.PlayOneShot(entry.clip);
		}
		else
		{
			Debug.LogWarning($"SFX '{name}' not found!");
		}
	}

	public void ChangeSoundBackground(int index)
	{
		if (index >= 0 && index < backgroundList.Count)
		{
			currentBGMIndex = index;
			StartBGMSequence();
		}
		else
		{
			Debug.LogWarning("Invalid background music index");
		}
	}

	public void StartBGMSequence()
	{
		if (bgmLoopCoroutine != null)
			StopCoroutine(bgmLoopCoroutine);

		bgmLoopCoroutine = StartCoroutine(PlayBackgroundMusicSequence());
	}

	private IEnumerator PlayBackgroundMusicSequence()
	{
		while (true)
		{
			if (backgroundList.Count == 0)
				yield break;

			AudioClip clip = backgroundList[currentBGMIndex].clip;

			if (clip != null)
			{
				bgmSource.clip = clip;
				bgmSource.Play();
				yield return new WaitForSeconds(clip.length);
			}
			else
			{
				Debug.LogWarning("Clip is null at index: " + currentBGMIndex);
				yield return new WaitForSeconds(1f);
			}

			currentBGMIndex = (currentBGMIndex + 1) % backgroundList.Count;
		}
	}

	// Shortcuts
	public void ClickSFX() => PlaySFX("click");

	public void PushSFX() => PlaySFX("push");

	public void PopSFX() => PlaySFX("pop");

	public void WinSFX() => PlaySFX("win");

	public void LoseSFX() => PlaySFX("lose");

	public void VictorySFX() => PlaySFX("victory");
}