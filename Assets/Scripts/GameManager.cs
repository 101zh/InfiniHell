using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // [TopWall, RightWall, BottomWall, LeftWall]
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject a;
    [SerializeField] private BoxCollider2D[] walls;
    [SerializeField] private Camera mainCam;
    [SerializeField] private float wallThickness = 1f;
    [SerializeField] private float spawnerSize = 1f;

    private static float screenWidth; // In Coordinate Values
    private static float screenHeight;// In Coordinate Values

    // Start is called before the first frame update
    void Start()
    {
        updateBoundaries();
        Debug.Log(screenHeight);
        Debug.Log(screenWidth);
        StartCoroutine(IndefiniteQuadruplePattern());
    }

    bool isIndefiniteRunning = false;

    IEnumerator IndefiniteQuadruplePattern()
    {
        isIndefiniteRunning = true;
        Vector2[] spawnPositions = generatePosInAll4Quandrants(generateRandomPositionOutsideBounds());
        GameObject[] spawnerInstances = new GameObject[4];
        for (int i = 0; i < spawnerInstances.Length; i++)
        {
            spawnerInstances[i] = Instantiate(spawner, spawnPositions[i], spawner.transform.rotation);
        }

        while (isIndefiniteRunning)
        {
            Vector2[] destinations = generatePosInAll4Quandrants(generateRandomPositionWithinBounds());
            Vector2[] velocities = new Vector2[4];
            for (int i = 0; i < spawnerInstances.Length; i++) velocities[i] = Vector2.zero;

            while (Vector2.Distance(destinations[0], (Vector2)spawnerInstances[0].transform.position) > 0.0005)
            {
                for (int i = 0; i < spawnerInstances.Length; i++)
                    spawnerInstances[i].transform.position = Vector2.SmoothDamp(spawnerInstances[i].transform.position, destinations[i], ref velocities[i], 0.5f, 10f, Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(0.25f);
            float waitTime = 0.0f;
            for (int i = 0; i < spawnerInstances.Length; i++)
            {
                waitTime = spawnerInstances[i].GetComponent<SquareSpawner>().SpinFire(3, 10);
            }
            yield return new WaitForSeconds(waitTime);
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator QuadruplePattern()
    {
        Vector2[] spawnPositions = generatePosInAll4Quandrants(generateRandomPositionOutsideBounds());
        GameObject[] spawnerInstances = new GameObject[4];
        for (int i = 0; i < spawnerInstances.Length; i++)
        {
            spawnerInstances[i] = Instantiate(spawner, spawnPositions[i], spawner.transform.rotation);
        }

        Vector2[] destinations = generatePosInAll4Quandrants(generateRandomPositionWithinBounds());
        Vector2[] velocities = new Vector2[4];
        for (int i = 0; i < spawnerInstances.Length; i++) velocities[i] = Vector2.zero;

        while (Vector2.Distance(destinations[0], (Vector2)spawnerInstances[0].transform.position) > 0.0005)
        {
            for (int i = 0; i < spawnerInstances.Length; i++)
                spawnerInstances[i].transform.position = Vector2.SmoothDamp(spawnerInstances[i].transform.position, destinations[i], ref velocities[i], 0.5f, 10f, Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);
        float waitTime = 0.0f;
        for (int i = 0; i < spawnerInstances.Length; i++)
        {
            waitTime = spawnerInstances[i].GetComponent<SquareSpawner>().SpinFire(3, 10);
        }
        yield return new WaitForSeconds(waitTime);
        yield return new WaitForSeconds(0.25f);

        destinations = generatePosInAll4Quandrants(generateRandomPositionOutsideBounds());
        for (int i = 0; i < spawnerInstances.Length; i++) velocities[i] = Vector2.zero;

        while (Vector2.Distance(destinations[0], (Vector2)spawnerInstances[0].transform.position) > 0.0005)
        {
            for (int i = 0; i < spawnerInstances.Length; i++)
                spawnerInstances[i].transform.position = Vector2.SmoothDamp(spawnerInstances[i].transform.position, destinations[i], ref velocities[i], 0.5f, 10f, Time.deltaTime);
            yield return null;
        }

        foreach (GameObject spawner in spawnerInstances)
        {
            Destroy(spawner);
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

    IEnumerator SingularPattern()
    {
        GameObject spawnerInstance = Instantiate(spawner, generateRandomPositionOutsideBounds(), spawner.transform.rotation);
        Vector2 destination = generateRandomPositionWithinBounds();
        Vector2 velocity = Vector2.zero;

        while (Vector2.Distance(destination, (Vector2)spawnerInstance.transform.position) > 0.0005)
        {
            spawnerInstance.transform.position = Vector2.SmoothDamp(spawnerInstance.transform.position, destination, ref velocity, 0.5f, 10f, Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);
        yield return new WaitForSeconds(spawnerInstance.GetComponent<SquareSpawner>().SpinFire(3, 10));
        yield return new WaitForSeconds(0.25f);

        destination = generateRandomPositionOutsideBounds();
        velocity = Vector2.zero;
        while (Vector2.Distance(destination, (Vector2)spawnerInstance.transform.position) > 0.0005)
        {
            spawnerInstance.transform.position = Vector2.SmoothDamp(spawnerInstance.transform.position, destination, ref velocity, 0.5f, 10f, Time.deltaTime);
            yield return null;
        }
        Destroy(spawnerInstance);
    }

    private Vector2 generateRandomPositionWithinBounds()
    {
        float randX = Random.Range(-screenWidth + spawnerSize, screenWidth - spawnerSize);
        float randY = Random.Range(-screenHeight + spawnerSize, screenHeight - spawnerSize);

        return new Vector2(randX, randY);
    }

    private Vector2 generateRandomPositionOutsideBounds()
    {
        bool leftRight = fiftyPercent();

        float xMin = leftRight ? screenWidth + wallThickness : 0;
        float yMin = leftRight ? 0 : screenHeight + wallThickness;

        int negativeOrPositiveX = fiftyPercent() ? -1 : 1;
        int negativeOrPositiveY = fiftyPercent() ? -1 : 1;
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

    /// <summary>Returns true 50% of the time</summary>
    public bool fiftyPercent()
    {
        return Random.Range(0, 2) == 0;
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
}
