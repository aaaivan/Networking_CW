using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Leguar.TotalJSON;

public class GameManager : MonoBehaviour
{
	PlayerData playerData;
	public PlayerData PlayerData { get { return playerData; } }

	[SerializeField]
	string filePath;

	static GameManager instance;
	public static GameManager Instance { get { return instance; } }

	private void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);

			LoadPlayerData();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void SavePlayerData()
	{
		string serializedData = JSON.Serialize(playerData).CreateString();
		File.WriteAllText(filePath, serializedData);
	}

	public void LoadPlayerData()
	{
		if(!File.Exists(filePath))
		{
			playerData = new PlayerData();
			SavePlayerData();
		}
		string serializedData = File.ReadAllText(filePath);
		playerData = JSON.ParseString(serializedData).Deserialize<PlayerData>();
	}
}
