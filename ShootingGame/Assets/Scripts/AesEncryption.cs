// Source: https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-7.0

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AesEncryption
{
	public static byte[] Encrypt(string _original, string _key, string _IV)
	{
		byte[] key = Encoding.ASCII.GetBytes(_key);
		byte[] IV = Encoding.ASCII.GetBytes(_IV);

		// Encrypt the string to an array of bytes.
		return EncryptStringToBytes_Aes(_original, key, IV);
	}

	public static string Decrypt(byte[] _encrypted, string _key, string _IV)
	{
		byte[] key = Encoding.ASCII.GetBytes(_key);
		byte[] IV = Encoding.ASCII.GetBytes(_IV);

		return DecryptStringFromBytes_Aes(_encrypted, key, IV);
	}


	static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
	{
		// Check arguments.
		if (plainText == null || plainText.Length <= 0)
			throw new ArgumentNullException("plainText");
		if (Key == null || Key.Length <= 0)
			throw new ArgumentNullException("Key");
		if (IV == null || IV.Length <= 0)
			throw new ArgumentNullException("IV");
		byte[] encrypted;

		// Create an Aes object
		// with the specified key and IV.
		using (Aes aesAlg = Aes.Create())
		{
			aesAlg.Key = Key;
			aesAlg.IV = IV;

			// Create an encryptor to perform the stream transform.
			ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

			// Create the streams used for encryption.
			using (MemoryStream msEncrypt = new MemoryStream())
			{
				using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
				{
					using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
					{
						//Write all data to the stream.
						swEncrypt.Write(plainText);
					}
					encrypted = msEncrypt.ToArray();
				}
			}
		}

		// Return the encrypted bytes from the memory stream.
		return encrypted;
	}

	static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
	{
		// Check arguments.
		if (cipherText == null || cipherText.Length <= 0)
			throw new ArgumentNullException("cipherText");
		if (Key == null || Key.Length <= 0)
			throw new ArgumentNullException("Key");
		if (IV == null || IV.Length <= 0)
			throw new ArgumentNullException("IV");

		// Declare the string used to hold
		// the decrypted text.
		string plaintext = null;

		// Create an Aes object
		// with the specified key and IV.
		using (Aes aesAlg = Aes.Create())
		{
			aesAlg.Key = Key;
			aesAlg.IV = IV;

			// Create a decryptor to perform the stream transform.
			ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

			// Create the streams used for decryption.
			using (MemoryStream msDecrypt = new MemoryStream(cipherText))
			{
				using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					using (StreamReader srDecrypt = new StreamReader(csDecrypt))
					{

						// Read the decrypted bytes from the decrypting stream
						// and place them in a string.
						plaintext = srDecrypt.ReadToEnd();
					}
				}
			}
		}

		return plaintext;
	}
}