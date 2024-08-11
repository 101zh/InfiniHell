using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] protected Transform[] firePoints;
    [SerializeField] protected float smoothTimeFactor = 0.75f;

    private bool isDoneRotating = false;
    protected bool isDoneFiring = false;
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

    public bool DoneFiring()
    {
        return isDoneFiring;
    }

    protected bool DoneRotating()
    {
        return isDoneRotating;
    }

    protected IEnumerator smoothRotate(float rotationAngle)
    {
        isDoneRotating = false;
        float zVelocity = 0.0f;

        float targetAngle = transform.eulerAngles.z + rotationAngle > 360 ? transform.eulerAngles.z + rotationAngle - 360 : transform.eulerAngles.z + rotationAngle;

        while (!(transform.eulerAngles.z < targetAngle + 0.05 && transform.eulerAngles.z > targetAngle - 0.05))
        {
            float angle = Mathf.SmoothDamp(transform.eulerAngles.z, targetAngle, ref zVelocity, smoothTimeFactor);
            transform.rotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }
        isDoneRotating = true;
    }

    public void setBulletSpeed(float speed)
    {
        straightProjectile.GetComponent<Projectile>().setBulletSpeed(speed);
        waveProjectile.GetComponent<Projectile>().setBulletSpeed(speed);
    }

    public void setSmoothTimeFactor(float time)
    {
        smoothTimeFactor = time < 0 ? 0 : time;
    }
}