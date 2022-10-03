using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Order
{
    public string boxType;
    public int amount;
    public float time;
    public float timeMax;
    public bool audioDone = false;
    public Order(float time)
    {
        this.time = time;
        this.timeMax = time;
    }
}

public class OrderManager : MonoBehaviour
{
    List<Order> orders = new List<Order>();

    public string firstFewOrders;

    int cur;

    public Dictionary<string, Stack<DropZone>> boxAmounts = new Dictionary<string, Stack<DropZone>>();

    public static OrderManager instance;

    public float orderCooldown = 30f;
    float orderCooldownTime = 4;

    List<string> existingOrders = new List<string>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        boxAmounts.Add("ChickenBox", new Stack<DropZone>());
        boxAmounts.Add("LambBox", new Stack<DropZone>());
        boxAmounts.Add("RooBox", new Stack<DropZone>());
        boxAmounts.Add("BeefBox", new Stack<DropZone>());
        boxAmounts.Add("PorkBox", new Stack<DropZone>());
        boxAmounts.Add("SlothBox", new Stack<DropZone>());
    }
    void Update()
    {
        if (orderCooldownTime > 0)
        {
            if (orders.Count < 3)
            {
                orderCooldownTime -= Time.deltaTime;
            }
        }
        else
        {
            AddOrder();
            orderCooldownTime = orderCooldown;
        }

        UpdateOrders();
    }

    void UpdateOrders()
    {
        int i;
        for (i = 0; i < orders.Count; i++)
        {
            if (i == 3)
            {
                break;
            }

            orders[i].time -= Time.deltaTime;
            if (orders[i].time / orders[i].timeMax < 0.2f && !orders[i].audioDone)
            {
                AudioManager.instance.Play("HurryUp", 0.8f);
                orders[i].audioDone = true;
            }

            if (orders[i].time <= 0)
            {
                GameManager.instance.EndGame();
            }

            transform.GetChild(i).Find("Panel").gameObject.SetActive(true);
            transform.GetChild(i).Find("Panel").GetComponent<Image>().fillAmount = orders[i].time / orders[i].timeMax;
            transform.GetChild(i).Find("Panel").Find("Image").GetComponent<Image>().sprite = CrateUI.dict[orders[i].boxType];
            transform.GetChild(i).Find("Panel").Find("OrderText").GetComponent<TextMeshProUGUI>().text = boxAmounts[orders[i].boxType].Count.ToString() + "/" + orders[i].amount.ToString();
        }
        for (; i < 3; i++)
        {
            transform.GetChild(i).Find("Panel").gameObject.SetActive(false);
        }
    }

    public void AddBox(string boxType, DropZone dropZone)
    {
        boxAmounts[boxType].Push(dropZone);
        for (int i = 0; i < orders.Count; i++)
        {
            if (orders[i].boxType == boxType)
            {
                if (boxAmounts[boxType].Count >= orders[i].amount)
                {
                    CompleteOrder(orders[i]);
                    existingOrders.Remove(orders[i].boxType);
                    orders.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void CompleteOrder(Order order)
    {
        for (int i = 0; i < order.amount; i++)
        {
            boxAmounts[order.boxType].Pop().RemoveCorrectBox();
        }
        AudioManager.instance.Play("OrderUp");
    }

    public void RemoveBox(string boxType, DropZone dropZone)
    {
        if (boxAmounts[boxType].Contains(dropZone))
        {
            Stack<DropZone> temp = new Stack<DropZone>();
            while (boxAmounts[boxType].Peek() != dropZone)
            {
                temp.Push(boxAmounts[boxType].Pop());
            }
            boxAmounts[boxType].Pop();
            while (temp.Count > 0)
            {
                boxAmounts[boxType].Push(temp.Pop());
            }
        }
    }


    public void AddOrder()
    {
        foreach (Stack<DropZone> stack in boxAmounts.Values)
        {
            if (stack.Count > 0)
            {
                print(stack.Peek());
            }
        }

        if (orders.Count >= 3)
        {
            //GameManager.instance.EndGame();
            return;
        }

        Order newOrder = new Order(20);
        
        int boxIndex = 0;

        bool firstRun = true;

        while (existingOrders.Contains(BoxManager.instance.boxNames[boxIndex]) || firstRun)
        {
            if (cur >= firstFewOrders.Length)
            {
                int rng = Random.Range(0, BoxManager.instance.weightSum);
                for (boxIndex = 0; boxIndex < BoxManager.instance.weights.Length; boxIndex++)
                {
                    rng -= BoxManager.instance.weights[boxIndex];
                    if (rng <= 0)
                    {
                        break;
                    }
                }
            }
            else
            {
                boxIndex = (int)firstFewOrders[cur] - 48;
                cur++;
            }
            firstRun = false;
        }

        boxIndex = Mathf.Clamp(boxIndex, 0, BoxManager.instance.weights.Length - 1);

        newOrder.boxType = BoxManager.instance.boxNames[boxIndex];
        newOrder.amount = Random.Range(1, 4);
        

        if (boxAmounts[newOrder.boxType].Count >= newOrder.amount)
        {
            CompleteOrder(newOrder);
        }
        else
        {
            orders.Add(newOrder);
            existingOrders.Add(newOrder.boxType);
            AudioManager.instance.Play("Whistle", 0.1f);
        }
    }
}
