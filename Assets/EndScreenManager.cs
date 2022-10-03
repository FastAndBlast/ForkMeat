using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    public static EndScreenManager instance;

    public int mainMenuIndex;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void UpdateValues()
    {
        transform.Find("Score").GetComponent<TextMeshProUGUI>().text = "Score: " + GameManager.instance.score;
        transform.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "HighScore: " + GameManager.instance.highScore;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuIndex);
    }


}
