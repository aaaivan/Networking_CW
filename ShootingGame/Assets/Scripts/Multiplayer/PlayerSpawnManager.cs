using Cinemachine;
using Photon.Pun;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


public class PlayerSpawnManager : MonoBehaviour
{
	[SerializeField]
	GameObject playerPrefab;
	[SerializeField]
	CinemachineVirtualCamera followPlayerCam;

	// Start is called before the first frame update
	void Start()
    {


		GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(20, 10, 0), Quaternion.identity);

		StarterAssetsInputs inputs = player.GetComponent<StarterAssetsInputs>();
		if (UnityEngine.Input.mousePresent)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			inputs.cursorLocked = false;
		}

		PhotonView photonView = player.gameObject.GetComponent<PhotonView>();
		if ( photonView != null && photonView.IsMine)
		{
			followPlayerCam.Follow = player.transform.Find("PlayerCameraRoot");
		}
	}
}
