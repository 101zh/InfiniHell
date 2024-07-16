using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayerSpawner : BulletSpawner
{
    //Transform[] firePoints [Right, MidRight, Mid, MidLeft, Left]

    /// <summary>
    /// Initiates bullet firing pattern. 
    /// </summary>
    /// <param name="times"></param>
    /// <returns>The seconds for firing pattern to end</returns>
    public float FireOffAllFirePoints(int times)
    {
        StartCoroutine(FireOffAllFirePointsCoroutine(times));
        return times * timedelay;
    }

    /// <summary>
    /// Initiates bullet firing pattern. 
    /// </summary>
    /// <param name="times"></param>
    /// <returns>The seconds for firing pattern to end</returns>
    public float MiddleFire(int times)
    {
        StartCoroutine(MiddleFireCoroutine(times));
        return times * timedelay;
    }

    private IEnumerator MiddleFireCoroutine(int times)
    {
        for (int i = 0; i < times; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                Instantiate(straightProjectile, firePoints[j].position, firePoints[j].rotation);
            }
            yield return new WaitForSeconds(timedelay);
        }
    }

    private IEnumerator FireOffAllFirePointsCoroutine(int times)
    {
        for (int i = 0; i < times; i++)
        {
            base.FireOffAllFirePoints();
            yield return new WaitForSeconds(timedelay);
        }
    }
}
