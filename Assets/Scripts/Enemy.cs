﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    private Rigidbody2D rb2d;
    private GameObject player;
    private Animator anim;

    public GameObject health_points;
    public int healthLevel = 100;
    public SpriteRenderer spre;

    private int armorLevel = 0;

    // Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        spre = GetComponent<SpriteRenderer>();
        health_points = transform.GetChild(0).transform.GetChild(0).gameObject;
        health_points.GetComponent<Text>().text = healthLevel.ToString();
    }

    public void PlayAnim(string animName) // play animation
    {
        anim.SetTrigger(animName);
    }

    public void TakeHealthPoint(int attackPoints) // this function take hero health point
    {
        
        attackPoints -= armorLevel;
        if (attackPoints > 0)
        {
            healthLevel -= attackPoints;
        }
        if (healthLevel <= 0)
        {
            Destroy(gameObject);
        }
        Vector2 playerPosition = player.transform.position;
        Vector2 enemyPosition = GetComponent<Rigidbody2D>().position;
    }


    
    


    // Update is called once per frame
    void Update () 
    {
        health_points.GetComponent<Text>().text = healthLevel.ToString();
    }
}
