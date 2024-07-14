using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float movementSpeed = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("run");
        Destroy(gameObject);
    }
}
