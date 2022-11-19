using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;

public class MultiplayerGameOver : MonoBehaviour
{
	[SerializeField]
	TMP_Text winnerField;
	[SerializeField]
	string winnerString = "{0} won the game!";
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

		string winnersList = "";
		for(int i = 0; i < winnerList.Count - 1; i++)
		{
			winnersList += winnerList[i];
			winnersList += ", ";
		}
		winnersList += winnerList[winnerList.Count - 1];
		winnerField.text = string.Format(winnerString, winnersList);
	}
}
