using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FightControl : MonoBehaviour
{
// ATTRIBUTES
    // attack contol buttons
    public string normal_attack_key = "v";
    public string strong_attack_key = "c";
    public string defence_key = "z";
    // attack information for enemies
    private Vector4 attackArea = new Vector4(0,0,0,0);
    private string attackDirection = "right";
    public bool attackAreaIsActive = false;
    private string specialEffect = "no";
    // hero components
    private GameObject hand;
    private Animator anim;
    private MovingControl movingControl;
    // variables
    private float normalAttackPoints = 20;
    private float strongAttackPoints = 30;
    private float attackPoints = 0;
    private bool isCoroutineExecuting = false;
    private float heroPositionX = 0;
    private float heroPositionY = 0;
    

// STANDART METHODS

    // Used for initialization
    void Start()
    {
        movingControl = base.GetComponent<MovingControl>();
        hand = GameObject.FindGameObjectWithTag("Hand");
        anim = hand.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // get hero position
        heroPositionX = base.transform.position.x;
        heroPositionY = base.transform.position.y;
        // hero can not attack during defence or previous attack
        if (!attackAreaIsActive)  
        {
            if (Input.GetKey(defence_key))
            {
                
            }
            else if (Input.GetKey(normal_attack_key))
            {
                Attack("normal");
            }
            else if (Input.GetKey(strong_attack_key))
            {
                Attack("strong");
            }
        }
    }

// METHODS

    // Get attack kind and set attak variables for enemy
    // example call Attack("attack kind")
    private void Attack(string attackKind)
    {
        float special_x = 0, special_y = 0, special_z = 0, special_w = 0, special_attackPoints = 0;
        attackAreaIsActive = true;
        attackDirection = movingControl.DirectionMode();
        if (false/*weapondManager.AnySpecialEffects()*/) // set special effects 
        {
             /* -----------------
              * set special_(...)
              * set specialEffect
              ------------------ */
        }
        if(attackKind == "normal")
        {
            attackPoints = normalAttackPoints + special_attackPoints;
            StartCoroutine(ExecuteActionAfterTime(0.1f, () => { attackAreaIsActive = false; }));
            switch (attackDirection)
            {
                case "up": SetAttackArea(special_x + 1.5f, special_y + 0, special_z + 1.5f, special_w + 8.8f); break;
                case "down": SetAttackArea(special_x + 1.5f, special_y + 10, special_z + 1.5f, special_w + 0); break;
                case "right": SetAttackArea(special_x + 0, special_y + 3, special_z + 7, special_w + 3); break;
                case "left": SetAttackArea(special_x + 7, special_y + 3, special_z + 0, special_w + 3); break;
            }
        }
        else if(attackKind == "strong")
        {
            attackPoints = strongAttackPoints + special_attackPoints;
            StartCoroutine(ExecuteActionAfterTime(0.15f, () => { attackAreaIsActive = false; }));
            switch (attackDirection)
            {
                case "up": SetAttackArea(special_x + 4, special_y + 2, special_z + 4, special_w + 10.8f); break;
                case "down": SetAttackArea(special_x + 4, special_y + 13, special_z + 4, special_w + 3); break;
                case "right": SetAttackArea(special_x + 0, special_y + 6.3f, special_z + 9, special_w + 6.3f); break;
                case "left": SetAttackArea(special_x + 9, special_y + 6.3f, special_z + 0, special_w + 6.3f); break;
            }
        }
    }

    // Set area where hero make attack
    // example call SetAttackArea(left width, down height, right width, upper height)
    private void SetAttackArea(float x = 0, float y = 0, float z = 0, float w = 0)
    {           
        attackArea.x = heroPositionX - x;
        attackArea.y = heroPositionY - y;
        attackArea.z = heroPositionX + z;
        attackArea.w = heroPositionY + w;
    }

    // Return true if hero is attacking
    // example call HeroIsAttacking()
    public bool HeroIsAttacking()
    {
        if (attackAreaIsActive) return true;
        else return false;
    }

    // Return special effect name
    // example call GetSpecialEffect()
    public ref string GetSpecialEffect()
    {
        return ref specialEffect;
    }

    // Return area where hero make attack
    // example call AttackArea()
    public ref Vector4 AttackArea()
    {
        return ref attackArea;
    }

    // Return information about attack direction (up,down,left,right)
    // example call AttackDirection()
    public ref string AttackDirection()
    {
        return ref attackDirection;
    }

    // Return attack point
    // example call GetAttackPoint()
    public ref float GetAttackPoint()
    {
        return ref attackPoints;
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
