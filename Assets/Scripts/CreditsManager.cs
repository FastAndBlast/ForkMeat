using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public float delay;
    public float scrollSpeed;

    public float scrollLockPoint = 1000;

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
        //print(transform.localPosition.y);
    }
}
