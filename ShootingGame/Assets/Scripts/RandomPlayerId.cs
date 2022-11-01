using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomPlayerId : MonoBehaviour
{
	TMP_InputField playerId;
	private void Awake()
	{
		playerId = GetComponent<TMP_InputField>();
	}

	// Start is called before the first frame update
	void Start()
    {
		playerId.text = string.Format("Player {0}", Random.Range(1, 1000000));
    }
}
