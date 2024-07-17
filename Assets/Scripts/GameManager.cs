using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject spawner;
    [SerializeField] private BoxCollider2D[] walls; // [TopWall, RightWall, BottomWall, LeftWall]
    [SerializeField] private Camera mainCam;
    [SerializeField] private float wallThickness = 1f;
    [SerializeField] private float spawnerSize = 1f;
    [SerializeField] private GameObject deathMenu;

    private readonly (float, float) bulletSpeed = (1f, 4f);
    private readonly (float, float) spawnerSpeed = (10f, float.MaxValue);
    private readonly (int, int) numOfPatterns = (0, 4);
    private readonly (float, float) shotDelay = (0.15f, 0.75f);

    [SerializeField] private float curDifficulty = 0.0f;

    [SerializeField] float timer = 0f;
    private float curMinBulletSpeed = 1f;
    private bool reachedMaxPatterns = false;
    private float curSpawnerSpeed;
    private int curMaxNumOfPatterns = 1;
    private float curPatternIncreaseThreshold = 0.3f;
    private static float screenWidth; // In Coordinate Values
    private static float screenHeight;// In Coordinate Values
    [SerializeField] int curNumOfActivePatterns = 0;

    // Start is called before the first frame update
    void Start()
    {
        updateBoundaries();
        curSpawnerSpeed = (spawnerSpeed.Item2 - spawnerSpeed.Item1) * curDifficulty + spawnerSpeed.Item1;
    }


    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 15f)
        {
            timer = 0f;
            curDifficulty += 0.1f;
            curMinBulletSpeed = curDifficulty * (bulletSpeed.Item2 - 2) > bulletSpeed.Item2 - 2 ? bulletSpeed.Item2 - 2 : curDifficulty * (bulletSpeed.Item2 - 2);
            curSpawnerSpeed = (spawnerSpeed.Item2 - spawnerSpeed.Item1) * curDifficulty + spawnerSpeed.Item1;
            if (!reachedMaxPatterns && curDifficulty > curPatternIncreaseThreshold)
            {
                curPatternIncreaseThreshold += 0.3f;
                curMaxNumOfPatterns += 1;
                reachedMaxPatterns = curMaxNumOfPatterns == numOfPatterns.Item2;
            }
        }


        if (curNumOfActivePatterns < curMaxNumOfPatterns)
        {
            if (percentage(0.5f))
            {
                selectRandomPattern();
            }
            else
            {
                StartCoroutine(TheNoPattern(5f));
            }
        }

    }

    IEnumerator TheNoPattern(float seconds)
    {
        curNumOfActivePatterns++;
        yield return new WaitForSeconds(seconds);
        curNumOfActivePatterns--;
    }

    IEnumerator QuadruplePattern()
    {
        curNumOfActivePatterns++;
        Vector2[] spawnPositions = generatePosInAll4Quandrants(generateRandomPositionOutsideBounds());
        GameObject[] spawnerInstances = new GameObject[4];

        float timeDelay = (shotDelay.Item2 - ((shotDelay.Item2 - shotDelay.Item1) * curDifficulty)) < shotDelay.Item1 ? shotDelay.Item1 : (shotDelay.Item2 - ((shotDelay.Item2 - shotDelay.Item1) * curDifficulty));
        int times = percentage(0.5f) ? Random.Range(3, 6) : Random.Range(10, 12);

        for (int i = 0; i < spawnerInstances.Length; i++)
        {
            spawnerInstances[i] = Instantiate(spawner, spawnPositions[i], spawner.transform.rotation);
            spawnerInstances[i].GetComponent<SquareSpawner>().setTimeDelay(timeDelay);
            spawnerInstances[i].GetComponent<SquareSpawner>().setBulletSpeed(Random.Range(curMinBulletSpeed, bulletSpeed.Item2));
        }

        Vector2[] destinations = generatePosInAll4Quandrants(generateRandomPositionWithinBounds());
        Vector2[] velocities = new Vector2[4];
        for (int i = 0; i < spawnerInstances.Length; i++) velocities[i] = Vector2.zero;

        while (Vector2.Distance(destinations[0], (Vector2)spawnerInstances[0].transform.position) > 0.005)
        {
            for (int i = 0; i < spawnerInstances.Length; i++)
                spawnerInstances[i].transform.position = Vector2.SmoothDamp(spawnerInstances[i].transform.position, destinations[i], ref velocities[i], 0.5f, curSpawnerSpeed, Time.deltaTime);
            yield return null;
        }


        float waitTime = 0.0f;
        for (int i = 0; i < spawnerInstances.Length; i++)
        {
            waitTime = spawnerInstances[i].GetComponent<SquareSpawner>().SpinFire(times, 9f);
        }
        yield return new WaitForSeconds(waitTime);


        destinations = generatePosInAll4Quandrants(generateRandomPositionOutsideBounds());
        for (int i = 0; i < spawnerInstances.Length; i++) velocities[i] = Vector2.zero;

        while (Vector2.Distance(destinations[0], (Vector2)spawnerInstances[0].transform.position) > 0.005)
        {
            for (int i = 0; i < spawnerInstances.Length; i++)
                spawnerInstances[i].transform.position = Vector2.SmoothDamp(spawnerInstances[i].transform.position, destinations[i], ref velocities[i], 0.5f, curSpawnerSpeed, Time.deltaTime);
            yield return null;
        }

        foreach (GameObject spawner in spawnerInstances)
        {
            Destroy(spawner);
        }
        curNumOfActivePatterns--;
    }

    IEnumerator SingularPattern()
    {
        curNumOfActivePatterns++;
        GameObject spawnerInstance = Instantiate(spawner, generateRandomPositionOutsideBounds(), spawner.transform.rotation);
        int times = Random.Range(0, 7);
        float rotation = Random.Range(2f, 45f);
        spawnerInstance.GetComponent<SquareSpawner>().setTimeDelay(Random.Range(shotDelay.Item1, shotDelay.Item2));
        spawnerInstance.GetComponent<SquareSpawner>().setBulletSpeed(Random.Range(bulletSpeed.Item1, curMinBulletSpeed));
        Vector2 destination = generateRandomPositionWithinBounds();
        Vector2 velocity = Vector2.zero;

        while (Vector2.Distance(destination, (Vector2)spawnerInstance.transform.position) > 0.005)
        {
            spawnerInstance.transform.position = Vector2.SmoothDamp(spawnerInstance.transform.position, destination, ref velocity, 0.5f, curSpawnerSpeed, Time.deltaTime);
            yield return null;
        }


        yield return new WaitForSeconds(spawnerInstance.GetComponent<SquareSpawner>().SpinFire(times, rotation));


        destination = generateRandomPositionOutsideBounds();
        velocity = Vector2.zero;
        while (Vector2.Distance(destination, (Vector2)spawnerInstance.transform.position) > 0.005)
        {
            spawnerInstance.transform.position = Vector2.SmoothDamp(spawnerInstance.transform.position, destination, ref velocity, 0.5f, curSpawnerSpeed, Time.deltaTime);
            yield return null;
        }
        Destroy(spawnerInstance);
        curNumOfActivePatterns--;
    }

    private void selectRandomPattern()
    {
        int rand = Random.Range(0, 2);
        switch (rand)
        {
            case 0:
                StartCoroutine(QuadruplePattern()); break;
            case 1:
                StartCoroutine(SingularPattern()); break;
        }
    }
    Vector2[] generatePosInAll4Quandrants(Vector2 point)
    {
        Vector2[] points = new Vector2[4];
        points[0] = point;
        points[1] = new Vector2(-point.x, point.y);
        points[2] = new Vector2(point.x, -point.y);
        points[3] = new Vector2(-point.x, -point.y);

        return points;
    }

    private Vector2 generateRandomPositionWithinBounds()
    {
        float randX = Random.Range(-screenWidth + spawnerSize, screenWidth - spawnerSize);
        float randY = Random.Range(-screenHeight + spawnerSize, screenHeight - spawnerSize);

        return new Vector2(randX, randY);
    }

    private Vector2 generateRandomPositionOutsideBounds()
    {
        bool leftRight = percentage(0.5f);

        float xMin = leftRight ? screenWidth + wallThickness : 0;
        float yMin = leftRight ? 0 : screenHeight + wallThickness;

        int negativeOrPositiveX = percentage(0.5f) ? -1 : 1;
        int negativeOrPositiveY = percentage(0.5f) ? -1 : 1;
        float randX = Random.Range(xMin, screenWidth + 5f) * negativeOrPositiveX;
        float randY = Random.Range(yMin, screenHeight + 5f) * negativeOrPositiveY;

        return new Vector2(randX, randY);
    }

    public static bool isOutOfBounds(Vector2 position)
    {
        if ((position.x < -screenWidth || position.x > screenWidth) || (position.y > screenHeight || position.y < -screenHeight))
            return true;

        return false;
    }

    /// <summary>
    /// Returns true <paramref name="percentage"/> of the time
    /// </summary>
    /// <param name="percentage"></param>
    /// <returns>true or false</returns>
    public bool percentage(float percentage)
    {
        return Random.Range(0.0f, 1.0f) < percentage;
    }

    public void updateBoundaries()
    {
        Vector3 cameraSize = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        // Adds 1f to move walls a bit outside camera boundaries
        screenWidth = cameraSize.x;
        screenHeight = cameraSize.y;

        walls[0].size = new Vector2(screenWidth * 2f, wallThickness);
        walls[0].transform.position = new Vector2(0, screenHeight + (0.5f * wallThickness));

        walls[1].size = new Vector2(wallThickness, screenHeight * 2f);
        walls[1].transform.position = new Vector2(screenWidth + (0.5f * wallThickness), 0);

        walls[2].size = new Vector2(screenWidth * 2f, wallThickness);
        walls[2].transform.position = new Vector2(0, -screenHeight - (0.5f * wallThickness));

        walls[3].size = new Vector2(wallThickness, screenHeight * 2f);
        walls[3].transform.position = new Vector2(-screenWidth - (0.5f * wallThickness), 0);
    }

    public void activateDeathMenu()
    {
        deathMenu.SetActive(true);
    }
}
