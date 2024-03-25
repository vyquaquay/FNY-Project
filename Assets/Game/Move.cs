using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    //animation and physic
    private Rigidbody2D rbd2;
    private Animator Theanimator;
    public float speed = 2.0f;
    public float horizonMovement;
    // Start is called before the first frame update
    private void Start()
    {
        //player game object
        rbd2 = GetComponent<Rigidbody2D>();
        Theanimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    //to chance the directon
    private void Update()
    {
        horizonMovement = Input.GetAxisRaw("Horizontal");
    }
    //to make character run
    private void FixedUpdate()
    {
        rbd2.velocity = new Vector2(horizonMovement*speed,rbd2.velocity.y);
    }
}