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
			noisePrefab.GetComponent<AudioSource>().volume = Mathf.Pow(collision.relativeVelocity.magnitude, 2) / 1000000f;
			//print(Mathf.Pow(collision.relativeVelocity.magnitude, 2) / 1000f);

            noisePrefab.transform.position = transform.position;
			//noisePrefab.GetComponent<AudioSource>().Play();
			Destroy(noisePrefab, 2);
		}
	}
}
