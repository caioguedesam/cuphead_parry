using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Script used to control 2D movement in Unity

    // Input Variables
    private float horizontal_movement;
    private float vertical_movement;
    private bool jump_input;
    private bool jump_hold;
    private Vector2 move_direction;

    // Player References
    public Collider2D hitbox;
    [HideInInspector]
    public Rigidbody2D rb;
    private Collision coll;


    // Player Variables
    [Space]
    [Header("Variables")]
    public float move_speed = 600f;
    public float jump_speed = 400f;
    public float fall_multiplier = 4f;
    public float lowjump_multiplier = 4f;

    [Header("Booleans")]
    public bool can_parry = false;

    [Space]
    [Header("Polish")]
    public ParticleSystem parry_particle;


    private void Awake()
    {
        // Setting ground collision reference
        coll = GetComponent<Collision>();
        // Setting hitbox reference
        hitbox = GetComponent<Collider2D>();
        // Setting rigidbody reference
        rb = GetComponent<Rigidbody2D>();
        // Freezing rotation (so the capsule won't spin around)
        rb.freezeRotation = true;
    }

    // Input function
    private void GetInput()
    {
        // Computing all inputs
        horizontal_movement = Input.GetAxis("Horizontal");
        vertical_movement = Input.GetAxis("Vertical");
        jump_input = Input.GetButtonDown("Jump");
        jump_hold = Input.GetButton("Jump");
    }

    // Movement functions
    private void Walk(Vector2 move_direction)
    {
        // Adding velocity based on horizontal movement input
        rb.velocity = new Vector2(move_direction.x * move_speed * Time.deltaTime, rb.velocity.y);
    }

    private void Jump()
    {
        // Adding velocity based on jump speed
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.velocity += Vector2.up * jump_speed * Time.deltaTime;
    }

    private void Gravity()
    {
        // To ensure smoother jumping, adding gravity multipliers over regular fall time
        if (rb.velocity.y < 0)                      // if player is falling
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fall_multiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !jump_hold)   // or if player has released jump button
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowjump_multiplier - 1) * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checking if object collided with pink
        if (collision.gameObject.GetComponent("Pink"))
        {
            Pink particle_ref = collision.gameObject.GetComponent<Pink>();
            can_parry = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Checking if object has exited collision with pink
        if (collision.gameObject.GetComponent("Pink"))
        {
            can_parry = false;
        }
    }

    void Update()
    {
        GetInput();

        move_direction = new Vector2(horizontal_movement, vertical_movement);
        Walk(move_direction);

        if(jump_input && (coll.is_grounded || can_parry))
        {
            Jump();
            if (can_parry)
                parry_particle.Play();
        }
        Gravity();
    }

}
