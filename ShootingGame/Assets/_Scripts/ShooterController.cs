using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    StarterAssetsInputs inputs = null;
    [SerializeField] GameObject projectilePrefab = null;
    [SerializeField] Transform spawnPosition = null;

    [Header("Fire Parameters")]
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] float fireAngle = 45;
    [SerializeField] float firePower = 2;

    float _lastShotTime;

    private void Awake()
    {
        inputs = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inputs.fire && Time.time > fireRate + _lastShotTime)
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        _lastShotTime = Time.time;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition.position, spawnPosition.rotation);
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();
        Vector3 forceDirection = transform.forward * Mathf.Cos(Mathf.Deg2Rad * fireAngle) + transform.up * Mathf.Sin(Mathf.Deg2Rad * fireAngle);
        rigidBody.AddForce(forceDirection * firePower, ForceMode.Impulse);
    }

}
