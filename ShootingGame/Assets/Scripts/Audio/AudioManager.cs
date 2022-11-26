using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField]
	List<SoundEffect> sounds;

	static AudioManager instance;

	static public AudioManager Instance
	{
		get
		{
			if (instance == null)
				throw new UnityException("You need to add a AudioManager to your scene");
			return instance;
		}
	}

	private void Awake()
	{
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	/// <summary>
	/// Play the specified sound at the specified location
	/// </summary>
	/// <param name="name"> name of the sound </param>
	/// <param name="location"> location in world coordinates </param>
	public void Play3dSound(string name, Vector3 location)
	{
		SoundEffect sound = sounds.Find(s => s.name == name);
		if (sound == null)
		{
			return;
		}

		GameObject go = Instantiate(new GameObject(), location, Quaternion.identity);
		go.AddComponent<AudioSource>();
		AudioSource audioSource = go.AddComponent<AudioSource>();
		audioSource.loop = false;
		audioSource.volume = sound.volume;
		audioSource.pitch = sound.pitch;
		audioSource.clip = sound.clip;
		audioSource.playOnAwake = false;
		audioSource.Play();
		
		Destroy(go, sound.clip.length);
	}
}
