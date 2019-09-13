using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeroManager : MonoBehaviour
{
// ATTRIBUTES

    // Hero stats
    public float healthLevel = 100f;
    private float maxhealthLevel = 100f;
    private float armorPoint = 0f;
    // Hero behaviour
    public bool heroIsNoisy = false;
    // Hero component
    private MovingControl movingControl;
    private FightControl fightControl;
    public GameObject health_points;
  
// STANDARD METHODS

    // Used for initialization
    void Start()
    {
        health_points = transform.Find("Canvas/Text").gameObject; 
        movingControl = base.GetComponent<MovingControl>();
        fightControl = base.GetComponent<FightControl>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        // show hero health level
        health_points.GetComponent<Text>().text = healthLevel.ToString();

        // hero is noisy when attacks or make noises during moves
        if (fightControl.HeroIsAttacking() || movingControl.HeroIsMovingLoudly()) heroIsNoisy = true;
        else heroIsNoisy = false;

    }


//METHODS

    // Takes hero health points and decide about defeat
    // example call TakeHealth(enemy attack pionts)
    public void TakeHealth(float attackPoints)
    {
        attackPoints -= armorPoint;
        if (attackPoints > 0)
        {
            healthLevel -= attackPoints;
        }

        if (healthLevel <= 0)
        {
            //event when we die
        }
    }

}

