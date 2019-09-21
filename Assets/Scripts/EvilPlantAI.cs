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
    private MovingControl movingControl;
    private HeroManager heroManager;
    // EvilPlant modes
    private string direction = "left";
    private bool attackMode = false;
    private string hitDirection = "right";
    private bool onAttack = false;
    // variables
    private float x_distacne = 0;
    private float y_distacne = 0;
    private Vector2 position;
    private int attackSingleRhizome = 4;
    private float horizontalHit = 30;
    private float verticallHit = 60;

    // STANDARD METHODS
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        movingControl = player.GetComponent<MovingControl>();
        heroManager = player.GetComponent<HeroManager>();
        position = base.transform.position;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        SetHeroPosition();

        if (y_distacne < 30)
        {
            if (attackMode && x_distacne <= 18 && !onAttack && y_distacne < 14)
            {
                //Attack();
                onAttack = true;
                if (direction == "right")
                {
                    anim.SetTrigger("EvilPlantRightAttack");
                    //Invoke("IntermediateAttack", 0.2f);
                    //Invoke("Attack", 0.4f);
                }
                else
                {
                    anim.SetTrigger("EvilPlantLeftAttack");
                    //Invoke("IntermediateAttack", 0.2f);
                    //Invoke("Attack", 0.4f);
                }
            }
            else if (x_distacne < 30 && heroManager.HeroIsNoisy())  // plant is listening 
            {
                attackMode = true;
            }

        }
    }

    // METHODS
    private void SetHeroPosition()
    {
        x_distacne = movingControl.GetPosition().x - position.x;
        y_distacne = movingControl.GetPosition().y - position.y;
        if (x_distacne >= 0) direction = "right";
        else
        {
            x_distacne = -x_distacne;
            direction = "left";
        }
        y_distacne = y_distacne > 0 ? y_distacne : -y_distacne;
    }

    private bool HeroWithinAttackRange(int number)
    {
        switch (number)
        {
            case 1: { if (x_distacne < 3 && y_distacne < 20) return true; } break;
            case 2: { if (x_distacne < 5 && y_distacne < 18) return true; } break;
            case 3: { if (x_distacne < 7 && y_distacne < 16) return true; } break;
            case 4: { if (x_distacne < 9 && y_distacne < 14) return true; } break;
            case 5: { if (x_distacne < 13 && y_distacne < 12) return true; } break;
        }
        return false;
    }

    private void Attack()
    {

    }

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
    public void Attackkk()
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

    // used for make action after time 
    // example call StartCoroutine(ExecuteActionAfterTime(time,action))
    IEnumerator ExecuteActionAfterTime(float time)
    {
        for(int i=1;i<6;i++)
        {
            yield return new WaitForSeconds(time);
            if(HeroWithinAttackRange(i))
            {

            }
        }
    }
}