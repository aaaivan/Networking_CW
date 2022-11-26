using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;

/// <summary>
/// Game over screen for th emultiplayer game
/// </summary>
public class MultiplayerGameOver : MonoBehaviour
{
	[SerializeField]
	TMP_Text winnerField;
	[SerializeField]
	string winnerString = "{0} won the game!";
	List<string> winnerList = new List<string>();

	/// <summary>
	/// Add the names of the winners to the game over screen
	/// </summary>
	/// <param name="_winners"> name of the winners </param>
	public void SetWinners(List<string> _winners)
	{
		foreach(var winner in _winners)
		{
			if (!winnerList.Contains(winner))
			{
				winnerList.Add(winner);
			}
		}

		// build the winners message
		string winnersList = "";
		for (int i = 0; i < winnerList.Count - 1; i++)
		{
			winnersList += winnerList[i];
			winnersList += ", ";
		}
		winnersList += winnerList[winnerList.Count - 1];
		winnerField.text = string.Format(winnerString, winnersList);
	}
}
