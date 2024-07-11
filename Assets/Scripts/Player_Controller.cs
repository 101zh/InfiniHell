using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] int movementSpeed = 1250;
    [SerializeField] private ParticleSystem deathParticles;
    private Rigidbody2D rb2D;
    private CircleCollider2D circleCollider2D;
    private SpriteRenderer spriteRenderer;
    private InfiniHellInput inputActions;

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

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            rb2D.velocity = move.ReadValue<Vector2>().normalized * movementSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Projectile"))
        {
            StartCoroutine(die());
        }
    }

    private IEnumerator die()
    {
        isDead = true;
        spriteRenderer.enabled = false;

        rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
        circleCollider2D.enabled = false;

        deathParticles.Play();
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
