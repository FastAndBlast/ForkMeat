using UnityEngine.Audio;
using System;
using UnityEngine;

//Credit to Brackeys youtube tutorial on Audio managers, as the majority of this code and learning how to use it was made by him.


public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    //AudioManager

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = 1;
            s.source.pitch = 1;
            s.source.loop = s.loop;
			s.source.time = s.startTime;
        }
    }

	void Start()
    {
        Play("Song", 0.1f);
      
    }

	public void SendHelp() 
	{
		Sound s = Array.Find(sounds, sound => sound.name == "Click");
		s.source.time = s.startTime;
        s.source.Play();
	}

    public void Play(string name, float volume=1, float pitch=1)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
		s.source.volume = volume;
		s.source.pitch = pitch;
		s.source.time = s.startTime;
        s.source.Play();
    }

    //this addition to the code was made by me, the rest was from Brackeys tutorial
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source.Stop();
    }
}