using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public Vector2 size;

    public void TryAddBox(GameObject box)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Box")
        {
            TryAddBox(other.gameObject);
        }
    }

}
