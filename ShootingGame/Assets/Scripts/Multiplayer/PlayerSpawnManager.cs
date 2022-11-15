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
	List<Transform> spawnLocations;
	[SerializeField]
	GameObject playerPrefab;
	[SerializeField]
	CinemachineVirtualCamera followPlayerCam;

	// Start is called before the first frame update
	void Start()
    {
		Transform spawnTransf = spawnLocations[PhotonNetwork.LocalPlayer.ActorNumber % spawnLocations.Count];
		GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnTransf.position, spawnTransf.rotation);
		PlayerMechanics playerMechanics = player.GetComponent<PlayerMechanics>();
		if ( playerMechanics.IsLocalPlayer )
		{
			followPlayerCam.Follow = player.transform.Find("PlayerCameraRoot");
		}
	}
}
