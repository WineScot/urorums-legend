using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ----------PLAYER CONTROLER----------
 * controls basic player moves like:
 * walking, running, jumping, flips
 * and controls action direction:
 * up, down, right, left
 */
public class PlayerController : MonoBehaviour
{
    // COMPONENT 
    private Rigidbody2D rb2d;
    private Animator anim;
    Event e;
    // MOVE CONTROL BUTTONS
    public string up_key = "up";
    public string down_key = "down";
    public string left_key = "left";
    public string right_key = "right";
    public string jump_key = "space";
    // SPECIAL MODE
    private bool canMove = true;
    private bool upMode = false;
    private bool downMode = false;
    private bool canJump = true;
    public string direction = "right";
    // VARIABLES 
    private float walkSpeed = 50;
    private float runSpeed = 90;
    private float flipSpeed = 200;
    private float jumpHeight = 110;

    public bool left_go = false;
    public bool right_go = false;

    // START
    void Start()
    {
       anim = GetComponent<Animator>();
       rb2d = GetComponent<Rigidbody2D>();
       left_go = false;
       right_go = false;
    }

    private void FlipPrivent()
    {
        left_go = false;
        right_go = false;
}

    void FixedUpdate()
    {
        if (canMove)
        {
            // JUMPING
            if (Input.GetKey(jump_key) && canJump)
			{
				Vector2 movement = new Vector2(rb2d.velocity.x, jumpHeight);
				rb2d.velocity = movement;
			}

            // WALKING
            e = Event.current;
            if (Input.GetKey(right_key)) // RIGHT 
            {
                direction = "right";
                if (e.capsLock)
                {
                    Walk(ref runSpeed);
                }
                else
                {
                    Walk(ref walkSpeed);
                }
            }
            else if (Input.GetKey(left_key)) // LEFT
            {
                direction = "left";
                if (e.capsLock)
                {
                    Walk(ref runSpeed);
                }
                else
                {
                    Walk(ref walkSpeed);
                }
            }
			else //STOPPING
            {
                anim.SetTrigger("playerIdle");
				Vector2 movement = new Vector2(0, rb2d.velocity.y);
				rb2d.velocity = movement;
            }
			
            // LOOKING/FIGHTING - UP
			if (Input.GetKey(up_key)) upMode = true;
			else upMode = false;
			
            // LOOKING/FIGHTING/CROUCHNIG - DOWN
			if (Input.GetKey(down_key)) downMode = true;
			else downMode = false;

            // ROGGHT FLIP
            if (Input.GetKeyDown(right_key))
            {
                if (right_go)
                {
                    Vector2 movement = new Vector2(200, rb2d.velocity.y);
                    rb2d.velocity = movement;
                    anim.SetTrigger("playerLeftWalk");
                    canMove = false;
                    Invoke("SetCantMove", 0.1f);
                }
                else
                {
                    right_go = true;
                }
                Invoke("FlipPrivent", 0.2f);
            }

            // LEFT FLIP
            if (Input.GetKeyDown(left_key))
            {
                if (left_go)
                {
                    Vector2 movement = new Vector2(-200, rb2d.velocity.y);
                    rb2d.velocity = movement;
                    anim.SetTrigger("playerRightWalk");
                    canMove = false;
                    Invoke("SetCantMove", 0.1f);
                }
                else
                {
                    left_go = true;
                }
                Invoke("FlipPrivent", 0.2f);
            }
        }
    }


    // #CAN JUMP ?
    void OnTriggerExit2D(Collider2D other)
    {
        canJump = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        canJump = true;
    }
    // CAN JUMP ?#

    public string GetDirection()
    {
        return direction;
    }
    public string DirectionMode()
    {
        if (upMode) return "up";
        else if (downMode) return "down";
        else return "normal";
    }

    public void WhileStrongAttack(float moveS, float jumpH)
    {
        canMove = false;
        Vector2 movement = new Vector2(moveS, jumpH);
        rb2d.velocity = movement;
        Invoke("SetCantMove", 0.5f);
    }

    public void SetCantMove()
    {
        canMove = true;
    }

    private void Walk(ref float moveSpeed)
    {
        if (direction == "right")
        {
            Vector2 movement = new Vector2(moveSpeed, rb2d.velocity.y);
            rb2d.velocity = movement;
            anim.SetTrigger("playerRightWalk");
        }
        else
        {
            Vector2 movement = new Vector2(-moveSpeed, rb2d.velocity.y);
            rb2d.velocity = movement;
            anim.SetTrigger("playerLeftWalk");
        }
    }

}