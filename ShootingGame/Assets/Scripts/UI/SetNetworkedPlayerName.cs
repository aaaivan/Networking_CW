using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetNetworkedPlayerName : MonoBehaviour
{
	private void Awake()
	{
		Photon.Realtime.Player owner = GetComponentInParent<PhotonView>().Owner;
		if (owner != null)
		{
			TMP_Text nameField = gameObject.GetComponent<TMP_Text>();
			nameField.text = owner.NickName;
		}
	}
}
