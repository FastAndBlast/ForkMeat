using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionNoise : MonoBehaviour
{
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag != "Player") 
		{
			AudioManager.instance.Play("WoodHit", Mathf.Pow(collision.relativeVelocity.magnitude, 2) / 60);
		}
	}
}
