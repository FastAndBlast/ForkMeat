using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject canvas;

    public float popUpDistance = 3f;

    public bool open = false;

    public static Shop instance;

    public static int money = 100;

    public int capacityCost = 7;
    public int speedCost = 3;
    public int boostCost = 6;
    public int mapCost = 10;

    public float speedUpgrade = 50000;
    public float boostUpgrade = 1f;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Vector3.Distance(PlayerController.instance.transform.position, transform.position) > popUpDistance)
        {
            canvas.SetActive(false);
            if (open)
            {
                OpenShop();
            }
        }
        else
        {
            if (Input.GetButtonDown("OpenShop"))
            {
                OpenShop();
            }
            canvas.SetActive(true);
        }

        
    }

    public void OpenShop()
    {
        open = !open;
        if (open)
        {
            ShopUI.instance.gameObject.SetActive(true);
            ShopUI.instance.UpdateValues();
        }
        else
        {
            ShopUI.instance.gameObject.SetActive(false);
            ShopUI.instance.CloseShop();
        }
    }

    public void UpgradeCapacity()
    {
        if (money >= capacityCost)
        {
            PlayerController.instance.UpgradeBoxMax();
            money -= capacityCost;
        }
    }

    public void UpgradeSpeed()
    {
        if (money >= speedCost)
        {
            PlayerController.instance.speed += speedUpgrade;
            money -= speedCost;
        }
    }

    public void UpgradeBoost()
    {
        if (money >= boostCost)
        {
            if (PlayerController.instance.boostEnabled)
            {
                PlayerController.instance.boostCooldown -= boostUpgrade;
            }
            else
            {
                PlayerController.instance.boostEnabled = true;
            }
            money -= boostCost;
        }
    }

    public void UpgradeMap()
    {
        if (money >= mapCost)
        {
            money -= mapCost;
        }
    }
}
