using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
	public string username;
	public int bestScore;
	public int bestTime;
	public string bestScoreDate;
	public int playersInGame;
	public string roomName;

	public int GetEntries()
	{
		return 6;
	}

	public string GetKey(int index)
	{
		switch (index)
		{
			case 0:
				return "Username";
			case 1:
				return "Best Game Score";
			case 2:
				return "Best Game Remaining Time";
			case 3:
				return "Best Game Date";
			case 4:
				return "Best Game Total Players";
			case 5:
				return "Best Game Room Name";
			default:
				return string.Empty;
		}
	}

	public string GetValue(int index)
	{
		switch(index)
		{
			case 0:
				return username;
			case 1:
				return bestScore.ToString();
			case 2:
				return bestTime.ToString();
			case 3:
				return bestScoreDate;
			case 4:
				return playersInGame.ToString();
			case 5:
				return roomName;
			default:
				return string.Empty;
		}
	}
}
