using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


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
		Transform spawnTransf = spawnLocations[PhotonNetwork.LocalPlayer.ActorNumber % spawnLocations.Count];
		GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnTransf.position, spawnTransf.rotation);
		followPlayerCam.Follow = player.transform.Find("PlayerCameraRoot");
		InputsManager.Instance.thirdPersonController = player.GetComponent<ThirdPersonController>();
	}
}
