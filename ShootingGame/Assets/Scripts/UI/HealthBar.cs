using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
	[SerializeField]
	GameObject fillRect;
	[SerializeField]
	PlayerMechanics playerMechanics;
	float fillAmount = 1.0f;
	Camera cam; // main camera

	private void Awake()
	{
		cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
		SetFillAmount(fillAmount);
	}

	private void LateUpdate()
	{
		// billboarding code
		transform.LookAt(transform.position + cam.transform.forward);

		// check if the health of the player has changed. If yes, update the health bar.
		float newFill = (float)playerMechanics.CurrentHealth / playerMechanics.FullHealth;
		if(newFill != fillAmount)
		{
			SetFillAmount(newFill);
		}
	}

	/// <summary>
	/// set the fill amount of the health bar
	/// </summary>
	/// <param name="_fill"> fill amount, should be between 0 and 1 </param>
	public void SetFillAmount(float _fill)
	{
		_fill = Mathf.Clamp01(_fill);
		fillAmount = _fill;
		Vector3 currectScale = fillRect.transform.localScale;
		fillRect.transform.localScale = new Vector3(_fill, currectScale.y, currectScale.z);
	}
}
