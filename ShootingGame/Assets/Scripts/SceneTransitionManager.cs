using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
	static SceneTransitionManager instance;

	static public SceneTransitionManager Instance
	{
		get { return instance; }
	}

	private void OnDestroy()
	{
		if(instance == this)
		{
			instance = null;
		}
	}

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
