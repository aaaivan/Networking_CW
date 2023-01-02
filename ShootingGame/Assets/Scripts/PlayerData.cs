using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
	public string uid;

	public string username;
	public int bestScore;
	public string bestScoreDate;
	public int playersInGame;
	public string roomName;

	public PlayerData()
	{
		uid = Guid.NewGuid().ToString();
	}

	public int GetEntries()
	{
		return 5;
	}

	public string GetKey(int index)
	{
		switch (index)
		{
			case 0:
				return "Username";
			case 1:
				return "Best Score";
			case 2:
				return "Date";
			case 3:
				return "Total Players";
			case 4:
				return "Room Name";
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
				return bestScoreDate;
			case 3:
				return playersInGame.ToString();
			case 4:
				return roomName;
			default:
				return string.Empty;
		}
	}
}
