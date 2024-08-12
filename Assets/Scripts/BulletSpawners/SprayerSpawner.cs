using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayerSpawner : BulletSpawner
{
    //Transform[] firePoints [Right, MidRight, Mid, MidLeft, Left]

    public sealed override void Fire(int times, float rotationAngle, bool middleFire = false)
    {
        if (middleFire) { StartCoroutine(MiddleFireCoroutine(times)); }
        else { StartCoroutine(FireOffAllFirePointsCoroutine(times)); }
    }

    private IEnumerator MiddleFireCoroutine(int times)
    {
        isDoneFiring = false;
        for (int i = 0; i < times; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                Instantiate(straightProjectile, firePoints[j].position, firePoints[j].rotation);
            }
            yield return new WaitForSeconds(smoothTimeFactor);
        }
        isDoneFiring = true;
    }

    private IEnumerator FireOffAllFirePointsCoroutine(int times)
    {
        isDoneFiring = false;
        for (int i = 0; i < times; i++)
        {
            base.FireOffAllFirePoints();
            yield return new WaitForSeconds(smoothTimeFactor);
        }
        isDoneFiring = true;
    }
}
