using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveProjectile : Projectile
{
    [SerializeField] private bool isReflected = false;
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float period = (2 * Mathf.PI);

    private Vector2 originalPos;
    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        amplitude = isReflected ? -1f : 1f;
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.position = calc();
    }

    private Vector2 calc()
    {
        float x = speed * time;
        float b = (2 * Mathf.PI) / period;
        return originalPos + new Vector2(x / b, amplitude * Mathf.Sin(x));
    }
}
