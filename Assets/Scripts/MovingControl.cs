using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;



/*
 * ----------PLAYER CONTROLER----------
 * controls basic player moves like:
 * walking, running, jumping, dashs
 * and controls hero modes and direction like:
 * up, down, right, left, canMove 
 */

/*
 * ------------IMPORTANT!-----------
 * Solution with caps lock is from 
 * this page: https://answers.unity.com/questions/164795/unable-to-trace-capslock-on-and-off.html
 * author:  quicktom
 * author page: https://answers.unity.com/users/33200/u3d-91312121.html
 */

public class MovingControl : MonoBehaviour
{
    
// ATTRIBUTES
    // component 
    private Rigidbody2D rb2d;
    private Animator anim;
    // move control buttons
    public string up_key = "up";
    public string down_key = "down";
    public string left_key = "left";
    public string right_key = "right";
    public string jump_key = "space";
    public string dash_key = "x";
    public string save_key = "s";
    // special mode
    private bool canMove = true;
    private bool canDash = true;
    private bool upMode = false;
    private bool downMode = false;
    private bool canJump = true;
    private bool heroOnGround = true;
    private bool dashMode = false;
    private bool runMode = false;
    private string direction = "right";
    // variables 
    private float zero = 0;
    private float jumpHighStart = 0;
    private float walkSpeed = 9;
    private float runSpeed = 45;
    private float dashSpeed = 350;
    private float currentVerticalSpeed = 0;
    private float currentHorizontalSpeed = 0;
    private float jumpHeight = 70;
    private float dashBlockTime = 1;
    private float dashLongTime = 0.02f;
    private int loudSoundNumber = 0;
    private bool loudMoving = false;
    private Vector2 position;
    
    private bool isCoroutineExecuting = false;
    private int isCapsLockOn;

/*---------Caps lock detection---------*/
    [DllImport("user32.dll")]
    public static extern short GetKeyState(int keyCode);
    // READ "IMPORTANT!"!
 /*------------------------------------*/

// STANDARD METHODS

    // Used for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        // detect caps lock state
        runMode = (((ushort)GetKeyState(0x14)) & 0xffff)>0 ? true:false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        position = base.transform.position;
        // can hero move?
        if (canMove)
        {
            currentVerticalSpeed = rb2d.velocity.y;
            currentHorizontalSpeed = rb2d.velocity.x > 0 ? rb2d.velocity.x : -rb2d.velocity.x;

            // running or walking
            runMode = (((ushort)GetKeyState(0x14)) & 0xffff) > 0 ? true : false;
            if (Input.GetKey("left shift")) runMode = !runMode;
            // moving
            if (Input.GetKey(right_key)) // right 
            {
                direction = "right";
                MakeMove();
                anim.SetTrigger("playerRightWalk");
            }
            else if (Input.GetKey(left_key)) // left
            {
                direction = "left";
                MakeMove();
                anim.SetTrigger("playerLeftWalk");
            }
            else //stopping
            {
                Move(ref zero, ref currentVerticalSpeed, ref direction);
                anim.SetTrigger("playerIdle");
            }
            // active up mode
            if (Input.GetKey(up_key)) upMode = true;
            else upMode = false;
            // active down mode
            if (Input.GetKey(down_key)) downMode = true;
            else downMode = false;
            // active dash mode
            if (Input.GetKey(dash_key)/* && canDash*/) Dash();
            // jump
            if (Input.GetKey(jump_key) && canJump)
            {
                if(heroOnGround)
                {
                    jumpHighStart = base.transform.position.y;
                    MakeSound();
                }   
                else if (currentVerticalSpeed <= 0) canJump = false;
                Move(ref currentHorizontalSpeed, ref jumpHeight, ref direction);
                if (base.transform.position.y - jumpHighStart > 20) canJump = false;
            }
            else if(!heroOnGround)
            {
                canJump = false;
            }
            // save mechanism
            if (Input.GetKey(save_key))
            {
                GetComponent<HeroManager>().SaveHero(); // save
            }
        }
    }

