using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigationManager : MonoBehaviour
{
	[SerializeField]
	MenuData[] menuData;
	
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

	public void ShowMenu(string menuName)
	{
		bool found = false;

		for (int i = 0; i < menuData.Length; i++)
		{
			MenuData m = menuData[i];
			m.menuObject.SetActive(m.menuName == menuName);

			found |= m.menuName == menuName;
		}

		if(!found)
		{
			Debug.LogError($"No menu called {menuName} exists!");
		}

	}

	public GameObject MenuGet(string menuName)
	{
		for (int i = 0; i < menuData.Length; i++)
		{
			MenuData m = menuData[i];
			if (m.menuName == menuName)
				return m.menuObject;
		}

		return null;
	}
}
