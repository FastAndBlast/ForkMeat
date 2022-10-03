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
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void UpdateValues()
    {
        transform.GetChild(0).Find("Score").GetComponent<TextMeshProUGUI>().text = "Score: " + GameManager.instance.score;
        transform.GetChild(0).Find("HighScore").GetComponent<TextMeshProUGUI>().text = "HighScore: " + GameManager.instance.highScore;
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
