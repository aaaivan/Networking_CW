using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] Transform respawnLocation;
    GameObject player;
    int deathCount = 0;


    private void Awake()
    {
        player = transform.parent.gameObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 4 /* water (used for the lava) */)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        player.transform.position = respawnLocation.position;
        player.transform.rotation = respawnLocation.rotation;
        player.GetComponent<ThirdPersonController>().Reset();
        Physics.SyncTransforms();
        ++deathCount;
    }
}
