using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayerSpawner : BulletSpawner
{
    //Transform[] firePoints [Right, MidRight, Mid, MidLeft, Left]

    public IEnumerator MidFire(int times)
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

    public IEnumerator FireOffAllFirePoints(int times)
    {
        for (int i = 0; i < times; i++)
        {
            base.FireOffAllFirePoints();
            yield return new WaitForSeconds(timedelay);
        }
    }
}
