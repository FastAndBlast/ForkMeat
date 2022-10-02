using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float timeScale = 1.0f;

    public static GameManager instance;
    
    private void Awake()
    {
        instance = this;
    }
}