using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectile : Projectile
{
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
