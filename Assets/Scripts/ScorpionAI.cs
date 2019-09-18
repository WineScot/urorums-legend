using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScorpionAI : MonoBehaviour {
// ATTRIBUTES
    // scorpion components
    private Rigidbody2D rb2d;
    private Animator anim;
    private GameObject hero;
    private Enemy enemyManager;
    private HeroManager heroManager;
    private MovingControl movingControl;
    // special mode
    public bool duringAttackMode = false;
    public bool followHeroMode = false;
    private string direction = "right";
    //variables
    private float x_viewRange = 50;
    private float y_viewRange = 50;
    private float x_attackRange = 7.3f;
    private float y_attackRange = 5.6f;
    private float moveSpeed = 15;
    private float zero = 0;
    private float attackPoint = 1f;
    private float tailAttackPoint = 10f;
    private float horizontalHit = 30;
    private float verticallHit = 60;
    // scorpion params
    private float x_velocity = 0;
    private int warningAttackNr = 0;
    private float y_velocity = 0;
    public Vector2 scorpionPosition;

    // STANDARD METHODS

    // Used for initialization
    void Start()
    {
        rb2d = base.GetComponent<Rigidbody2D>();
        x_velocity = rb2d.velocity.x;
        y_velocity = rb2d.velocity.y;
        anim = base.GetComponent<Animator>();
        
        hero = GameObject.FindGameObjectWithTag("Player");
        enemyManager = base.GetComponent<Enemy>();
        heroManager = hero.GetComponent<HeroManager>();
        movingControl = hero.GetComponent<MovingControl>();

        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // set scorpion position
        scorpionPosition = rb2d.position;
        // attack if hero is close else go to hero
        if (HeroWithinRange("attack"))
        {
            anim.SetTrigger("EnemyStanding");
            Attack();
        }
        else
        {
            // detect if hero stand on scorpion
            if(HeroWithinRange("hero on scorpion")) 
            {
                anim.SetTrigger("EnemyStanding");
                Invoke("UpperAttack", 0.5f);
            }
            warningAttackNr = 0;
            // follow hero if enemy see him
            if (HeroWithinRange("follow"))
            {
                enemyManager.SetOrderInLayer(-Mathf.RoundToInt((DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).x * 10)));
                FollowHero(ref heroManager.GetHeroPosition(), ref scorpionPosition);
            }
            else
            {
                followHeroMode = false;
                anim.SetTrigger("EnemyStanding");
            }
        }
        // set crust
        SetCrust();
    }

