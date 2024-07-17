using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2.5f;
    [SerializeField] private ParticleSystem deathParticles;
    private Rigidbody2D rb2D;
    private CircleCollider2D circleCollider2D;
    private SpriteRenderer spriteRenderer;
    private InfiniHellInput inputActions;

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
        move = inputActions.Player.Move;
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
        direction = move.ReadValue<Vector2>();
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
            GameObject.Find("GameManager").GetComponent<GameManager>().activateDeathMenu();
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
}
