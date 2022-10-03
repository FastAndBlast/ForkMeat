using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void GameStart()
	{
		SceneManager.LoadScene("Main", LoadSceneMode.Single);
	}

	public void Credits()
	{
		SceneManager.LoadScene("Credits", LoadSceneMode.Single);
	}

	public void CloseGame()
	{
		Application.Quit();
	}
}
