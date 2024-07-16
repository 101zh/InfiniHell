using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] protected Transform[] firePoints;
    [SerializeField] protected float timedelay = 0.75f;

    protected static GameObject straightProjectile;
    protected static GameObject waveProjectile;
    void Awake()
    {
        if (straightProjectile == null)
            straightProjectile = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Straight Projectile.prefab", typeof(GameObject));
        if (waveProjectile == null)
            waveProjectile = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Wave Projectile.prefab", typeof(GameObject));
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
}