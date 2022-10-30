using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
	[SerializeField]
	GameObject fillRect;
	[SerializeField]
	Camera cam;

	private void LateUpdate()
	{
		transform.LookAt(transform.position + cam.transform.forward);
	}

	public void SetFillAmount(float fill)
	{
		fill = Mathf.Clamp01(fill);
		Vector3 currectScale = fillRect.transform.localScale;
		fillRect.transform.localScale = new Vector3(fill, currectScale.y, currectScale.z);
	}
}
