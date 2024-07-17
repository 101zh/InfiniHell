using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float movementSpeed = 1f;

    public void setBulletSpeed(float speed)
    {
        movementSpeed = speed < 1f ? 1f : speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
