using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSpawner : BulletSpawner
{
    //Transform[] firePoints [Top, Right, Down, Left]

    IEnumerator SpinFire(int times, float rotationOffset, float bulletAngle = 0)
    {
        for (int j = 0; j < times; j++)
        {
            GameObject[] bullets = FireOffAllFirePoints();
            foreach (GameObject bullet in bullets)
            {
                bullet.transform.Rotate(0, 0, bulletAngle);
            }

            yield return new WaitForSeconds(timedelay);
            transform.Rotate(0, 0, rotationOffset);
        }
    }

}
