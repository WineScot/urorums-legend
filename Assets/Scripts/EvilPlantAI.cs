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
    private FightControl fightControl;
    private Enemy enemyManager;
    // EvilPlant modes
    private string heroSide = "left";
    private bool attackMode = false;
    private string hitDirection = "right";
    private bool onAttack = false;
    // variables
    private float x_distacne = 0;
    private float y_distacne = 0;
    private Vector2 position;
    private float rhizomeAttackPoint = 3;
    private float horizontalHit = 10;
    private float verticallHit = 10;
    private int attackPosition = 0;
    public bool singleDamage = true;
    private string dieCommand = "total";

    // STANDARD METHODS
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        movingControl = player.GetComponent<MovingControl>();
        heroManager = player.GetComponent<HeroManager>();
        enemyManager = base.GetComponent<Enemy>();
        position = base.transform.position;
        fightControl = player.GetComponent<FightControl>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        GetHeroPosition();

        if (y_distacne < 30)
        {
            if (attackMode && x_distacne <= 14)
            {
                Attack();
            }
            else if (x_distacne < 30 && heroManager.HeroIsNoisy())  // plant is listening 
            {
                attackMode = true;
            }

        }
        if(!attackMode)
        {
            if (Input.GetKey(fightControl.normal_attack_key) || Input.GetKey(fightControl.strong_attack_key)) 
            {
                enemyManager.TakeDamage(ref rhizomeAttackPoint, ref dieCommand);
            }
        }
    }

    // METHODS
    private void GetHeroPosition()
    {
        x_distacne = movingControl.GetPosition().x - position.x;
        y_distacne = movingControl.GetPosition().y - position.y;
        if (x_distacne >= 0) heroSide = "right";
        else
        {
            x_distacne = -x_distacne;
            heroSide = "left";
        }
    }

    private bool HeroWithinAttackRange(int number)
    {
        if (hitDirection != heroSide) return false;
        switch (number)
        {
            case 1: { if (x_distacne < 7 && y_distacne < 11) return true; } break;
            case 2: { if (x_distacne < 8.4f && y_distacne < 10) return true; } break;
            case 3: { if (x_distacne < 11.3f && y_distacne < 9) return true; } break;
            case 4: { if (x_distacne < 13 && y_distacne < 4.9f) return true; } break;
            case 5: { if (x_distacne < 14 && y_distacne < 3.9f) return true; } break;
        }
        return false;
    }

    private void Attack()
    {
        if(attackPosition == 0)
        {
            if (heroSide == "right")
            {
                anim.SetTrigger("EvilPlantRightAttack");
                hitDirection = "right";
            }
            else
            {
                anim.SetTrigger("EvilPlantLeftAttack");
                hitDirection = "left";
            }
            StartCoroutine(MakeAttack(0.08f));
        }
        else
        {
            if (HeroWithinAttackRange(attackPosition) && singleDamage)
            {
                DealDamage();
                singleDamage = false;
            }
        }
    }

    private void DealDamage()
    {
        heroManager.TakeHealth(rhizomeAttackPoint);
        heroManager.TakeSpecialDamage("paralysys", 0.2f);
        movingControl.Move(ref horizontalHit, ref verticallHit, ref hitDirection);
    }

    // used for make action after time 
    // example call StartCoroutine(ExecuteActionAfterTime(time,action))
    IEnumerator MakeAttack(float time)
    {
        attackPosition = 1;
        yield return new WaitForSeconds(time);
        for (int i=2;i<6;i++)
        {
            yield return new WaitForSeconds(time);
            attackPosition = i;
            singleDamage = true;
        }
        attackPosition = 0;
    }
}