using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
	static SceneTransitionManager instance;

	static public SceneTransitionManager Instance
	{
		get
		{
			if (instance == null)
				throw new UnityException("You need to add a SceneTransitionManager to your scene");
			return instance;
		}
	}

	private void OnDestroy()
	{
		instance = null; 
	}

	private void Awake()
	{
		instance = this;
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
