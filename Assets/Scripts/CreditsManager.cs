using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    public float delay;
    public float scrollSpeed;

    public float scrollLockPoint = 1000;

    bool changed;

    // Update is called once per frame
    void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else if (transform.localPosition.y < scrollLockPoint)
        {
            transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
        }
		else if (!changed)
		{
            StartCoroutine(ChangeScene());
            changed = true;
		}
        //print(transform.localPosition.y);
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        yield break;
    }
}