//METHODS

    // move hero vertical and horizontal
    // example call Move(horizontal speed value,verical speed value,move direction (left or right))
    public void Move(ref float x_moveSpeed,ref float y_moveSpeed,ref string direction)
    {
        Vector2 movement;
        if (direction == "right") movement = new Vector2(x_moveSpeed, y_moveSpeed);
        else movement = new Vector2(-x_moveSpeed, y_moveSpeed);
        rb2d.velocity = movement;
        if (canJump)
        {
            if (x_moveSpeed > walkSpeed) loudMoving = true;
            else loudMoving = false;
        }
        if(x_moveSpeed == 0 && dashMode) dashMode = false;
    }

    // decide about moving speed
    // example call MakeMove()
    private void MakeMove()
    {
        if (dashMode) Move(ref dashSpeed, ref currentVerticalSpeed, ref direction);
        else if (runMode) Move(ref runSpeed, ref currentVerticalSpeed, ref direction);
        else Move(ref walkSpeed, ref currentVerticalSpeed, ref direction);

    }

    // return currentVerticalSpeed
    // example call CurrentVerticalSpeed()
    public ref float CurrentVerticalSpeed()
    {
        return ref currentVerticalSpeed;
    }

    // return currentVerticalSpeed
    // example call CurrentVerticalSpeed()
    public ref float CurrentHorizontalSpeed()
    {
        return ref currentHorizontalSpeed;
    }

    // return current hero position reference
    // example call GetPosition()
    public ref Vector2 GetPosition()
    {
        return ref position;
    }
	
    // activates dash mode for dashLongTime time
    // example call Dash()
    private void Dash()
    {
        dashMode = true;
        canDash = false;
        StartCoroutine(ExecuteActionAfterTime(dashLongTime,() => { dashMode = false; }));
        StartCoroutine(ExecuteActionAfterTime(dashBlockTime, () => { canDash = true; }));
    }

    // paralyse hero for some time
    // example call Paralyse(time)
    private void Paralyse(float time)
    {
        canMove = false;
        Move(ref zero, ref zero, ref direction);
        StartCoroutine(ExecuteActionAfterTime(0.5f, () => { canMove = true; }));
    }

    // activates loud moving
    // example call MakeSound()
    void MakeSound()
    {
        loudMoving = true;
        loudSoundNumber++;
        StartCoroutine(ExecuteActionAfterTime(0.1f, () => 
        {
            loudSoundNumber--;
            if (loudSoundNumber <= 0)
            {
                loudMoving = false;
                loudSoundNumber = 0;
            }
        }));
    }

    // return true if hero is moving loudly (loudMoving is true)
    // example call HeroIsMovingLoudly()
    public bool HeroIsMovingLoudly()
    {
        if (loudMoving) return true;
        else return false;
    }

    // action when hero touch the floor
    void OnTriggerEnter2D(Collider2D other)
    {
        MakeSound();
    }

    // action when hero break away from the floor
    void OnTriggerExit2D(Collider2D other)
    {
        heroOnGround = false;
    }

    // action when hero is touching the floor
    void OnTriggerStay2D(Collider2D other)
    {
        heroOnGround = true;
        canJump = true;
    }

    // return reference to direction
    // example call GetDirection()
    public ref string GetDirection()
    {
        return ref direction;
    }

    // return canMove variable
    // example call GetCanMove()
    public ref bool GetCanMove()
    {
        return ref canMove;
    }

    // return hero direction mode
    // example call DirectionMode()
    public string DirectionMode()
    {
        if (upMode) return "up";
        else if (downMode) return "down";
        else return GetDirection();
    }

    // used for make action after time 
    // example call StartCoroutine(ExecuteActionAfterTime(time,action))
    IEnumerator ExecuteActionAfterTime(float time, Action action)
    {
        if (isCoroutineExecuting) yield break;
        isCoroutineExecuting = true;
        yield return new WaitForSeconds(time);
        action();
        isCoroutineExecuting = false;
    }
}
