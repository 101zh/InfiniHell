using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // [TopWall, RightWall, BottomWall, LeftWall]
    [SerializeField] private BoxCollider2D[] walls;
    [SerializeField] private Camera mainCam;
    [SerializeField] private float wallThickness = 1f;

    private static float screenWidth; // In Coordinate Values
    private static float screenHeight;// In Coordinate Values

    // Start is called before the first frame update
    void Start()
    {
        Vector3 cameraSize = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        // Adds 1f to move walls a bit outside camera boundaries
        screenWidth = cameraSize.x;
        screenHeight = cameraSize.y;
    }

    // Update is called once per frame
    void Update()
    {
        walls[0].size = new Vector2(screenWidth * 2f, wallThickness);
        walls[0].transform.position = new Vector2(0, screenHeight + (0.5f * wallThickness));

        walls[1].size = new Vector2(wallThickness, screenHeight * 2f);
        walls[1].transform.position = new Vector2(screenWidth + (0.5f * wallThickness), 0);

        walls[2].size = new Vector2(screenWidth * 2f, wallThickness);
        walls[2].transform.position = new Vector2(0, -screenHeight - (0.5f * wallThickness));

        walls[3].size = new Vector2(wallThickness, screenHeight * 2f);
        walls[3].transform.position = new Vector2(-screenWidth - (0.5f * wallThickness), 0);
    }

    public static bool isOutOfBounds(Vector2 position)
    {
        if (position.x < -screenWidth || position.x > screenWidth || position.y > screenHeight || position.y < -screenHeight)
            return false;

        return true;
    }
}
