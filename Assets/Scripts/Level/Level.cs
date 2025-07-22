using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public bool isActive = false;
    public void ResetLevel()
    {

    }

    public void Active()
    {
        isActive = true;
        gameObject.SetActive(true);
	}
    public void DeActive()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}
