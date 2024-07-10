using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    Rigidbody2D rb2D;
    [SerializeField] int movementSpeed;
    private InfiniHellInput inputActions;

    private InputAction move;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        inputActions = new InfiniHellInput();
    }

    private void OnEnable()
    {
        move =  inputActions.Player.Move;
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
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(move.ReadValue<Vector2>());
        rb2D.velocity = move.ReadValue<Vector2>().normalized * movementSpeed * Time.deltaTime;
    }
}
