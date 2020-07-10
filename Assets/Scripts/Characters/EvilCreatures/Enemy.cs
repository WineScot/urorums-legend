using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour, ICharacter
{
// ATTRIBUTES
    // enemy components
    private Rigidbody2D rb2d;
    private GameObject hero;
    private Animator anim;
    public GameObject health_points;
    public SpriteRenderer spri;
    private FightControl heroFightControl;
    // variables
    public float healthLevel = 100;
    public Vector4 measurements;
    public Vector4 areaPosition;
    public Vector4 crust;
    private bool underAttack = false;
    private float enemyPositionX = 0;
    private float enemyPositionY = 0;
    private float x_velocity = 0;
    private float y_velocity = 0;

    // STANDARD METHOD

    // Used for initialization
    void Start () 
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        hero = GameObject.FindGameObjectWithTag("Player");
        spri = GetComponent<SpriteRenderer>();
        health_points = transform.GetChild(0).transform.GetChild(0).gameObject;
        health_points.GetComponent<Text>().text = healthLevel.ToString();
        heroFightControl = hero.GetComponent<FightControl>();
        //base.GetComponent<SpriteRenderer>().sortingOrder = base.name.ToCharArray()[base.name.Length-2];
    }

    // Update is called once per frame
    void Update()
    {
        x_velocity = rb2d.velocity.x;
        y_velocity = rb2d.velocity.y;
        enemyPositionX = base.transform.position.x;
        enemyPositionY = base.transform.position.y;
        SetAreaPosition();
        health_points.GetComponent<Text>().text = healthLevel.ToString();
        if (!heroFightControl.HeroIsAttacking()) underAttack = false;
        if (heroFightControl.HeroIsAttacking() && !underAttack && EnemyInAttackArea(heroFightControl.AttackArea()))
        {
            underAttack = true;
            TakeDamage(ref heroFightControl.GetAttackPoint(), ref heroFightControl.AttackDirection());
            SpecialDamage(ref heroFightControl.GetSpecialEffect());
        }
    }

    // METHOD

    // set enemy edges
    // example call SetAreaPosition()
    private void SetAreaPosition()
    {
        areaPosition.x = enemyPositionX - measurements.x;
        areaPosition.y = enemyPositionY - measurements.y;
        areaPosition.z = enemyPositionX + measurements.z;
        areaPosition.w = enemyPositionY + measurements.w;
    }

    public void SetOrderInLayer(int number)
    {
        base.GetComponent<SpriteRenderer>().sortingOrder = number;
    }
    // return true if enemy area and attack area have common part
    // example call EnemyInAttackArea(attak area edges)
    private bool EnemyInAttackArea(Vector4 attackArea)
    {
        if (areaPosition.x > attackArea.z || areaPosition.z < attackArea.x || areaPosition.y > attackArea.w || areaPosition.w < attackArea.y) return false;
        else return true;
    }

    // play anim with trigger
    // example call PlayAnim("trigger name")
    public void PlayAnim(string animName) // play animation
    {
        anim.SetTrigger(animName);
    }

    // enemy's behaviour on special damage effect
    // example call SpecialDamage("special effect name")
    private void SpecialDamage(ref string specialEffect)
    {
        switch(specialEffect)
        {
            case "no": /*normal attack*/ break;
                /*special effect like burn, paralysis*/
        }
    }

    // reduce health level by damage depends on crust
    // example call TakeDamage(damage points,"attack direction")
    public void TakeDamage(ref float damage, ref string attackDirection)
    {
        switch(attackDirection)
        {
            case "up":
                {
                    damage = damage > crust.y ? damage - crust.y : 0;
                }
                break;
            case "down":
                {
                    damage = damage > crust.w ? damage - crust.w : 0;
                }
                break;
            case "right":
                {
                    damage = damage > crust.x ? damage - crust.x : 0;
                }
                break;
            case "left":
                {
                    damage = damage > crust.z ? damage - crust.z : 0;
                }
                break;
            case "total":
                {
                    damage = healthLevel;
                }
                break;
        }
        healthLevel -= damage;
        if (healthLevel <= 0)
        {
            Destroy(gameObject);
        }
    }

    // return crust
    // example call GetCrust()
    public ref Vector4 GetCrust()
    {
        return ref crust;
    }

    // enemy jump
    // example call FollowHero(hero position,scorpion position)
    public void Jump(float y_moveSpeed)
    {
        Vector2 movement = new Vector2(x_velocity, y_moveSpeed);
        /* scorpion jump animation */
        /* StartCoroutine(ExecuteActionAfterTime(0, () => { rb2d.velocity = movement; })); */
        rb2d.velocity = movement;
    }

    public void TakeDamage(IDamage damage)
    {
        throw new NotImplementedException();
    }
}
