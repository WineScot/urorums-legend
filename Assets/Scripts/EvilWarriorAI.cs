using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EvilWarriorAI : MonoBehaviour
{// ATTRIBUTES
    // scorpion components
    private Rigidbody2D rb2d;
    private Animator anim;
    private GameObject hero;
    private Enemy enemyManager;
    private Collider2D colider;
    private HeroManager heroManager;
    private MovingControl movingControl;
    // warrior Skill
    public bool followSkill;
    public bool up_attackSkill;
    public bool dwon_attackSkill;
    public bool defenceSkill;
    public bool jumpSkill;
    public bool dashSkill;
    public bool runSkill;
    // special mode
    private bool duringAttackMode = false;
    private bool followHeroMode = false;
    public string x_direction = "right";
    public string y_direction = "up";
    //variables
    private float x_viewRange = 50;
    private float y_viewRange = 50;
    private float moveSpeed = 15;
    private float zero = 0;
    private float attackPoint = 1f;
    // scorpion params
    private float x_velocity = 0;
    private float y_velocity = 0;
    public Vector2 scorpionPosition;

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
        // set scorpion position
        scorpionPosition = rb2d.position;
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
            // detect if hero stand on scorpion
            if (HeroWithinRange("hero on scorpion"))
            {
                anim.SetTrigger("EnemyStanding");
                Invoke("UpperAttack", 0.5f);
            }
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
            if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).y < 4.5f)
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).x < 3.5f) return true;
            return false;
        }
        else if (rangeName == "down attack")
        {
            if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).y < 4.5f)
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).x < 3.5f) return true;
            return false;
        }
        else if (rangeName == "up attack")
        {
            if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).y < 4.5f)
                if (DistanceVector(ref heroManager.GetHeroPosition(), ref scorpionPosition).x < 3.5f) return true;
            return false;
        }
        else if (rangeName == "hero on scorpion")
        {
            if (heroManager.GetHeroPosition().y > scorpionPosition.y)
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
        if (heroPosition.x > scorpionPosition.x + 2)
        {
            x_direction = "right";
            Move(ref moveSpeed, ref y_velocity, ref x_direction);
            anim.SetTrigger("EnemyRightWalk");
        }
        else if (heroPosition.x < scorpionPosition.x - 2)
        {
            x_direction = "left";
            Move(ref moveSpeed, ref y_velocity, ref x_direction);
            anim.SetTrigger("EnemyLeftWalk");
        }
        else
        {
            x_direction = "no";
            anim.SetTrigger("EnemyStanding");
        }
        if(heroPosition.y > scorpionPosition.y + 3) y_direction = "up";
        else if(heroPosition.y < scorpionPosition.y - 3) y_direction = "down";
        else y_direction = "no";
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
            duringAttackMode = true;
            if (x_direction == "right") anim.SetTrigger("ScorpionRightAttack");
            else anim.SetTrigger("ScorpionLeftAttack");
                StartCoroutine(ExecuteActionAfterTime(0.6f, () => { duringAttackMode = false; }));
            
        }
    }

    // set crust depends on direction
    // example call SetCrust()
    private void SetCrust()
    {
        if (x_direction == "right")
        {
            enemyManager.GetCrust() = new Vector4(10, 0, 5, 25);
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
