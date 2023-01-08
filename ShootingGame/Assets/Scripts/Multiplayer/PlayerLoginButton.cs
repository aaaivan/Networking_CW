using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayerLoginButton : MonoBehaviour
{
	[SerializeField]
	TMP_InputField playerNameField;
	[SerializeField]
	TMP_InputField passwordField;
	[SerializeField]
	TMP_Text warningText;
	string instructionString = "The username must be at least 3 characters long.\nThe password must be at least 6 characters long.";
	string connectingString = "Connecting...";

	private void Awake()
	{
		playerNameField.onSelect.AddListener((string s) =>
		{
			warningText.text = instructionString;
		});
		passwordField.onSelect.AddListener((string s) =>
		{
			warningText.text = instructionString;
		});
	}

	private void OnDestroy()
	{
		playerNameField.onSelect.RemoveAllListeners();
		passwordField.onSelect.RemoveAllListeners();
	}

	private void OnEnable()
	{
		warningText.text = instructionString;
	}

	public void OnSubmit()
	{
		if(playerNameField.text.Trim().Length > 0 && passwordField.text.Length >= 6)
		{
			LogIn(playerNameField.text.Trim(), CreateAccount.MD5Hash(passwordField.text));
		}
		playerNameField.text = "";
		passwordField.text = "";
	}

	/// <summary>
	/// Login with specified username and password
	/// </summary>
	/// <param name="username"></param>
	/// <param name="pass"></param>
	void LogIn(string username, string pass)
	{
		warningText.text = connectingString;

		LoginWithPlayFabRequest request = new LoginWithPlayFabRequest()
		{
			Password = pass,
			Username = username,
			InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
			{
				// make sure PlayerProfile is included
				GetPlayerProfile = true,
				// Define rules for PlayerProfile request
				ProfileConstraints = new PlayerProfileViewConstraints()
				{
					ShowDisplayName = true,
				}
			}
		};
		PlayFabClientAPI.LoginWithPlayFab(request, LoginSuccessCallback, LoginFailCallback);
	}

	void LoginSuccessCallback(LoginResult result)
	{
		Debug.Log("PlayFab - Login success for user: " + result.PlayFabId);
		GameManager.Instance.SetPlayFabID(result.PlayFabId);
		NetworkManager.Instance.ConnectToMaster(result.InfoResultPayload.PlayerProfile.DisplayName);
	}

	void LoginFailCallback(PlayFabError error)
	{
		Debug.Log("PlayFab - Login fail: " + error.ErrorMessage);
		warningText.text = error.ErrorMessage;
	}


}
