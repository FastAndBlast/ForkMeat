using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Forklift : MonoBehaviour
{
    public List<GameObject> boxes = new List<GameObject>();

    Transform pivot;

    //string[] urlList = new string[2];

    private void Start()
    {
        pivot = transform.Find("Pivot");
    }

    private void Update()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].transform.position = pivot.Find(i.ToString()).position;
            boxes[i].transform.rotation = pivot.Find(i.ToString()).rotation;
        }


        if (Input.GetButtonDown("DropBoxes"))
        {
            DropBoxes();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Box")
        {
            GameObject box = other.gameObject;

            float y = 1.5f;

            while (box != null)
            {
                if (boxes.Count < PlayerController.instance.maxBoxes && !boxes.Contains(box))
                {
                    AddBox(box);
                }

                box = null;

                //Gizmos.DrawWireCube(pivot.transform.position + Vector3.up * y, Vector3.one * 0.5f);

                RaycastHit[] cast = Physics.BoxCastAll(pivot.transform.position + Vector3.up * y, Vector3.one * 0.5f, Vector3.up);

                foreach (RaycastHit hit in cast)    
                {
                    print(hit.transform.name);
                    if (hit.transform.tag == "Box")
                    {
                        box = hit.transform.gameObject;
                        break;
                    }
                }
                y++;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < 3; i++)
            {
                Gizmos.DrawWireCube(pivot.transform.position + Vector3.up * (i + 0.5f), Vector3.one * 1f);
            }
        }
    }


    public void AddBox(GameObject box)
    {
        boxes.Add(box);
        box.transform.GetComponent<BoxCollider>().enabled = false;
        box.transform.GetComponent<Rigidbody>().isKinematic = true;
        box.tag = "MovingBox";
        box.transform.position = pivot.Find((boxes.Count - 1).ToString()).position;
        //box.transform.SetParent(pivot.Find(boxes.Count.ToString()), false);
        //box.transform.localPosition = Vector3.zero;
        Conveyer.RemoveBox(box);
    }

    public void DropBoxes()
    {
        for (int i = 0; i < boxes.Count;)
        {
            //boxes[i].transform.SetParent(null, true);
            boxes[i].GetComponent<BoxCollider>().enabled = true;
            boxes[i].transform.GetComponent<Rigidbody>().isKinematic = false;
            boxes[i].tag = "Box";
            boxes.Remove(boxes[i]);
        }
        StartCoroutine(DisableCollider(2f));
    }

    public IEnumerator DisableCollider(float time)
    {
        GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(time);

        GetComponent<BoxCollider>().enabled = true;

        yield break;
    }

}
