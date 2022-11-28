using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinglePlayerMenuButton : MonoBehaviour
{
	public void Restart()
	{
		SceneManager.LoadScene("SinglePlayer");
	}
	public void MainMenu()
	{
		SceneManager.LoadScene("Main");
	}
}
