using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSpawner : BulletSpawner
{
    //Transform[] firePoints [Top, Right, Down, Left]

    /// <summary>
    /// Initiates bullet firing pattern. 
    /// </summary>
    public void SpinFire(int times, float rotationAngle)
    {
        StartCoroutine(SpinFireCoroutine(times, rotationAngle));
    }

    private IEnumerator SpinFireCoroutine(int times, float rotationAngle)
    {
        isDoneFiring = false;
        for (int j = 0; j < times; j++)
        {
            GameObject[] bullets = FireOffAllFirePoints();

            StartCoroutine(smoothRotate(rotationAngle));
            yield return new WaitUntil(DoneRotating);
        }
        isDoneFiring = true;
    }

}