// METHODS

    // gets 2xobject Vector2 positon and return Vector2(x distance, y dystance)
    // example call DistanceVector(first object position, second object position)
    private Vector2 DistanceVector(ref Vector2 heroPosition, ref Vector2 enemyPosition)
    {
        float x_distance = (heroPosition.x - enemyPosition.x) > 0 ? (heroPosition.x - enemyPosition.x) : -(heroPosition.x - enemyPosition.x);
        float y_distance = (heroPosition.y - enemyPosition.y) > 0 ? (heroPosition.y - enemyPosition.y) : -(heroPosition.y - enemyPosition.y);
        return new Vector2(x_distance, y_distance);
    }

    

    // return true if hero is within checked range
    // example call HeroWithinRange("range name")
    private bool HeroWithinRange(string rangeName)
    {
        if (rangeName == "follow")
        {
            if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).x < 0.9f) return false;
            if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).y > y_viewRange) return false;
            if (followHeroMode)
            {
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).x < 2 * x_viewRange) return true;
            }
            else
            {
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).x < x_viewRange) return true;
            }
            return false;
        }
        else if (rangeName == "attack")
        {
            if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).y < y_attackRange)
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).x < x_attackRange) return true;
            return false;
        }
        else if (rangeName == "hero on scorpion")
        {
            if(heroManager.GetHeroPosition().y > scorpionPosition.y)
            {
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).y < 7.7f)
                    if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).x < 6) return true;
            }
            return false;
        }
        else return false;
    }

    // set attributes to follow hero
    // example call FollowHero(hero position,scorpion position)
    private void FollowHero(ref Vector2 heroPosition, ref Vector2 scorpionPosition)
    {
        followHeroMode = true;
        if(heroPosition.x > scorpionPosition.x)
        {
            direction = "right";
            GetComponent<BoxCollider2D>().offset.Set(1.1f, -0.65f);
            Move(ref moveSpeed, ref y_velocity, ref direction);
            anim.SetTrigger("EnemyRightWalk");
        }
        else
        {
            direction = "left";
            GetComponent<BoxCollider2D>().offset.Set(-1.1f, -0.65f);
            Move(ref moveSpeed, ref y_velocity, ref direction);
            anim.SetTrigger("EnemyLeftWalk");
        }
    }

    // get horizontal and vertical speed and direction and move enemy
    // example call Move(horizontal speed,vertical speed, direction)
    public void Move(ref float x_moveSpeed, ref float y_moveSpeed, ref string direction)
    {
        Vector2 movement;
        if (direction == "right") movement = new Vector2(x_moveSpeed, y_moveSpeed);
        else movement = new Vector2(-x_moveSpeed, y_moveSpeed);
        rb2d.velocity = movement;
    }

    // set attack
    // example call Attack()
    public void Attack()
    {
        if(!duringAttackMode)
        {
            duringAttackMode = true;
            if (warningAttackNr < 2)
            {
                if(direction == "right") anim.SetTrigger("ScorpionRightAttack");
                else anim.SetTrigger("ScorpionLeftAttack");
                StartCoroutine(ExecuteActionAfterTime(0.05f, () => { ClawAttack(); }));
                StartCoroutine(ExecuteActionAfterTime(0.6f, () => { duringAttackMode = false; }));
            }
            else
            {
                if (direction == "right") anim.SetTrigger("ScorpionRightTailAttack");
                else anim.SetTrigger("ScorpionLeftTailAttack");
                StartCoroutine(ExecuteActionAfterTime(0.6f, () => { TailAttack(); }));
                warningAttackNr = 0;
                StartCoroutine(ExecuteActionAfterTime(1.4f, () => { duringAttackMode = false; }));
            }
        }
    }

    // upper attack
    // example call UpperAttack()
    private void UpperAttack()
    {
        if(HeroWithinRange("hero on scorpion") && !duringAttackMode)
        {
            duringAttackMode = true;
            if (direction == "right") anim.SetTrigger("ScorpionRightUpperAttack");
            else anim.SetTrigger("ScorpionLeftUpperAttack");
            heroManager.TakeHealth(3 * attackPoint);
            heroManager.TakeSpecialDamage("paralysys", 0.6f);
            movingControl.Move(ref horizontalHit, ref verticallHit, ref direction);
            StartCoroutine(ExecuteActionAfterTime(1.4f, () => { duringAttackMode = false; }));
        }    
    }

    // tail attack
    // example call TailAttack()
    private void TailAttack()
    {
        if(HeroWithinRange("attack"))
        {
            heroManager.TakeHealth(tailAttackPoint);
            heroManager.TakeSpecialDamage("paralysys", 0.3f);
            movingControl.Move(ref horizontalHit, ref verticallHit, ref direction);
        }
    }

    // claw attack
    // example call ClawAttack()
    private void ClawAttack()
    {
        if (HeroWithinRange("attack"))
        {
            heroManager.TakeHealth(attackPoint);
            warningAttackNr++;
        }
    }

    // set crust depends on direction
    // example call SetCrust()
    private void SetCrust()
    {
        if (direction == "right")
        {
            enemyManager.GetCrust() = new Vector4(10,0,5,25);
        }
        else
        {
            enemyManager.GetCrust() = new Vector4(5, 0, 10, 25);
        }
    }

    // used for make action after time 
    // example call StartCoroutine(ExecuteActionAfterTime(time,action))
    IEnumerator ExecuteActionAfterTime(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}