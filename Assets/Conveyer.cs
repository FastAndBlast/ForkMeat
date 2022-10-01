using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{

    public List<GameObject> boxes = new List<GameObject>();

    Vector3 dir;

    Transform start;

    Transform end;

    public static List<Conveyer> conveyers = new List<Conveyer>();

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

    public void AddBox(GameObject box, int position = -1)
    {
        if (position == -1)
        {
            boxes.Add(box);
        }
        else
        {
            boxes.Insert(position, box);
        }
        
        box.GetComponent<Rigidbody>().mass = 10000;
    }

    public static void RemoveBox(GameObject box)
    {
        foreach (Conveyer conveyer in conveyers)
        {
            if (conveyer.boxes.Contains(box))
            {
                conveyer.boxes.Remove(box);
                box.GetComponent<Rigidbody>().mass = 10;
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Box" && !boxes.Contains(other.gameObject))
        {
            bool added = false;

            float dist = Vector3.Distance(other.transform.position, end.position);
            for (int i = boxes.Count - 1; i >= 0; i--)
            {
                if (dist > Vector3.Distance(boxes[i].transform.position, end.position))
                {
                    AddBox(other.gameObject, i + 1);
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                AddBox(other.gameObject, 0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Box" && boxes.Contains(other.gameObject))
        {
            boxes.Remove(other.gameObject);
        }
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
