using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnBoxUI : MonoBehaviour
{
    
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = (Mathf.CeilToInt(BoxManager.instance.spawnTime)).ToString();
    }
}
