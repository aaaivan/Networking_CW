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
	string encryptionKey32;
	[SerializeField]
	string initializationVector16;
	[SerializeField]
	string fileName;
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
			filePath = Application.persistentDataPath + "/" + fileName;
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
		string encodedData = EncodeToBase64(serializedData);
		byte[] encryptedData = AesEncryption.Encrypt(encodedData, encryptionKey32, initializationVector16);
		File.WriteAllBytes(filePath, encryptedData);
	}

	public void LoadPlayerData()
	{
		if(!File.Exists(filePath))
		{
			playerData = new PlayerData();
			SavePlayerData();
		}
		byte[] encryptedData = File.ReadAllBytes(filePath);
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
}
