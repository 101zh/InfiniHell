using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusFireSpawner : BulletSpawner
{
    [SerializeField] private float chargeUpTime;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject chargeUpBall;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float warningTime;

    private Material whiteFlash;
    private Material originalMat;
    private SpriteRenderer chargeUpBallSR;
    private float maxScale;

    // Start is called before the first frame update
    void Start()
    {
        chargeUpBallSR = chargeUpBall.GetComponent<SpriteRenderer>();
        whiteFlash = Resources.Load<Material>("Rendering/Custom Materials/WhiteCircle");
        originalMat = chargeUpBallSR.material;

        ParticleSystem.MainModule mainModule = particles.main;
        mainModule.startSpeed = 0.25f * chargeUpTime - 1.86f;
        mainModule.startLifetime = Mathf.Abs(particles.shape.radius / particles.main.startSpeed.constant);
        maxScale = chargeUpBall.transform.localScale.x;

        StartCoroutine(FireLaser(5f));
    }

    /// <summary>
    /// Fires a laser
    /// </summary>
    /// <param name="seconds">the seconds the laser stays on screen</param>
    /// <returns>This Coroutine</returns>
    IEnumerator FireLaser(float seconds)
    {
        chargeUpBall.SetActive(true);
        particles.Play();

        Vector3 chargeUpBallScale = Vector3.zero;
        chargeUpBall.transform.localScale = chargeUpBallScale;
        float timeElapsedCharging = 0.0f;
        bool flashing = false;

        while (chargeUpBall.transform.localScale.x < maxScale)
        {
            if (!flashing && chargeUpTime - timeElapsedCharging <= warningTime)
            {
                flashing = true;
                StartCoroutine(Flash());
            }

            float scale = maxScale * (timeElapsedCharging / chargeUpTime);
            chargeUpBallScale.x = scale;
            chargeUpBallScale.y = scale;
            chargeUpBallScale.z = scale;

            chargeUpBall.transform.localScale = chargeUpBallScale;

            timeElapsedCharging += Time.deltaTime;
            yield return null;
        }

        particles.Stop();
        particles.Clear();
        laser.SetActive(true);
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator Flash()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i % 2 == 0)
            {
                chargeUpBallSR.material = whiteFlash;
            }
            else
            {
                chargeUpBallSR.material = originalMat;
            }
            yield return new WaitForSeconds(0.175f);
        }
        chargeUpBallSR.material = originalMat;
    }

    public void setChargeUpTime(float seconds)
    {
        if (seconds < 0) { seconds = 0.05f; }
        chargeUpTime = seconds;
    }

}
