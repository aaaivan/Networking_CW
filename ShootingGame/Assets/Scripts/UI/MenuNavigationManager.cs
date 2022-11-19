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
			return instance;
		}
	}

	public void OnDestroy()
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
	public void ShowMenu(int id)
	{
		for(int i = 0; i < menus.Length; i++)
		{
			menus[i].SetActive(i == id);
		}
	}
}
