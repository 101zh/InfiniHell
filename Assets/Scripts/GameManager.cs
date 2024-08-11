using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float boundaryTolerance = 0.25f;
    private GameObject squareSpawner;
    [SerializeField] private float wallThickness = 1f;
    [SerializeField] private float spawnerSize = 1f;
    public static int level = -1;
    private static bool isSetDiff = false;
    private static float theSetDiff = 0.0f;

    public static readonly string defaultControls = "WASD";
    private readonly (float, float) bulletSpeeds = (1f, 4f);
    private readonly (float, float) spawnerSpeed = (10f, float.MaxValue);
    private readonly (int, int) numOfPatterns = (0, 4);
    private readonly (float, float) shotDelay = (0.15f, 0.75f);

    [SerializeField] private static float curDifficulty = 0.0f;

    private InGameMenuManager menuManager;
    private bool dead = false;
    static int timerSucessions = 0;
    static float timer = 0f;
    private float curMinBulletSpeed = 1f;
    private bool reachedMaxPatterns = false;
    private float curSpawnerSpeed;
    private int curMaxNumOfPatterns = 1;
    private float curPatternIncreaseThreshold = 0.3f;
    private static float screenWidth; // In Coordinate Values
    private static float screenHeight;// In Coordinate Values
    [SerializeField] int curNumOfActivePatterns = 0;

    public static GameManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        squareSpawner = (GameObject)Resources.Load("Prefabs/SquareSpawner");

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += SceneChangeEvent;
        OnSceneChange();
    }

    void SceneChangeEvent(Scene current, Scene next)
    {
        OnSceneChange();
    }

    void OnSceneChange()
    {
        StopAllCoroutines();
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {

        }
        else
        {
            curDifficulty = isSetDiff ? theSetDiff : 0.0f;
            menuManager = GameObject.Find("MenuManager").GetComponent<InGameMenuManager>();
            GameObject.Find("JoystickOutline").SetActive(PlayerPrefs.GetString("Controls", defaultControls).Equals("OnScreenJoystick"));
            updateBoundaries();
            curSpawnerSpeed = (spawnerSpeed.Item2 - spawnerSpeed.Item1) * curDifficulty + spawnerSpeed.Item1;
            if (level == 0)
            {
                GameObject tutorialText = (GameObject)Instantiate(Resources.Load("Prefabs/TutorialText"), GameObject.Find("Canvas").transform);
                string controlScheme = PlayerPrefs.GetString("Controls", defaultControls);
                tutorialText.GetComponentInChildren<TMP_Text>().text = "Use " + controlScheme;
                StartCoroutine(Tutorial());
            }
            else
            {
                StartCoroutine(InfiniteMode());
            }
        }
    }

    IEnumerator InfiniteMode()
    {
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > 15f)
            {
                timerSucessions++;
                timer = 0f;
                curDifficulty += 0.1f;
                curMinBulletSpeed = curDifficulty * (bulletSpeeds.Item2 - 2) > bulletSpeeds.Item2 - 2 ? bulletSpeeds.Item2 - 2 : curDifficulty * (bulletSpeeds.Item2 - 2);
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
            yield return null;
        }
    }

    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(8f);
        StartCoroutine(SingularPattern(2, 9f, 1f, 1f));
        yield return new WaitForSeconds(8f);
        StartCoroutine(QuadruplePattern(6, 9f, 0.75f, 1f));
        yield return new WaitForSeconds(14f);
        if (!dead)
            SceneManager.LoadScene("TitleScene");
    }

    IEnumerator TheNoPattern(float seconds)
    {
        curNumOfActivePatterns++;
        yield return new WaitForSeconds(seconds);
        curNumOfActivePatterns--;
    }

    void StartRandomQuadruplePattern()
    {
        int times = percentage(0.5f) ? Random.Range(3, 6) : Random.Range(10, 12);
        float timeDelay = (shotDelay.Item2 - ((shotDelay.Item2 - shotDelay.Item1) * curDifficulty)) < shotDelay.Item1 ? shotDelay.Item1 : (shotDelay.Item2 - ((shotDelay.Item2 - shotDelay.Item1) * curDifficulty));
        float bulletSpeed = Random.Range(curMinBulletSpeed, bulletSpeeds.Item2);

        StartCoroutine(QuadruplePattern(times, 9f, timeDelay, bulletSpeed));
    }

    IEnumerator QuadruplePattern(int times, float rotation, float timeDelay, float bulletSpeed)
    {
        curNumOfActivePatterns++;
        Vector2[] spawnPositions = generatePosInAll4Quandrants(generateRandomPositionOutsideBounds());
        GameObject[] spawnerInstances = new GameObject[4];

        for (int i = 0; i < spawnerInstances.Length; i++)
        {
            spawnerInstances[i] = Instantiate(squareSpawner, spawnPositions[i], squareSpawner.transform.rotation);
            spawnerInstances[i].GetComponent<SquareSpawner>().setSmoothTimeFactor(timeDelay);
            spawnerInstances[i].GetComponent<SquareSpawner>().setBulletSpeed(bulletSpeed);
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

        for (int i = 0; i < spawnerInstances.Length; i++)
        {
            spawnerInstances[i].GetComponent<SquareSpawner>().SpinFire(times, rotation);
        }
        yield return new WaitUntil(spawnerInstances[0].GetComponent<SquareSpawner>().DoneFiring);


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

    void StartRandomSingularPattern()
    {
        int times = Random.Range(0, 7);
        float rotation = Random.Range(2f, 45f);
        float timeDelay = Random.Range(shotDelay.Item1, shotDelay.Item2);
        float bulletSpeed = Random.Range(bulletSpeeds.Item1, curMinBulletSpeed);

        StartCoroutine(SingularPattern(times, rotation, timeDelay, bulletSpeed));
    }

    IEnumerator SingularPattern(int times, float rotation, float timeDelay, float bulletSpeed)
    {
        curNumOfActivePatterns++;
        GameObject spawnerInstance = Instantiate(squareSpawner, generateRandomPositionOutsideBounds(), squareSpawner.transform.rotation);
        spawnerInstance.GetComponent<SquareSpawner>().setSmoothTimeFactor(timeDelay);
        spawnerInstance.GetComponent<SquareSpawner>().setBulletSpeed(bulletSpeed);
        Vector2 destination = generateRandomPositionWithinBounds();
        Vector2 velocity = Vector2.zero;

        while (Vector2.Distance(destination, (Vector2)spawnerInstance.transform.position) > 0.005)
        {
            spawnerInstance.transform.position = Vector2.SmoothDamp(spawnerInstance.transform.position, destination, ref velocity, 0.5f, curSpawnerSpeed, Time.deltaTime);
            yield return null;
        }

        spawnerInstance.GetComponent<SquareSpawner>().SpinFire(times, rotation);
        yield return new WaitUntil(spawnerInstance.GetComponent<SquareSpawner>().DoneFiring);


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
                StartRandomQuadruplePattern(); break;
            case 1:
                StartRandomSingularPattern(); break;
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
        // [TopWall, RightWall, BottomWall, LeftWall]
        BoxCollider2D[] walls = GameObject.Find("Walls").transform.GetComponentsInChildren<BoxCollider2D>();

        Vector3 cameraSize = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        // Adds 1f to move walls a bit outside camera boundaries
        screenWidth = cameraSize.x;
        screenHeight = cameraSize.y;

        walls[0].size = new Vector2(screenWidth * 2f, wallThickness);
        walls[0].transform.GetChild(0).transform.localScale = new Vector2(screenWidth * 2f - boundaryTolerance - wallThickness, 0.02f);
        walls[0].transform.position = new Vector2(0, screenHeight + (0.5f * wallThickness) - boundaryTolerance);

        walls[1].size = new Vector2(wallThickness, screenHeight * 2f);
        walls[1].transform.GetChild(0).transform.localScale = new Vector2(0.02f, screenHeight * 2f - boundaryTolerance - wallThickness); ;
        walls[1].transform.position = new Vector2(screenWidth + (0.5f * wallThickness) - boundaryTolerance, 0);

        walls[2].size = new Vector2(screenWidth * 2f, wallThickness);
        walls[2].transform.GetChild(0).transform.localScale = new Vector2(screenWidth * 2f - boundaryTolerance - wallThickness, 0.02f);
        walls[2].transform.position = new Vector2(0, -screenHeight - (0.5f * wallThickness) + boundaryTolerance);

        walls[3].size = new Vector2(wallThickness, screenHeight * 2f);
        walls[3].transform.GetChild(0).transform.localScale = new Vector2(0.02f, screenHeight * 2f - boundaryTolerance - wallThickness);
        walls[3].transform.position = new Vector2(-screenWidth - (0.5f * wallThickness) + boundaryTolerance, 0);
    }

    private static float diffOnDeath;
    private static float timeOnDeath;

    public void onDeath()
    {
        dead = true;
        diffOnDeath = curDifficulty;
        timeOnDeath = timer + (timerSucessions * 15f);
        menuManager.activateDeathMenu();

    }

    public static float getTime()
    {
        return timer + (timerSucessions * 15f);
    }

    public static float getCurDifficulty()
    {
        return curDifficulty;
    }

    public static float getTimeSurvived()
    {
        return timeOnDeath;
    }

    public static float getDifficultyOnDeath()
    {
        return diffOnDeath;
    }

    public static void setDiff(float diff)
    {
        isSetDiff = true;
        theSetDiff = diff;
    }

    public static void clearDiff()
    {
        isSetDiff = false;
        theSetDiff = 0.0f;
    }
}
