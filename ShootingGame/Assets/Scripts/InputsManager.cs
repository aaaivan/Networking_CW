using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class InputsManager : MonoBehaviour
{
	[HideInInspector]
	public ThirdPersonController thirdPersonController;

	static InputsManager instance;
	public static InputsManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void OnDestroy()
	{
		if(instance == this)
		{
			instance = null;
		}
	}

	public void Awake()
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

	public void DisableThirdPersonInputs()
	{
		ThirdPersonController tpc = thirdPersonController;
		tpc.DisableGameInputs();
	}

	public void EnableThirdPersonInputs()
	{
		ThirdPersonController tpc = thirdPersonController;
		tpc.EnableGameInputs();
	}

	public void ToggleThirdPersonInputs()
	{
		ThirdPersonController tpc = thirdPersonController;
		if(tpc.AreInputsEnabled())
			DisableThirdPersonInputs();
		else
			EnableThirdPersonInputs();
	}
}
