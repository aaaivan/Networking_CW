using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
	[SerializeField]
	GameObject fillRect;
	Camera cam;
	private void Awake()
	{
		cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
	}

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
