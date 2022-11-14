using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerMenuButton : MonoBehaviour
{
	public void Restart()
	{
		SceneTransitionManager.Instance.LoadScene("SinglePlayer");
	}
	public void MainMenu()
	{
		SceneTransitionManager.Instance.LoadScene("Main");
	}

}
