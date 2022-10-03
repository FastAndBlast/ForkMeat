using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionNoise : MonoBehaviour
{
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag != "Player") 
		{
			//AudioManager.instance.Play("WoodHit", Mathf.Pow(collision.relativeVelocity.magnitude, 2) / 60);
			GameObject noisePrefab = Instantiate(AudioManager.instance.boxNoisePrefab);
			noisePrefab.GetComponent<AudioSource>().volume = Mathf.Pow(collision.relativeVelocity.magnitude, 2) / 150;
			//print(Mathf.Pow(collision.relativeVelocity.magnitude, 2));
			//print(collision.relativeVelocity.magnitude);

            noisePrefab.transform.position = transform.position;
			//noisePrefab.GetComponent<AudioSource>().Play();
			Destroy(noisePrefab, 2);
		}
	}
}
