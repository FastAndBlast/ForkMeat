using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;

    Transform carryCapacity;
    Transform speed;
    Transform boost;
    Transform map;

    public TextMeshProUGUI moneyText;

    private void Awake()
    {
        instance = this;
        carryCapacity = transform.Find("CarryCapacityPanel");
        speed = transform.Find("SpeedPanel");
        boost = transform.Find("BoostPanel");
        map = transform.Find("MapPanel");
        //gameObject.SetActive(false);
    }

    private void Update()
    {
        moneyText.text = "$" + Shop.instance.money.ToString();
    }

    public void UpdateValues()
    {
        carryCapacity.gameObject.SetActive(true);
        speed.gameObject.SetActive(true);
        boost.gameObject.SetActive(true);
        map.gameObject.SetActive(true);
        carryCapacity.Find("ProductCost").Find("CostText").GetComponent<TextMeshProUGUI>().text = "$" + Shop.instance.capacityCost.ToString();
        speed.Find("ProductCost").Find("CostText").GetComponent<TextMeshProUGUI>().text = "$" + Shop.instance.speedCost.ToString();
        boost.Find("ProductCost").Find("CostText").GetComponent<TextMeshProUGUI>().text = "$" + Shop.instance.boostCost.ToString();
        map.Find("ProductCost").Find("CostText").GetComponent<TextMeshProUGUI>().text = "$" + Shop.instance.mapCost.ToString();
    }

    public void CloseShop()
    {
        carryCapacity.gameObject.SetActive(false);
        speed.gameObject.SetActive(false);
        boost.gameObject.SetActive(false);
        map.gameObject.SetActive(false);
    }

    public void UpgradeCapacity()
    { 
        Shop.instance.UpgradeCapacity();
        UpdateValues();
    }

    public void UpgradeSpeed()
    {
        Shop.instance.UpgradeSpeed();
        UpdateValues();
    }

    public void UpgradeBoost()
    {
        Shop.instance.UpgradeBoost();
        UpdateValues();
    }

    public void UpgradeMap()
    {
        Shop.instance.UpgradeMap();
        UpdateValues();
    }
}
