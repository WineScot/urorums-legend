using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public void OnClickButton(int whichButton)
    {
        if (whichButton == 1)
            PlayerPrefs.DeleteAll();    // New Game - nie zaszkodzi wyczyścić PlayerPrefs, na wypadek gdyby jakimś cudem coś tam jednak było
        SceneManager.LoadScene(whichButton);
    }
}