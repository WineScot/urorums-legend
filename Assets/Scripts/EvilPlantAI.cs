using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EvilPlantAI : MonoBehaviour
{
// ATTRIBUTES
    // plant components
    private Rigidbody2D rb2d;
    private Animator anim;
    private GameObject player;
    // variables
    public bool attackMode = false;
    public float x_distacne = 0;
    public float y_distacne = 0;
    private int attackSingleRhizome = 4;
    private bool onAttack = false;
    private string direction = "left";
    private float horizontalHit = 30;
    private float verticallHit = 60;
    private string hitDirection = "right";

// STANDARD METHODS
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 enemyPosition = GetComponent<Rigidbody2D>().position;
        x_distacne = (playerPosition.x - enemyPosition.x) >= 0 ? (playerPosition.x - enemyPosition.x) : -(playerPosition.x - enemyPosition.x);
        y_distacne = (playerPosition.y - enemyPosition.y) >= 0 ? (playerPosition.y - enemyPosition.y) : -(playerPosition.y - enemyPosition.y);
        
        if (y_distacne < 30)
        {
            if (attackMode && x_distacne <= 18 && !onAttack && y_distacne < 14)
            {
                onAttack = true;
                if (playerPosition.x > enemyPosition.x)
                {
                    anim.SetTrigger("EvilPlantRightAttack");
                    direction = "right";
                    Invoke("IntermediateAttack", 0.2f);
                    Invoke("Attack", 0.4f);
                }
                else
                {
                    anim.SetTrigger("EvilPlantLeftAttack");
                    direction = "left";
                    Invoke("IntermediateAttack", 0.2f);
                    Invoke("Attack", 0.4f);
                }
            }
            else if (x_distacne < 30 && player.GetComponent<HeroManager>().heroIsNoisy)  // plant is listening 
            {
                attackMode = true;
            }
            
        }
    }

// METHODS


    public void IntermediateAttack()
    {
        if (x_distacne < 8 && y_distacne < 14)
        {
            player.gameObject.GetComponent<HeroManager>().TakeHealth(2 * attackSingleRhizome);
            if (direction == "left")
            {
                hitDirection = "left";
            }
            else
            {
                hitDirection = "right";
            }
            player.gameObject.GetComponent<MovingControl>().Move(ref horizontalHit, ref verticallHit, ref hitDirection);
        }
        
    }
    public void Attack()
    {
        if (x_distacne > 18 || y_distacne > 14)
        {
            onAttack = false;
            return;
        }
        else if (x_distacne >= 13)
        {
            player.gameObject.GetComponent<HeroManager>().TakeHealth(attackSingleRhizome);
        }
        else if (x_distacne >= 9)
        {
            player.gameObject.GetComponent<HeroManager>().TakeHealth(2 * attackSingleRhizome);
        }
        else
        {
            player.gameObject.GetComponent<HeroManager>().TakeHealth(3 * attackSingleRhizome);
        }
        if (direction == "left")
        {
            hitDirection = "left";
        }
        else
        {
            hitDirection = "right";
        }
        player.gameObject.GetComponent<MovingControl>().Move(ref horizontalHit, ref verticallHit, ref hitDirection);
        onAttack = false;
    }

}