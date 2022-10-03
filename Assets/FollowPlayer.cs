using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public void Update()
    {
        transform.position = PlayerController.instance.transform.position;
    }
}
