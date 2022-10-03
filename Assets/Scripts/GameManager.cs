using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float timeScale = 1.0f;

    public bool paused = false;

    public static GameManager instance;

    public float score = 0;

    public static bool gameOver = false;

    public GameObject driftCamera;

    private void Start()
    {
        gameOver = false;
    }

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
        if (!gameOver)
        {
            highScore = Mathf.Max(highScore, score);
            EndScreenManager.instance.transform.GetChild(0).gameObject.SetActive(true);
            EndScreenManager.instance.UpdateValues();
            Camera.main.gameObject.SetActive(false);
            driftCamera.SetActive(true);
            gameOver = true;
            AudioManager.instance.Play("EngineOff", 0.2f);
            AudioManager.instance.Stop("EngineIdle");
            AudioManager.instance.Stop("EngineRev");
            AudioManager.instance.Stop("EngineTransUp");
            AudioManager.instance.Stop("EngineTransDown");
            AudioManager.instance.Stop("Boost");
        }
    }
}