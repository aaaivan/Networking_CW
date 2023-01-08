using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateAccount : MonoBehaviour
{
	[SerializeField]
	TMP_InputField usernameField;
	[SerializeField]
	TMP_InputField passwordField;
	[SerializeField]
	TMP_InputField repeatPasswordField;
	[SerializeField]
	TMP_Text warningText;
	string instructionString = "The username must be at least 3 characters long.\nThe password must be at least 6 characters long.";
	string successString = "Account created successfully!\nGo back and log in to start playing!";
	string shortUsernameWarning = "The username is too short!";
	string shortPassWarning = "The password is too short!";
	string passNotMatchingWarning = "The passwords don't match!";

	private void Awake()
	{
		usernameField.onSelect.AddListener((string s) =>
		{
			warningText.text = instructionString;
		});
		passwordField.onSelect.AddListener((string s) =>
		{
			warningText.text = instructionString;
		});
		repeatPasswordField.onSelect.AddListener((string s) =>
		{
			warningText.text = instructionString;
		});
	}

	private void OnDestroy()
	{
		usernameField.onSelect.RemoveAllListeners();
		passwordField.onSelect.RemoveAllListeners();
		repeatPasswordField.onSelect.RemoveAllListeners();
	}

	private void OnEnable()
	{
		warningText.text = instructionString;
	}

	public void TryCreateAccount()
	{
		if (usernameField.text.Trim().Length >= 3 &&
			passwordField.text.Length >= 6 &&
			passwordField.text == repeatPasswordField.text)
		{
			DoCreateAccount(usernameField.text.Trim(), MD5Hash(passwordField.text));
		}
		else if(usernameField.text.Trim().Length < 3)
		{
			warningText.text = shortUsernameWarning;
		}
		else if (passwordField.text.Trim().Length < 6)
		{
			warningText.text = shortPassWarning;
		}
		else if (passwordField.text != repeatPasswordField.text)
		{
			warningText.text = passNotMatchingWarning;
		}

		usernameField.text = "";
		passwordField.text = "";
		repeatPasswordField.text = "";
	}

	/// <summary>
	/// Create a player account with the specified username and password
	/// </summary>
	/// <param name="username"> Username. Cannot match an existing username. </param>
	/// <param name="pass"> Password. Must be at least 6 characters. </param>
	void DoCreateAccount(string username, string pass)
	{
		RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
		{
			DisplayName = username, // use the username as the display name too
			Username = username,
			Password = pass,
			RequireBothUsernameAndEmail = false,
		};
		PlayFabClientAPI.RegisterPlayFabUser(request, AccountCreatedSuccessCallback, AccountCreatedFailCallback);
	}

	void AccountCreatedSuccessCallback(RegisterPlayFabUserResult result)
	{
		Debug.Log("PlayFab - Account created successfully");
		warningText.text = successString;
	}

	void AccountCreatedFailCallback(PlayFabError error)
	{
		Debug.Log("PlayFab - Account creation failed: " + error.ErrorMessage);
		warningText.text = error.ErrorMessage;
	}

	public static string MD5Hash(string pass)
	{
		var md5 = System.Security.Cryptography.MD5.Create();
		byte[] bs = System.Text.Encoding.UTF8.GetBytes(pass);
		bs = md5.ComputeHash(bs);
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach (byte b in bs)
		{
			sb.Append(b.ToString("x2").ToLower());
		}

		return sb.ToString();
	}

}
