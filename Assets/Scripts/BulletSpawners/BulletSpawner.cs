using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] protected Transform[] firePoints;
    [SerializeField] protected float timedelay = 0.75f;

    protected GameObject straightProjectile;
    protected GameObject waveProjectile;
    void Awake()
    {
        if (straightProjectile == null)
            straightProjectile = (GameObject)Resources.Load("Prefabs/Straight Projectile");
        if (waveProjectile == null)
            waveProjectile = (GameObject)Resources.Load("Prefabs/Wave Projectile");
    }

    protected GameObject[] FireOffAllFirePoints()
    {
        GameObject[] bullets = new GameObject[firePoints.Length];

        for (int i = 0; i < firePoints.Length; i++)
        {
            bullets[i] = Instantiate(straightProjectile, firePoints[i].position, firePoints[i].rotation);
        }

        return bullets;
    }

    public void setBulletSpeed(float speed)
    {
        straightProjectile.GetComponent<Projectile>().setBulletSpeed(speed);
        waveProjectile.GetComponent<Projectile>().setBulletSpeed(speed);
    }

    public void setTimeDelay(float time)
    {
        timedelay = time < 0 ? 0 : time;
    }
}