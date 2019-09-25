using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RatAI : MonoBehaviour
{
    // ATTRIBUTES
    // rat components
    private Rigidbody2D rb2d;
    private Animator anim;
    private GameObject hero;
    private Enemy enemyManager;
    private Collider2D colider;
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
    private float attackPoint = 5f;
    private float horizontalHit = 30;
    private float verticallHit = 60;
    // rat params
    private float x_velocity = 0;
    private float y_velocity = 0;
    public Vector2 ratPosition;

    // STANDARD METHODS

    // Used for initialization
    void Start()
    {
        rb2d = base.GetComponent<Rigidbody2D>();
        anim = base.GetComponent<Animator>();
        colider = base.GetComponent<Collider2D>();

        hero = GameObject.FindGameObjectWithTag("Player");
        enemyManager = base.GetComponent<Enemy>();
        heroManager = hero.GetComponent<HeroManager>();
        movingControl = hero.GetComponent<MovingControl>();


    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // set rat position
        ratPosition = rb2d.position;
        x_velocity = rb2d.velocity.x;
        y_velocity = rb2d.velocity.y;
        // attack if hero is close else go to hero
        if (HeroWithinRange("attack"))
        {
            anim.SetTrigger("EnemyStanding");
            Attack();
        }
        else
        {
            // detect if hero stand on rat
            if (HeroWithinRange("hero on rat"))
            {
                anim.SetTrigger("EnemyStanding");
                Invoke("UpperAttack", 2f);
            }
            // follow hero if rat see him
            if (HeroWithinRange("follow"))
            {
                enemyManager.SetOrderInLayer(-Mathf.RoundToInt((DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).x * 10)));
                FollowHero(ref heroManager.GetHeroPosition(), ref ratPosition);
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
            if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).x < 0.9f) return false;
            if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).y > y_viewRange) return false;
            if (followHeroMode)
            {
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).x < 2 * x_viewRange) return true;
            }
            else
            {
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).x < x_viewRange) return true;
            }
            return false;
        }
        else if (rangeName == "attack")
        {
            if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).y < y_attackRange)
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).x < x_attackRange) return true;
            return false;
        }
        else if (rangeName == "hero on rat")
        {
            if (heroManager.GetHeroPosition().y > ratPosition.y)
            {
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).y < 7.7f)
                    if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).x < 6) return true;
            }
            return false;
        }
        else return false;
    }

    // set attributes to follow hero
    // example call FollowHero(hero position,rat position)
    private void FollowHero(ref Vector2 heroPosition, ref Vector2 ratPosition)
    {
        followHeroMode = true;
        if (heroPosition.x > ratPosition.x)
        {
            direction = "right";
            Move(ref moveSpeed, ref y_velocity, ref direction);
            anim.SetTrigger("EnemyRightWalk");
            //colider.offset.Set(1.1f, -0.65f);
            //colider.isTrigger = true;
            colider.offset = new Vector2(1.1f, -0.65f);
        }
        else
        {
            direction = "left";
            Move(ref moveSpeed, ref y_velocity, ref direction);
            anim.SetTrigger("EnemyLeftWalk");
            //colider.offset.Set(-1.1f, -0.65f);
            //colider.isTrigger = false;
            colider.offset = new Vector2(-1.1f, -0.65f);
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
        if (!duringAttackMode)
        { 
            if (direction == "right") anim.SetTrigger("RatRightAttack");
            else anim.SetTrigger("RatLeftAttack");
            StartCoroutine(ExecuteActionAfterTime(0.05f, () => { FrontAttack(); }));
            StartCoroutine(ExecuteActionAfterTime(0.6f, () => { duringAttackMode = false; }));
        }
    }

    // upper attack
    // example call UpperAttack()
    private void UpperAttack()
    {
        if (HeroWithinRange("hero on rat") && !duringAttackMode)
        {
            /*drop the hero*/
        }
    }

    // front attack
    // example call FrontAttack()
    private void FrontAttack()
    {
        if (HeroWithinRange("attack"))
        {
            /*jump into hero direction and deal damage*/
            heroManager.TakeHealth(attackPoint);
        }
    }

    // set crust depends on direction
    // example call SetCrust()
    private void SetCrust()
    {
        if (direction == "right")
        {
            enemyManager.GetCrust() = new Vector4(0, 0, 0, 0);
        }
        else
        {
            enemyManager.GetCrust() = new Vector4(0, 0, 0, 0);
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
