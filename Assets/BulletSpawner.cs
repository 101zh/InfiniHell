using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] Transform firePoint;

    private GameObject straightProjectile;
    private GameObject waveProjectile;
    // Start is called before the first frame update
    void Start()
    {
        straightProjectile = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Straight Projectile.prefab", typeof(GameObject));
        waveProjectile = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Wave Projectile.prefab", typeof(GameObject));
        StartCoroutine(Spin(30, 0, 0));
    }

    IEnumerator Spin(float angleBetweenBullets, float offset, float bulletAngle)
    {
        for (int j = 0; j < 4; j++)
        {
            for (float i = 0f; i < 360f; i += angleBetweenBullets)
            {
                GameObject bullet = Instantiate(straightProjectile, firePoint.position, firePoint.rotation);
                Debug.Log(bullet.transform.rotation);
                bullet.transform.Rotate(0, 0, bulletAngle);
                Debug.Log(bullet.transform.rotation);
                transform.Rotate(0, 0, angleBetweenBullets);
            }

            yield return new WaitForSeconds(0.75f);
            transform.Rotate(0, 0, offset);
        }
    }
}