using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;

public class MultiplayerGameOver : MonoBehaviour
{
	[SerializeField]
	TMP_Text winnerField;
	List<string> winnerList = new List<string>();

	public void SetWinners(List<string> _winners)
	{
		foreach(var winner in _winners)
		{
			SetWinner(winner);
		}
	}

	public void SetWinner(string _winner)
	{
		if(!winnerList.Contains(_winner))
		{
			winnerList.Add(_winner);
		}

		string winnersString = "";
		for(int i = 0; i < winnerList.Count - 1; i++)
		{
			winnersString += winnerList[i];
			winnersString += ", ";
		}
		winnersString += winnerList[winnerList.Count - 1];
		winnerField.text = winnersString;
	}
}
