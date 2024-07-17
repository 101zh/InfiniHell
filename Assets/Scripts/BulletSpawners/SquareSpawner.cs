using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSpawner : BulletSpawner
{
    //Transform[] firePoints [Top, Right, Down, Left]

    /// <summary>
    /// Initiates bullet firing pattern. 
    /// </summary>
    /// <param name="times"></param>
    /// <returns>The seconds for firing pattern to end</returns>
    public float SpinFire(int times, float rotationAngle, float bulletAngle = 0)
    {
        StartCoroutine(SpinFireCoroutine(times, rotationAngle, bulletAngle));
        return timedelay * times;
    }

    private IEnumerator SpinFireCoroutine(int times, float rotationAngle, float bulletAngle)
    {
        for (int j = 0; j < times; j++)
        {
            GameObject[] bullets = FireOffAllFirePoints();
            foreach (GameObject bullet in bullets)
            {
                bullet.transform.Rotate(0, 0, bulletAngle);
            }

            yield return new WaitForSeconds(timedelay);
            transform.Rotate(0, 0, rotationAngle);
        }
    }

}
