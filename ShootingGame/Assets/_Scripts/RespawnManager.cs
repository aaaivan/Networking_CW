using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
	static RespawnManager instance;
	static public RespawnManager Instance
	{
		get
		{
			if (instance == null)
				throw new UnityException("You need to add a RespawnManager to your scene");
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
		else if (instance != null)
		{
			Destroy(gameObject);
		}
	}
	
	public void Respawn(GameObject player)
	{
		player.transform.position = gameObject.transform.position;
		player.transform.rotation = gameObject.transform.rotation;
		Physics.SyncTransforms();
	}

}
