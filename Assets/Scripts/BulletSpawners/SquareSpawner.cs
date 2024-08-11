using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSpawner : BulletSpawner
{
    //Transform[] firePoints [Top, Right, Down, Left]

    /// <summary>
    /// Initiates bullet firing pattern. 
    /// </summary>
    public void SpinFire(int times, float rotationAngle, float bulletAngle = 0)
    {
        StartCoroutine(SpinFireCoroutine(times, rotationAngle, bulletAngle));
    }

    private IEnumerator SpinFireCoroutine(int times, float rotationAngle, float bulletAngle)
    {
        isDoneFiring = false;
        for (int j = 0; j < times; j++)
        {
            GameObject[] bullets = FireOffAllFirePoints();
            foreach (GameObject bullet in bullets)
            {
                bullet.transform.Rotate(0, 0, bulletAngle);
            }

            StartCoroutine(smoothRotate(rotationAngle));
            yield return new WaitUntil(DoneRotating);
        }
        isDoneFiring = true;
    }

}
