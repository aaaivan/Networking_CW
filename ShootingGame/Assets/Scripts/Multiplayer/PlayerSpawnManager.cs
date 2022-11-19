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
		int index = -1;
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

		Transform spawnTransf = spawnLocations[index % spawnLocations.Count];
		GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnTransf.position, spawnTransf.rotation);
		followPlayerCam.Follow = player.transform.Find("PlayerCameraRoot");
		InputsManager.Instance.thirdPersonController = player.GetComponent<ThirdPersonController>();
	}
}
