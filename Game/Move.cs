using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    //animation and physic
    [Header("physic of player")]
    private Rigidbody2D rbd2;
    [SerializeField] public float speed = 2.0f;
    private float xAxis;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private Transform groundcheck;
    [SerializeField] private float groundchecky = 0.2f;
    [SerializeField] private float groundcheckx = 0.5f;
    [SerializeField] private LayerMask isground;
    Animator animator;

    public static Move Instance;

    private void Awake()
    {
         if( Instance != null &&  Instance != this)
        {
            Destroy(gameObject);
        }
         else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        //player game object
        rbd2 = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    private void Update()
    {
        getInput();
        Moving();
        Jump();
        Flip();
    }

    void getInput()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
    }

    private void Moving()
    {
        rbd2.velocity = new Vector2 (speed * xAxis, rbd2.velocity.y );
        animator.SetBool("Walking", rbd2.velocity.x != 0 && isonGround());
    } 
    public bool isonGround()
    {
        if (Physics2D.Raycast(groundcheck.position, Vector2.down, groundchecky, isground) 
            || Physics2D.Raycast(groundcheck.position + new Vector3(groundcheckx, 0, 0), Vector2.down, groundchecky, isground)
            || Physics2D.Raycast(groundcheck.position + new Vector3(-groundcheckx, 0, 0), Vector2.down, groundchecky, isground)
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Jump()
    {
        if(Input.GetButtonDown("Jump") && isonGround())
        {
            rbd2.velocity = new Vector3(rbd2.velocity.x, jumpForce);
        }
        animator.SetBool("Jumping", !isonGround());
    }
}
