using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostUI : MonoBehaviour
{
    Image backgroundImage;
    Image image;

    private void Start()
    {
        backgroundImage = GetComponent<Image>();
        image = transform.GetChild(0).GetComponent<Image>();
    }
    void Update()
    {
        if (PlayerController.instance.boostEnabled)
        {
            backgroundImage.enabled = true;
            image.enabled = true;
            image.fillAmount = 1 - (PlayerController.instance.boostCooldownTime / PlayerController.instance.boostCooldown);
        }
        else
        {
            backgroundImage.enabled = false;
            image.enabled = false;
        }
    }
}
