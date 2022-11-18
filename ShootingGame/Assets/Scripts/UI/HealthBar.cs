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
	Camera cam;
	private void Awake()
	{
		cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
		SetFillAmount(fillAmount);
	}

	private void LateUpdate()
	{
		transform.LookAt(transform.position + cam.transform.forward);

		float newFill = (float)playerMechanics.CurrentHealth / playerMechanics.Health;
		if(newFill != fillAmount)
		{
			SetFillAmount(newFill);
		}
	}

	public void SetFillAmount(float _fill)
	{
		_fill = Mathf.Clamp01(_fill);
		fillAmount = _fill;
		Vector3 currectScale = fillRect.transform.localScale;
		fillRect.transform.localScale = new Vector3(_fill, currectScale.y, currectScale.z);
	}
}
