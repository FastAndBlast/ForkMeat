using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkliftCollision : MonoBehaviour
{
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Box") 
		{
			if (collision.relativeVelocity.magnitude > 3)
			{
				AudioManager.instance.Play("WoodMetalHit", Mathf.Pow(collision.relativeVelocity.magnitude, 2) / 20);
			}
			else
			{
				AudioManager.instance.Play("WoodHit", Mathf.Pow(collision.relativeVelocity.magnitude, 2) / 20);
			}	
		}
		if (collision.gameObject.tag == "Metal") 
		{
			int rInt = Random.Range(1, 5);	
			AudioManager.instance.Play("MetalHit" + rInt.ToString(), Mathf.Pow(collision.relativeVelocity.magnitude, 2) / 20);
		}
		if (collision.gameObject.tag == "Wood")
		{
			AudioManager.instance.Play("WoodHit", Mathf.Pow(collision.relativeVelocity.magnitude, 2) / 20);
		}
	}
}
