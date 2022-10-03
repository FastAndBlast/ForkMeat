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
    public int maxCapacityUpgrades = 2;

    [Header("Speed Upgrade")]
    public int speedCost = 3;
    public int speedCostIncrease = 3;
    public float speedUpgrade = 0.5f;
    public float turnUpgrade = 12.5f;
    public int maxSpeedUpgrades = -1;

    [Header("Boost Upgrade")]
    public int boostCost = 6;
    public int boostCostIncrease = 6;
    public float boostUpgrade = 1f;
    public int maxBoostUpgrades = 3;

    [Header("Map Upgrade")]
    public int mapCost = 10;
    public int mapCostIncrease = 10;
    public int maxMapUpgrades = 1;








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
            maxCapacityUpgrades -= 1;
            if (maxCapacityUpgrades == 0)
            {
                ShopUI.instance.RemoveShopOption("CarryCapacity");
            }
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
            maxSpeedUpgrades -= 1;
            if (maxSpeedUpgrades == 0)
            {
                ShopUI.instance.RemoveShopOption("Speed");
            }
            AudioManager.instance.Play("Kaching", 0.5f);
        }
    }

    public string UpgradeBoost()
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
            maxBoostUpgrades -= 1;
            if (maxBoostUpgrades == 0)
            {
                ShopUI.instance.RemoveShopOption("Boost");
            }
            AudioManager.instance.Play("Kaching", 0.5f);

            return "Reduce Boost cooldown by 1s";
        }
        return "";
    }

    public string UpgradeMap()
    {
        if (money >= mapCost)
        {
            money -= mapCost;
            mapCost += mapCostIncrease;
            maxMapUpgrades -= 1;
            if (maxMapUpgrades == 0)
            {
                BoxManager.instance.boxNum += 1;
                ShopUI.instance.RemoveShopOption("Map");
            }
            else
            {
                MapUpgradeManager.instance.Upgrade();
            }
			AudioManager.instance.Play("Kaching", 0.5f);

            return "Extra Box every 10s";
        }
        return "";
    }
}
