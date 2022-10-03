using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Forklift : MonoBehaviour
{
    public List<GameObject> boxes = new List<GameObject>();

    public BoxCollider col;

    Transform pivot;

    //string[] urlList = new string[2];

    Vector3 centerOfMass;

    public static Forklift instance;

    public GameObject poofParticles;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pivot = transform.Find("Pivot");

        //centerOfMass = PlayerController.instance.GetComponent<Rigidbody>().centerOfMass;

        //PlayerController.instance.GetComponent<Rigidbody>().centerOfMass = centerOfMass;

        col.enabled = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("DropAllBoxes"))
        {
            DropBoxes();
			AudioManager.instance.Play("Putdown");
        }
        if (Input.GetButtonDown("DropBox") && boxes.Count > 0 && !Shop.instance.open)
        {
            RemoveBox();
			AudioManager.instance.Play("Putdown");
        }
    }

    public void FixedUpdate()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].transform.position = pivot.Find(i.ToString()).position;
            //boxes[i].GetComponent<Rigidbody>().MovePosition(pivot.Find(i.ToString()).position);
            //boxes[i].GetComponent<Rigidbody>().AddForce((transform.position - boxes[i].transform.position) * 5f);

            boxes[i].transform.rotation = pivot.Find(i.ToString()).rotation;
            boxes[i].transform.Rotate(Vector3.right, -90);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Box")
        {
            GameObject box = other.gameObject;

            Vector3 castPos = new Vector3(pivot.transform.position.x, 1.2f, pivot.transform.position.z);

            while (box != null)
            {
                if (boxes.Count < PlayerController.instance.maxBoxes && !boxes.Contains(box))
                {
                    AddBox(box);
					AudioManager.instance.Play("Pickup");
                }

                box = null;

                //Gizmos.DrawWireCube(pivot.transform.position + Vector3.up * y, Vector3.one * 0.5f);

                RaycastHit[] cast = Physics.BoxCastAll(castPos, new Vector3(1f, 0.2f, 1f), Vector3.up, pivot.rotation, 0.001f);

                foreach (RaycastHit hit in cast)
                {
                    //print(hit.transform.name);
                    if (hit.transform.tag == "Box")
                    {
                        box = hit.transform.gameObject;
                        break;
                    }
                }
                castPos.y += 0.8f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        /*
        if (Application.isPlaying)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 castPos = new Vector3(pivot.transform.position.x, i * 0.8f + 1.2f, pivot.transform.position.z);
                Gizmos.DrawWireCube(castPos, new Vector3(2f, 0.4f, 2f));
            }
        }
        */
    }


    public void AddBox(GameObject box)
    {
        boxes.Add(box);
        box.GetComponent<BoxCollider>().enabled = false;
        box.GetComponent<Rigidbody>().isKinematic = true;
        box.tag = "MovingBox";
        GameObject particles = Instantiate(poofParticles);
        particles.transform.position = box.transform.position;
        box.transform.position = pivot.Find((boxes.Count - 1).ToString()).position;
        //box.transform.SetParent(pivot.Find(boxes.Count.ToString()), false);
        //box.transform.localPosition = Vector3.zero;
        Conveyer.RemoveBoxFromAllConveyers(box);
        DropZone.RemoveBoxFromAllDropZones(box);
    }

    public void RemoveBox()
    {
        GameObject box = boxes[boxes.Count - 1];
        box.GetComponent<BoxCollider>().enabled = true;
        box.GetComponent<Rigidbody>().isKinematic = false;
        //box.transform.SetParent(null, false);

        //Vector3 randomForce = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(0, 1f)).normalized;
        Vector3 randomForce = Vector3.forward;
        box.GetComponent<Rigidbody>().mass = 0.1f;
        box.transform.position += Vector3.up * 0.5f;
        box.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(randomForce) * PlayerController.instance.boxThrowStrength);
        //box.GetComponent<Rigidbody>().AddForce(Vector3.up * PlayerController.instance.boxThrowStrength);
        box.GetComponent<Rigidbody>().AddForce(Vector3.up * PlayerController.instance.boxThrowUpStrength);
        box.GetComponent<Rigidbody>().AddTorque(Vector3.right * PlayerController.instance.spin);
        //box.tag = "Box";
        StartCoroutine(FixBox(0.5f, box));

        boxes.Remove(box);
    }

    public void DropBoxes()
    {
        for (int i = 0; i < boxes.Count;)
        {
            //boxes[i].transform.SetParent(null, true);
            boxes[i].GetComponent<BoxCollider>().enabled = true;
            boxes[i].GetComponent<Rigidbody>().isKinematic = false;
            boxes[i].tag = "Box";
            boxes.Remove(boxes[i]);
        }
        StartCoroutine(DisableCollider(2f));
    }

    public void ExplodeBoxes()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            RemoveBox();
        }
    }

    public IEnumerator DisableCollider(float time)
    {
        BoxCollider[] cols = GetComponents<BoxCollider>();
        foreach (BoxCollider col in cols)
        {
            col.enabled = false;
        }

        //GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(time);

        //GetComponent<BoxCollider>().enabled = true;

        foreach (BoxCollider col in cols)
        {
            col.enabled = true;
        }

        PlayerController.instance.GetComponent<Rigidbody>().centerOfMass = centerOfMass;

        yield break;
    }

    public IEnumerator FixBox(float time, GameObject box)
    {
        yield return new WaitForSeconds(time);
        box.GetComponent<Rigidbody>().mass = 10f;
        box.tag = "Box";
        Conveyer.CheckBoxAtAllConveyers(box);
        DropZone.CheckBoxAtAllDropZones(box);
        yield break;
    }
}
