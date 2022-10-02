using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Conveyer : MonoBehaviour
{

    public List<GameObject> boxes = new List<GameObject>();

    Vector3 dir;

    Transform start;

    Transform end;

    public static List<Conveyer> conveyers = new List<Conveyer>();

    public List<GameObject> inside = new List<GameObject>();

    private void Awake()
    {
        conveyers.Add(this);
    }

    private void Start()
    {
        start = transform.parent.Find("Start");
        end = transform.parent.Find("End");
        dir = (end.transform.position - start.transform.position).normalized;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            Vector3 target = Vector3.MoveTowards(boxes[i].transform.position, end.transform.position - dir * i, Time.fixedDeltaTime);
            boxes[i].GetComponent<Rigidbody>().MovePosition(target);
        }
    }

    public float GetPositionOfBox(int i)
    {
        return Vector3.Distance(boxes[i].transform.position, start.position) / Vector3.Distance(end.transform.position, start.position);
    }

    public void AddBox(GameObject box)
    {
        bool added = false;

        float dist = Vector3.Distance(box.transform.position, end.position);
        for (int i = boxes.Count - 1; i >= 0; i--)
        {
            if (dist > Vector3.Distance(boxes[i].transform.position, end.position))
            {
                boxes.Insert(i + 1, box);
                added = true;
                break;
            }
        }

        if (!added)
        {
            boxes.Insert(0, box);
        }

        box.GetComponent<Rigidbody>().mass = 10000;
    }

    public static void RemoveBoxFromAllConveyers(GameObject box)
    {
        foreach (Conveyer conveyer in conveyers)
        {
            if (conveyer.boxes.Contains(box))
            {
                conveyer.boxes.Remove(box);
                conveyer.inside.Remove(box);
                box.GetComponent<Rigidbody>().mass = 10;
            }
        }
    }

    public static void CheckBoxAtAllConveyers(GameObject box)
    {
        foreach (Conveyer conveyer in conveyers)
        {
            if (conveyer.inside.Contains(box))
            {
                conveyer.AddBox(box);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("ENTER: " + other.name);
        if (other.tag == "Box" && !boxes.Contains(other.gameObject))
        {
            AddBox(other.gameObject);
        }

        if (!inside.Contains(other.gameObject))
        {
            inside.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //print("XITED: " + other.name);
        if (other.tag == "Box" && boxes.Contains(other.gameObject))
        {
            boxes.Remove(other.gameObject);
        }

        if (inside.Contains(other.gameObject))
        {
            inside.Remove(other.gameObject);
        }
        //print(other.gameObject.name);
    }

    /*
    private void FixedUpdate()
    {
        foreach (GameObject go in inside)
        {
            go.GetComponent<Rigidbody>().AddForce(dir * Time.fixedDeltaTime * GameManager.timeScale);
        }
    }
    */
}
