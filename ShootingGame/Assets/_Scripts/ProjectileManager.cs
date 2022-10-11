using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    int collisionCounter = 0;

    private void OnCollisionExit(Collision collision)
    {
        collisionCounter++;
        if(collision.collider.gameObject.layer == 6 /* Character */ )
        {
            // do damage
            Debug.Log("Do damage");
        }
        if (collisionCounter == 2)
        {
            Destroy(gameObject);
        }
    }
}
