using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DropZone : MonoBehaviour
{
    public Vector2 size;

    public string boxType;

    public List<GameObject> boxes = new List<GameObject>();

    public static List<DropZone> dropZones = new List<DropZone>();

    public float timeToRemove = 30f;
    float maxTimeToRemove;

    //static List<int> boxValues = new List<int>();

    //Dictionary<>

    private void Awake()
    {
        dropZones.Add(this);
    }

    private void Start()
    {
        maxTimeToRemove = timeToRemove;
    }

    public void AddBox(GameObject box)
    {
        
        Vector3 pos = box.transform.position;
        pos = new Vector3(Mathf.Clamp(pos.x, transform.position.x + 0.5f, transform.position.x + size.x - 0.5f),
                        pos.y,
                        Mathf.Clamp(pos.z, transform.position.z + 0.5f, transform.position.z + size.y - 0.5f));

        if (pos.x < 0)
        {
            pos.x -= 1;
        }

        pos.x = pos.x - pos.x % 1 + 0.5f;
        
        
        if (pos.z < 0)
        {
            pos.z -= 1;
        }

        pos.y = pos.y - pos.y % 1f + 0.5f;

        pos.z = pos.z - pos.z % 1 + 0.5f;

        Vector3 rot = box.transform.eulerAngles;
        rot.x = rot.x - rot.x % 90;
        rot.y = rot.y - rot.y % 90;
        rot.z = rot.z - rot.z % 90;

        box.transform.position = pos;
        box.transform.eulerAngles = rot;
        boxes.Add(box);
    }

    public void Update()
    {
        if (timeToRemove > 0)
        {
            timeToRemove -= Time.deltaTime;
        }
        else
        {
            //TODO: ADD REWARDS AND EFFECTS
            for (int i = 0; i < boxes.Count;)
            {
                if (boxes[i].name == boxType)
                {
                    GameManager.instance.score += BoxManager.instance.valueDict[boxes[i].name];
                    Shop.instance.money += BoxManager.instance.valueDict[boxes[i].name];
                    Destroy(boxes[i]);
                    boxes.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            timeToRemove = maxTimeToRemove;
        }
    }

    public static void RemoveBoxFromAllDropZones(GameObject box)
    {
        foreach (DropZone dropZone in dropZones)
        {
            if (dropZone.boxes.Contains(box))
            {
                dropZone.boxes.Remove(box);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Box" && !boxes.Contains(other.gameObject))
        {
            GameObject box = other.gameObject;

            while (box != null)
            {
                AddBox(box);

                Vector3 castPos = box.transform.position + Vector3.up * 0.8f;

                box = null;

                RaycastHit[] cast = Physics.BoxCastAll(castPos, new Vector3(1f, 0.2f, 1f), Vector3.up, Quaternion.Euler(0, 0, 0), 0.001f);

                foreach (RaycastHit hit in cast)
                {
                    //print(hit.transform.name);
                    if (hit.transform.tag == "Box" && !boxes.Contains(hit.transform.gameObject))
                    {
                        box = hit.transform.gameObject;
                        break;
                    }
                }
                castPos.y += 0.8f;
            }
        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Box" && boxes.Contains(other.gameObject))
        {
            boxes.Remove(other.gameObject);
        }
    }
    */
}
