using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public Level[] levels;
	private int currentLevel;
    public int CurrentLevel { 
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
            OnLevelChanged?.Invoke(currentLevel);
        }
    }

    //Event
	public UnityEvent<int> OnLevelChanged;

	private void Awake()
	{
		levels=transform.GetComponentsInChildren<Level>();
	}
	public void ChangeLevel(int level)
    {
        if (level > levels.Length) return;

        levels[currentLevel].DeActive();
        levels[currentLevel].ResetLevel();

        CurrentLevel= level;

        levels[currentLevel].Active();
        
    }
}
