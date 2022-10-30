using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	[SerializeField, Tooltip("deg/s")]
	float rotationSpeed = 5;
	[SerializeField]
	Vector3 rotationAxis = new Vector3(0, 1, 0); 

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, rotationAxis);
		transform.rotation = transform.rotation * rotation;
    }
}
