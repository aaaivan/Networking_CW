using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpawnManager : MonoBehaviour
{
	[SerializeField]
	List<Transform> spawnLocations;
	[SerializeField]
	GameObject playerPrefab;
	[SerializeField]
	CinemachineVirtualCamera followPlayerCam;

	void Start()
    {
		int index = -1; // index of the local player in the PhotonNetwork.PlayerList
		for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
		{
			if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
			{
				index = i;
				break;
			}
		}
		if (index == -1)
			return;

		// since each player has a different index, this ensures no player spawns in the same location
		Transform spawnTransf = spawnLocations[index % spawnLocations.Count];
		// instantiate networked player character
		GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnTransf.position, spawnTransf.rotation);
		// have the camera following the local player's character
		followPlayerCam.Follow = player.transform.Find("PlayerCameraRoot");
		// register the ThirdPersonController with the InputsManger so that it can be enabled/disabled
		InputsManager.Instance.thirdPersonController = player.GetComponent<ThirdPersonController>();
	}
}
