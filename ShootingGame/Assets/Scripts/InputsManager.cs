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
		ThirdPersonController fpc = InputsManager.Instance.thirdPersonController;
		GameObject player = fpc.gameObject;
		StarterAssetsInputs inputs = player.GetComponent<StarterAssetsInputs>();
		if (Input.mousePresent)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			inputs.cursorLocked = false;
			inputs.cursorInputForLook = false;
		}
		fpc.DisableGameInputs();
	}

	public void EnableThirdPersonInputs()
	{
		ThirdPersonController fpc = InputsManager.Instance.thirdPersonController;
		GameObject player = fpc.gameObject;
		StarterAssetsInputs inputs = player.GetComponent<StarterAssetsInputs>();
		if (Input.mousePresent)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			inputs.cursorLocked = true;
			inputs.cursorInputForLook = true;
		}
		fpc.EnableGameInputs();
	}
}
