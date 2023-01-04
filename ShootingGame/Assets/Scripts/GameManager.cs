using System.IO;
using UnityEngine;
using Leguar.TotalJSON;
using PlayFab;
using PlayFab.ClientModels;

public class GameManager : MonoBehaviour
{
	PlayerData playerData = null;
	string playFabPlayerID = "";

	public PlayerData PlayerData { get { return playerData; } }

	[SerializeField]
	string encryptionKey32;
	[SerializeField]
	string initializationVector16;
	[SerializeField]
	string extension;
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
			filePath = Application.persistentDataPath + "/HotCyclops/Saves";
			if(!Directory.Exists(filePath))
			{
				Directory.CreateDirectory(filePath);
			}

			Debug.Log(filePath);
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void SavePlayerData()
	{
		if (playFabPlayerID == null)
			return;

		string serializedData = JSON.Serialize(playerData).CreateString();
		string encodedData = EncodeToBase64(serializedData);
		byte[] encryptedData = AesEncryption.Encrypt(encodedData, encryptionKey32, initializationVector16);

		string filename = playFabPlayerID + extension;
		string path = Path.Combine(filePath, filename);

		File.WriteAllBytes(path, encryptedData);
	}

	public void LoadPlayerData()
	{
		if (playFabPlayerID == null)
			return;

		string filename = playFabPlayerID + extension;
		string path = Path.Combine(filePath, filename);

		if (!File.Exists(path))
		{
			playerData = new PlayerData();
			SavePlayerData();
		}

		byte[] encryptedData = File.ReadAllBytes(path);

		string encodedData = AesEncryption.Decrypt(encryptedData, encryptionKey32, initializationVector16);
		string serializedData = DecodeFromBase64(encodedData);
		playerData = JSON.ParseString(serializedData).Deserialize<PlayerData>();
	}

	string EncodeToBase64(string original)
	{
		var dataBytes = System.Text.Encoding.UTF8.GetBytes(original);
		return System.Convert.ToBase64String(dataBytes);
	}

	string DecodeFromBase64(string base64string)
	{
		var dataBytes = System.Convert.FromBase64String(base64string);
		return System.Text.Encoding.UTF8.GetString(dataBytes);
	}

	public void SetPlayFabID(string id)
	{
		playFabPlayerID = id;
		LoadPlayerData();

		// fetch the total number of kills from the PlayFab server
		GetLeaderboardAroundPlayerRequest request1 = new GetLeaderboardAroundPlayerRequest()
		{
			PlayFabId = playFabPlayerID,
			MaxResultsCount = 1,
			StatisticName = GlobalLeaderboard.Instance.mostKillsName,
		};
		PlayFabClientAPI.GetLeaderboardAroundPlayer(request1,
			GetTotalKillsResult,
			(error) => { Debug.Log("PlayFab - Error: " + error.ErrorMessage); });

		// fetch the total number of games from the PlayFab server
		GetLeaderboardAroundPlayerRequest request2 = new GetLeaderboardAroundPlayerRequest()
		{
			PlayFabId = playFabPlayerID,
			MaxResultsCount = 1,
			StatisticName = GlobalLeaderboard.Instance.totalGamesName,
		};
		PlayFabClientAPI.GetLeaderboardAroundPlayer(request2,
			GetTotalGamesResult,
			(error) => { Debug.Log("PlayFab - Error: " + error.ErrorMessage); });
	}

	void GetTotalKillsResult(GetLeaderboardAroundPlayerResult result)
	{
		if (result.Leaderboard.Count != 1)
			return;

		playerData.totalKills = result.Leaderboard[0].StatValue;
		SavePlayerData();
	}

	void GetTotalGamesResult(GetLeaderboardAroundPlayerResult result)
	{
		if (result.Leaderboard.Count != 1)
			return;

		playerData.totalGames = result.Leaderboard[0].StatValue;
		SavePlayerData();
	}
}
