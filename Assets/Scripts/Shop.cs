using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject canvas;

    public float popUpDistance = 3f;

    public bool open;

    public static Shop instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Vector3.Distance(PlayerController.instance.transform.position, transform.position) > popUpDistance)
        {
            canvas.SetActive(false);
        }
        else
        {
            canvas.SetActive(true);
        }

        if (Input.GetButtonDown("OpenShop"))
        {
            OpenShop();
        }
    }

    public void OpenShop()
    {
        open = !open;
        if (open)
        {

        }
        else
        {

        }
    }
}
