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

    public float orderLength = 30f;

    public Transform canvasTransform;

    List<string> existingOrders = new List<string>();

    [HideInInspector]
    public Queue<int> boxPrioQueue = new Queue<int>();

    // 2 orders -> -5s (25)
    // 2 orders -> -5s (20)
    // 2 orders -> -5s (15)

    public float orderCooldownReduction = 5f;

    public float minimumOrderCooldown = 15f;

    public int ordersBeforeCooldownReduction = 2;

    [HideInInspector]
    public int orderCount = 0;

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

            Color col = canvasTransform.GetChild(i).GetComponent<Image>().color;
            col.a = 100f/255f;
            canvasTransform.GetChild(i).GetComponent<Image>().color = col; //.gameObject.SetActive(true);
            canvasTransform.GetChild(i).Find("Panel").gameObject.SetActive(true);
            canvasTransform.GetChild(i).Find("Panel").GetComponent<Image>().fillAmount = orders[i].time / orders[i].timeMax;
            canvasTransform.GetChild(i).Find("Panel").Find("Image").GetComponent<Image>().sprite = CrateUI.dict[orders[i].boxType];
            canvasTransform.GetChild(i).Find("Panel").Find("OrderText").GetComponent<TextMeshProUGUI>().text = boxAmounts[orders[i].boxType].Count.ToString() + "/" + orders[i].amount.ToString();
        }
        for (; i < 3; i++)
        {
            Color col = canvasTransform.GetChild(i).GetComponent<Image>().color;
            col.a = 0f;
            canvasTransform.GetChild(i).GetComponent<Image>().color = col;
            canvasTransform.GetChild(i).Find("Panel").gameObject.SetActive(false);
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
        
        int boxIndex = 0;

        bool firstRun = true;

        while ((existingOrders.Contains(BoxManager.instance.boxNames[boxIndex]) && existingOrders.Count < BoxManager.instance.availableBoxTypes) || firstRun)
        {
            if (boxPrioQueue.Count > 0)
            {
                boxIndex = boxPrioQueue.Dequeue();
                break;
            }
            else if (cur >= firstFewOrders.Length)
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

        float len = orderLength;
        if (boxIndex == 1 || (boxIndex == 2))
        {
            len += 10;
        }
        if (boxIndex == 3 || (boxIndex == 4))
        {
            len += 30;
        }
        else if (boxIndex == 5)
        {
            len += 60;
        }

        Order newOrder = new Order(len);

        boxIndex = Mathf.Clamp(boxIndex, 0, BoxManager.instance.weights.Length - 1);

        newOrder.boxType = BoxManager.instance.boxNames[boxIndex];
        newOrder.amount = Random.Range(1, 4);
        

        if (boxAmounts[newOrder.boxType].Count >= newOrder.amount)
        {
            CompleteOrder(newOrder);
        }
        else
        {
            orderCount++;
            if (orderCount == ordersBeforeCooldownReduction)
            {
                orderCooldown = Mathf.Max(minimumOrderCooldown, orderCooldown - orderCooldownReduction);
                orderCount = 0;
            }

            orders.Add(newOrder);
            existingOrders.Add(newOrder.boxType);
            AudioManager.instance.Play("Whistle", 0.1f);
            for (int i = 0; i < newOrder.amount; i++)
            {
                BoxManager.instance.boxPrioQueue.Enqueue(boxIndex);
            }
        }
    }
}
