using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class InputsManager : MonoBehaviour
{
	public ThirdPersonController thirdPersonController;

	static InputsManager instance;
	public static InputsManager Instance
	{
		get
		{
			if (instance == null)
				throw new UnityException("You need to add an InputsManager to your scene");
			return instance;
		}
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public void Awake()
	{
		instance = this;
	}
}
