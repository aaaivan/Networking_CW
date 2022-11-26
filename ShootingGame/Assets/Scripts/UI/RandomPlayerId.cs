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

	void Start()
    {
		// assign a random id to the player
		playerId.text = string.Format("Player {0}", Random.Range(1, 1000000));
    }
}
