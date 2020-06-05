using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2;
    public float jumpHeight = 4;
    public float gravity = -9.81f;
    public Transform groundDetector;
    public LayerMask groundMask;
    public bool grounded;
    private Animator anim;
    private Vector2 velocity;
    private Rigidbody2D collision;
    private float jumpCooldown = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        collision = GetComponent<Rigidbody2D>();
        velocity = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(groundDetector.position, 0.05f, groundMask);
        anim.SetBool("Grounded", grounded);
        float movement = Input.GetAxis("Horizontal");
        float look = Input.GetAxis("Vertical");
        //Movement animation logic
        Vector3 scale = transform.localScale;
        if (movement > 0)
        {
            scale.x = 1;
        }
        else if (movement < 0)
        {
            scale.x = -1;
        }
        transform.localScale = scale;
        anim.SetBool("Moving", movement != 0);
        //Jump logic
        if (jumpCooldown > 0){
            jumpCooldown -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && grounded && jumpCooldown <= 0)
        {
            anim.SetTrigger("Jump");
            grounded = false;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCooldown = 0.5f;
        }
        //Left Click
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack1");
        }
        //right Click
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("Attack2");
        }

        
        //Apply Gravity
        if (grounded && jumpCooldown <= 0)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        //Apply Velocity
        velocity.x = speed * movement;
        collision.velocity = velocity;

    }
}
