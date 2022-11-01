using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigationManager : MonoBehaviour
{
	[SerializeField]
	GameObject[] menus;
	
	static MenuNavigationManager instance;

	static public MenuNavigationManager Instance
	{
		get
		{
			if (instance == null)
				throw new UnityException("You need to add a MenuNavigationManager to your scene");
			return instance;
		}
	}
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != null)
		{
			Destroy(gameObject);
		}
	}
	public void ShowMenu(int id)
	{
		for(int i = 0; i < menus.Length; i++)
		{
			menus[i].SetActive(i == id);
		}
	}
}
