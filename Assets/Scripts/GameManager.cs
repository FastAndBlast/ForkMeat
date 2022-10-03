using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float timeScale = 1.0f;

    public static GameManager instance;

    public float score = 0;
    
    public float highScore
    {
        get
        {
            return PlayerPrefs.GetFloat("Highscore");
        }
        set
        {
            PlayerPrefs.SetFloat("Highscore", value);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void EndGame()
    {
        highScore = Mathf.Max(highScore, score);
        EndScreenManager.instance.gameObject.SetActive(true);
        EndScreenManager.instance.UpdateValues();
    }
}