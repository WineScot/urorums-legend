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
    private float moveSpeed = 15;
    private float runSpeed = 30;
    private float jumpSpeed = 20;
    private float zero = 0;
    private float attackPoint = 5f;
    private float horizontalHit = 40;
    private float verticallHit = 20;
    private bool singleHit = true;
    public bool aggressiveStanceMode = false;
    private bool readyToAttack = false;
    private bool cooldown = false;
    private bool duringJump = false;
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
        colider = base.GetComponent<BoxCollider2D>();

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
        // aggressive stance if hero is close else go to hero
        if(HeroWithinRange("attack"))
        {
            FrontAttack();
        }
        else if (HeroWithinRange("danger"))
        {
            //anim.SetTrigger("EnemyStanding");
            SetAggressiveStance();
        }
        else if(!duringJump)
        {
            aggressiveStanceMode = false;
            readyToAttack = false;
            cooldown = false;
            /*anim.SetTrigger("EnemyStanding");
             */
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
        else if (rangeName == "danger")
        {
            if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).y < 7.3f)
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).x < 29.6f) return true;
            return false;
        }
        else if (rangeName == "to close")
        {
            if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).y < 7.3f)
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).x < 13.6f) return true;
            return false;
        }
        else if (rangeName == "attack")
        {
            if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).y < 7.3f)
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).x < 5) return true;
            return false;
        }
        else if (rangeName == "hero on rat")
        {
            if (heroManager.GetHeroPosition().y > ratPosition.y)
            {
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).y < 7.7f)
                    if (DistanceVector(ref heroManager.GetHeroPosition(), ref ratPosition).x < 4) return true;
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
            colider.offset = new Vector2(2.5f, -1);
        }
        else
        {
            direction = "left";
            Move(ref moveSpeed, ref y_velocity, ref direction);
            anim.SetTrigger("EnemyLeftWalk");
            //colider.offset.Set(-1.1f, -0.65f);
            //colider.isTrigger = false;
            colider.offset = new Vector2(-2.5f, -1);
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
    public void SetAggressiveStance()
    {
        if (aggressiveStanceMode)
        {
            /*if (direction == "right") anim.SetTrigger("RightAggressiveStance");
            else anim.SetTrigger("LeftAggressiveStance"); */
            if (readyToAttack)
            {
                Move(ref runSpeed, ref y_velocity, ref direction);
            }
            if (HeroWithinRange("to close") && !cooldown)
            {
                Move(ref runSpeed, ref jumpSpeed, ref direction);
                readyToAttack = false;
                StartCoroutine(ExecuteActionAfterTime(0.3f, () => { cooldown = true; }));
                duringJump = true;
                StartCoroutine(ExecuteActionAfterTime(0.9f, () => { duringJump = false; }));
            }
        }
        else
        {
            /*if (direction == "right") anim.SetTrigger("StartRightAggressiveStance");
            else anim.SetTrigger("StartLeftAggressiveStance"); */
            aggressiveStanceMode = true;
            StartCoroutine(ExecuteActionAfterTime(2f, () => { readyToAttack = true; }));
        }
    }

    // upper attack
    // example call UpperAttack()
    private void UpperAttack()
    {
        if (HeroWithinRange("hero on rat") && !duringAttackMode)
        {
            heroManager.TakeSpecialDamage("paralysys", 0.6f);
            movingControl.Move(ref horizontalHit, ref verticallHit, ref direction);
        }
    }

    // front attack
    // example call FrontAttack()
    private void FrontAttack()
    {
        if (singleHit)
        {
            singleHit = false;
            heroManager.TakeHealth(attackPoint);
            StartCoroutine(ExecuteActionAfterTime(0.3f, () => { singleHit = true; }));
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
    IEnumerator ExecuteActionAfterTime(float time = 0, Action action = null)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
