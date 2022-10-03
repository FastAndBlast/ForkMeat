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

    public int money = 100;

    [Header("Capacity Upgrade")]
    public int capacityCost = 7;
    public int capacityCostIncrease = 7;

    [Header("Speed Upgrade")]
    public int speedCost = 3;
    public int speedCostIncrease = 3;
    public float speedUpgrade = 0.5f;
    public float turnUpgrade = 12.5f;

    [Header("Boost Upgrade")]
    public int boostCost = 6;
    public int boostCostIncrease = 6;
    public float boostUpgrade = 1f;

    [Header("Map Upgrade")]
    public int mapCost = 10;
    public int mapCostIncrease = 10;





    
    
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //money = 100;
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
				AudioManager.instance.Play("Rachet");
            }
            canvas.SetActive(true);
        }

        
    }

    public void OpenShop()
    {
        open = !open;
        if (open)
        {
            //ShopUI.instance.gameObject.SetActive(true);
            ShopUI.instance.UpdateValues();
            Time.timeScale = 0;
        }
        else
        {
            //ShopUI.instance.gameObject.SetActive(false);
            ShopUI.instance.CloseShop();
            Time.timeScale = 1;
        }
    }

    public void UpgradeCapacity()
    {
        if (money >= capacityCost)
        {
            PlayerController.instance.UpgradeBoxMax();
            money -= capacityCost;
            capacityCost += capacityCostIncrease;
			AudioManager.instance.Play("Kaching", 0.5f);
        }
    }

    public void UpgradeSpeed()
    {
        if (money >= speedCost)
        {
            PlayerController.instance.speed += speedUpgrade;
            PlayerController.instance.turnSpeed += turnUpgrade;
            money -= speedCost;
            speedCost += speedCostIncrease;
			AudioManager.instance.Play("Kaching", 0.5f);
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
            boostCost += boostCostIncrease;
			AudioManager.instance.Play("Kaching", 0.5f);
        }
    }

    public void UpgradeMap()
    {
        if (money >= mapCost)
        {
            money -= mapCost;
            mapCost += mapCostIncrease;
            MapUpgradeManager.instance.Upgrade();
			AudioManager.instance.Play("Kaching", 0.5f);
        }
    }
}
