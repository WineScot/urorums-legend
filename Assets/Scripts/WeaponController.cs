using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    // ATTACK AREA
    private GameObject left_sword;
    private GameObject right_sword;
    private GameObject up_sword;
    private GameObject down_sword;
    // ATTACK CONTROL BUTTONS
    public string normal_attack = "v";
    public string strong_attack = "c";

    private GameObject hand;
    private Animator anim;
    private PlayerController player_controller;
    public bool onAttack = false; // true if hero currently attack


    // Use this for initialization
    void Start ()
    {
        left_sword = GameObject.FindGameObjectWithTag("Sword L");
        right_sword = GameObject.FindGameObjectWithTag("Sword R");
        up_sword = GameObject.FindGameObjectWithTag("Sword U");
        down_sword = GameObject.FindGameObjectWithTag("Sword D");

        player_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        hand = GameObject.FindGameObjectWithTag("Hand");
        anim = hand.GetComponent<Animator>();
    }

    void FinishAttack() // change onAttack for true
    {
        onAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (onAttack==false)
        {
            if (Input.GetKeyDown(normal_attack))
            {
                switch(player_controller.DirectionMode())
                {
                    case "up":
                        {
                            up_sword.gameObject.GetComponent<WeaponAttack>().activeAttackArea = true;
                        }
                     break;
                    case "down":
                        {
                            down_sword.gameObject.GetComponent<WeaponAttack>().activeAttackArea = true;
                        }
                    break;
                    default:
                        {
                            if (player_controller.GetDirection() == "right")
                            {

                                right_sword.gameObject.GetComponent<WeaponAttack>().activeAttackArea = true;

                            }
                            else
                            {
                                left_sword.gameObject.GetComponent<WeaponAttack>().activeAttackArea = true;

                            }
                        }
                    break;

                }
                onAttack = true;
                Invoke("FinishAttack", 0.5f); //Hero can attack next time after 0.5s*/

            }
        }
    }
}
