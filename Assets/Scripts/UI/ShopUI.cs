using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;

    Transform carryCapacity;
    Transform speed;
    Transform boost;
    Transform map;

    public TextMeshProUGUI moneyText;

    public bool speedRemoved;
    public bool carryCapacityRemoved;
    public bool boostRemoved;
    public bool mapRemoved;

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
        RectTransform rect = GetComponent<RectTransform>();

        int ySize = 0;

        if (!carryCapacityRemoved)
        {
            carryCapacity.gameObject.SetActive(true);
            carryCapacity.Find("ProductCost").Find("CostText").GetComponent<TextMeshProUGUI>().text = "$" + Shop.instance.capacityCost.ToString();
            ySize++;
        }
        if (!speedRemoved)
        {
            speed.gameObject.SetActive(true);
            speed.Find("ProductCost").Find("CostText").GetComponent<TextMeshProUGUI>().text = "$" + Shop.instance.speedCost.ToString();
            ySize++;
        }
        if (!boostRemoved)
        {
            boost.gameObject.SetActive(true);
            boost.Find("ProductCost").Find("CostText").GetComponent<TextMeshProUGUI>().text = "$" + Shop.instance.boostCost.ToString();
            ySize++;
        }
        if (!mapRemoved)
        {
            map.gameObject.SetActive(true);
            map.Find("ProductCost").Find("CostText").GetComponent<TextMeshProUGUI>().text = "$" + Shop.instance.mapCost.ToString();
            ySize++;
        }

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 195 * ySize);
    }

    public void RemoveShopOption(string option)
    {
        if (option == "CarryCapacity")
        {
            carryCapacity.gameObject.SetActive(false);
            carryCapacityRemoved = true;
        }
        if (option == "Speed")
        {
            speed.gameObject.SetActive(false);
            speedRemoved = true;
        }
        if (option == "Boost")
        {
            boost.gameObject.SetActive(false);
            boostRemoved = true;
        }
        if (option == "Map")
        {
            map.gameObject.SetActive(false);
            mapRemoved = true;
        }
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
        string result = Shop.instance.UpgradeBoost();
        if (result != "")
        {
            boost.Find("Description").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = result;
        }
        UpdateValues();
    }

    public void UpgradeMap()
    {
        string result = Shop.instance.UpgradeMap();
        if (result != "")
        {
            map.Find("Description").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = result;
        }
        UpdateValues();
    }
}
