using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SavePlayerData
{
    public float health;
    public float[] position;
    public int currentScene;
    public string saveDate;

    public SavePlayerData (HeroManager hero)
    {
        health = hero.healthLevel;
        position = new float[3];
        position[0] = hero.transform.position.x;
        position[1] = hero.transform.position.y;
        position[2] = hero.transform.position.z;
        currentScene = hero.currentScene;
        saveDate = "" + System.DateTime.Now;
    }
}
