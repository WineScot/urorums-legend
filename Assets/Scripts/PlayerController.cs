using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/*
 * ----------PLAYER CONTROLER----------
 * controls basic player moves like:
 * walking, running, jumping, dashs
 * and controls action direction:
 * up, down, right, left
 */

/*
 * ------------IMPORTANT!-----------
 * Solution with caps lock is from 
 * this page: https://answers.unity.com/questions/164795/unable-to-trace-capslock-on-and-off.html
 * author:  quicktom
 * author page: https://answers.unity.com/users/33200/u3d-91312121.html
 */
public class PlayerController : MonoBehaviour
{
    // COMPONENT 
    private Rigidbody2D rb2d;
    private Animator anim;
    // MOVE CONTROL BUTTONS
    public string up_key = "up";
    public string down_key = "down";
    public string left_key = "left";
    public string right_key = "right";
    public string jump_key = "space";
    public string dash_key = "x";
    // SPECIAL MODE
    private bool canMove = true;
    private bool upMode = false;
    private bool downMode = false;
    private bool canJump = true;
    public string direction = "right";
    // VARIABLES 
    private float zero = 0;
    private float walkSpeed = 10;
    private float runSpeed = 50;
    private float dashSpeed = 200;
    private float jumpHeight = 110;
    private float dashBlockTime = 1;
    private float dashLongTime = 0.2f;
    private int loudSoundNumber = 0;
    public bool discreetMoving = true;
    private bool heroInAir = false;

    private bool dash = false;
    private bool canDash = true;
    // CAPS LOCK STATE
    [DllImport("user32.dll")]
    public static extern short GetKeyState(int keyCode);
    int isCapsLockOn;

    // START
    void Start()
    {
       anim = GetComponent<Animator>();
       rb2d = GetComponent<Rigidbody2D>();
       isCapsLockOn = (((ushort)GetKeyState(0x14)) & 0xffff);
    }

    void DashEnd()
    {
        dash = false;
        canDash = false;
        Invoke("UnlockDash", dashBlockTime);
    }

    void LoudSound()
    {
        discreetMoving = false;
        loudSoundNumber++;
        Invoke("LoudSoundStop", 0.1f);
    }

    void LoudSoundStop()
    {
        loudSoundNumber--;
        if(loudSoundNumber <= 0)
        {
            discreetMoving = true;
            loudSoundNumber = 0;
        }
        
    }

    void UnlockDash()
    {
        canDash = true;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), isCapsLockOn.ToString());
    }


    void FixedUpdate()
    {
        if(!canJump)
        {
            heroInAir = true;
        }
        else
        {
            if (heroInAir) LoudSound();
            heroInAir = false;
        }
        if (canMove)
        {
            // RUNNING or WALKING
            isCapsLockOn = (((ushort)GetKeyState(0x14)) & 0xffff);
            if(Input.GetKey("left shift"))
            {
                isCapsLockOn = isCapsLockOn > 0 ? 0 : 1;
            }
            // MOVING
            if (Input.GetKey(right_key)) // RIGHT 
            {
                direction = "right";
                if (dash) Walk(ref dashSpeed);
                else
                {
                    if(isCapsLockOn > 0) Walk(ref runSpeed);
                    else Walk(ref walkSpeed);
                }
                anim.SetTrigger("playerRightWalk");

            }
            else if (Input.GetKey(left_key)) // LEFT
            {
                direction = "left";

                if (dash) Walk(ref dashSpeed);
                else
                {
                    if (isCapsLockOn > 0) Walk(ref runSpeed);
                    else Walk(ref walkSpeed);
                }

                anim.SetTrigger("playerLeftWalk");
            }
			else //STOPPING
            {
                Walk(ref zero);
                anim.SetTrigger("playerIdle");
                if (dash) { DashEnd(); }

            }
            // LOOKING/FIGHTING - UP
			if (Input.GetKey(up_key)) upMode = true;
			else upMode = false;			
            // LOOKING/FIGHTING/CROUCHNIG - DOWN
			if (Input.GetKey(down_key)) downMode = true;
			else downMode = false;
            // DASH
            if (Input.GetKey(dash_key) && canDash) 
            {
                dash = true;
                Invoke("DashEnd", dashLongTime);
            }
            // JUMPING
            if (Input.GetKey(jump_key) && canJump)
            {
                Vector2 movement = new Vector2(rb2d.velocity.x, jumpHeight);
                rb2d.velocity = movement;
                LoudSound();
                heroInAir = true;
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
        if (canJump)
        {
            if (moveSpeed > walkSpeed) discreetMoving = false;
            else discreetMoving = true;
        }
        if (direction == "right")
        {
            Vector2 movement = new Vector2(moveSpeed, rb2d.velocity.y);
            rb2d.velocity = movement;
            
        }
        else
        {
            Vector2 movement = new Vector2(-moveSpeed, rb2d.velocity.y);
            rb2d.velocity = movement;
        }
    }

}