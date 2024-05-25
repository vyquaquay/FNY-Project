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
    private int dbJumpCounter;
    [SerializeField] private int maxdbJump = 1;
    [Header("Groundcheck setting")]
    [SerializeField] private Transform groundcheck;
    [SerializeField] private float groundchecky = 0.2f;
    [SerializeField] private float groundcheckx = 0.5f;
    [SerializeField] private LayerMask isground;
    Animator animator;
    PlayerStateList playerStateList;

    [Header("Coyotetime setting")]
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime = 0.1f;

    [Header("Dash setting")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCD;
    private bool canDash = true;
    private bool dashed;
    public static Move Instance;
    private float gravity;
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
        playerStateList = GetComponent<PlayerStateList>();
        rbd2 = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
   
        gravity = rbd2.gravityScale;
    }

    // Update is called once per frame

    private void Update()
    {
        getInput();
        UpdateJump();
        if (playerStateList.dashing) return;
        Flip();
        Moving();
        Jump();
        StartDash();
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
    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }
        if (isonGround())
        {
            dashed = false;
        }
    }
    IEnumerator Dash()
    {
        canDash = false;
        playerStateList.dashing = true;
        animator.SetTrigger("Dashing");
        rbd2.gravityScale = 0;
        rbd2.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        yield return new WaitForSeconds(dashTime);
        rbd2.gravityScale = gravity;
        playerStateList.dashing = false;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
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
       
            if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0)
            {
                rbd2.velocity = new Vector3(rbd2.velocity.x, jumpForce);
               /* playerStateList.Jumping = true;*/
            }
            else if (!isonGround() && dbJumpCounter < maxdbJump && Input.GetButtonDown("Jump"))
                     {
                        dbJumpCounter++;
                         rbd2.velocity = new Vector3(rbd2.velocity.x, jumpForce);
                     }

        animator.SetBool("Jumping", !isonGround());
    }
    void UpdateJump()
    {
        if(isonGround())
        {
           /* playerStateList.Jumping = false;*/
            coyoteTimeCounter = coyoteTime;
            dbJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
}
