using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class HeroManager : MonoBehaviour
{
    // ATTRIBUTES

    // Hero stats
    public float healthLevel = 100f;
    private float maxhealthLevel = 100f;
    private float armorPoint = 0f;
    // Hero behaviour
    private bool heroIsNoisy = false;
    // Hero component
    private MovingControl heroMovingControl;
    private FightControl fightControl;
    public GameObject health_points;
    // Hero position
    private Vector2 heroPosition;
    // variables
    private bool isCoroutineExecuting;
    public int currentScene = 1;        //scene in which hero saved the game
    public int currentSaveNumber = 0;   //currently chosen save

    // STANDARD METHODS

    // Used for initialization
    void Start()
    {
        health_points = transform.Find("Canvas/Text").gameObject;
        heroMovingControl = base.GetComponent<MovingControl>();
        fightControl = base.GetComponent<FightControl>();

        // loading save
        if (PlayerPrefs.GetInt("CurrentSave") >= 1 && PlayerPrefs.GetInt("CurrentSave") <= 5)             // currentSaveNumber != 0)
        {
            LoadHero();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        // show hero health level
        health_points.GetComponent<Text>().text = healthLevel.ToString();

        // hero is noisy when attacks or make noises during moves
        if (fightControl.HeroIsAttacking() || heroMovingControl.HeroIsMovingLoudly()) heroIsNoisy = true;
        else heroIsNoisy = false;

        //set hero position
        heroPosition = new Vector2(base.transform.position.x, base.transform.position.y);
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

    // Return hero position as Vector2 (x,y)
    // example call GetHeroPosition()
    public ref Vector2 GetHeroPosition()
    {
        return ref heroPosition;
    }

    // activates the special effect of taken damage
    // example call TakeSpecialDamage("special damage name", time, attack point)
    public void TakeSpecialDamage(string specialEffectName, float time = 0, float attackPoint = 0)
    {
        switch(specialEffectName)
        {
            case "paralysys": Pralysys(time); break;
        }
    }

    // paralysys hero for time
    // example call Pralysys(paralysis time)
    public void Pralysys(float time)
    {
        heroMovingControl.GetCanMove() = false;
        StartCoroutine(ExecuteActionAfterTime(time, () => { heroMovingControl.GetCanMove() = true; }));
    }

    public bool HeroIsNoisy()
    {
        return heroIsNoisy;
    }

    // used for make action after time 
    // example call StartCoroutine(ExecuteActionAfterTime(time,action))
    IEnumerator ExecuteActionAfterTime(float time, Action action)
    {
        if (isCoroutineExecuting) yield break;
        isCoroutineExecuting = true;
        yield return new WaitForSeconds(time);
        action();
        isCoroutineExecuting = false;
    }

    // Saving and Loading
    public void SaveHero()
    {
        if (currentSaveNumber != 0)
            SaveSystem.SavePlayer(currentSaveNumber, this);
        else
        {
            SaveLoadManager.DoWeLoad = false;
            SceneManager.LoadScene(2);
        }
    }
    public void LoadHero()     //(SavePlayerData data)
    {
        healthLevel = PlayerPrefs.GetFloat("HeroHealth");
        Vector3 position;
        position.x = PlayerPrefs.GetFloat("HeroPositionX");
        position.y = PlayerPrefs.GetFloat("HeroPositionY");
        position.z = PlayerPrefs.GetFloat("HeroPositionZ");
        transform.position = position;

        currentScene = PlayerPrefs.GetInt("CurrentScene"); //scene in which hero saved the game
        transform.Find("Canvas/DebugText").gameObject.GetComponent<Text>().text = "hero health: " + PlayerPrefs.GetFloat("HeroHealth") + "\nPos.x: " + PlayerPrefs.GetFloat("HeroPositionX") + "\nPos.y: " + PlayerPrefs.GetFloat("HeroPositionY") + "\nPos.z: " + PlayerPrefs.GetFloat("HeroPositionZ");
        //PlayerPrefs.DeleteAll();
    }
}

