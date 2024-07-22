using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2.5f;
    [SerializeField] private ParticleSystem deathParticles;
    private Rigidbody2D rb2D;
    private CircleCollider2D circleCollider2D;
    private SpriteRenderer spriteRenderer;
    private InfiniHellInput inputActions;

    private bool usingMouse;
    private Vector2 direction;
    private InputAction move;
    private bool isDead = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        inputActions = new InfiniHellInput();
    }

    private void OnEnable()
    {
        move = determineControls();
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!usingMouse)
            direction = move.ReadValue<Vector2>();
        else
        {
            direction = Vector2.zero;
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                if (Vector2.Distance(mousePos, (Vector2)transform.position) > 0.05)
                    direction = mousePos - (Vector2)transform.position;
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead)
        {
            rb2D.velocity = direction.normalized * movementSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Projectile"))
        {
            die();
            GameObject.Find("GameManager").GetComponent<GameManager>().onDeath();
        }
    }

    private void die()
    {
        isDead = true;
        spriteRenderer.enabled = false;

        rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
        circleCollider2D.enabled = false;

        deathParticles.Play();
    }


    public InputAction determineControls()
    {
        string controls = PlayerPrefs.GetString("Controls", GameManager.defaultControls);

        Debug.Log(controls);
        switch (controls)
        {
            case "WASD":
                return inputActions.Player.MoveWASD;
            case "Arrows":
                return inputActions.Player.MoveArrows;
            case "LeftStick":
                return inputActions.Player.MoveLeftStick;
            case "Mouse":
                usingMouse = true;
                return inputActions.Player.MoveMouse;
            case "OnScreenJoystick":
                return inputActions.Player.MoveLeftStick;
        }

        return null;
    }
}
