using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCanvasController : MonoBehaviour
{
    CanvasGroup bgCanvas;
    HeroManager hero;

    private float timeSinceCreation = 0f;
    private int currentSave = 0;
    private int buttonIsClicked = 0; // 0-not clicked, 1-new game, 2-load game, 3-settings, 4- controls, 5-about us, 6-save game
                                     // >>LOADING<<   21-save 1, 22-save 2, 23-save 3, 24-save 4, 25-save 5       >>SAVING<<     31-save 1, 32-save 2, 33-save 3, 34-save 4, 35-save 5
    public float timeUntilFadeIn = 0.5f;    
    public int speedOfFadeIn = 6;
    public int speedOfFadeOut = 15;

    string[] keys = {"up", "down", "left", "right", "space", "x", "s"};
    void Start()
    {
        bgCanvas = GameObject.Find("Canvas").GetComponent<CanvasGroup>();
        bgCanvas.alpha = 0f;
    }

    void FixedUpdate()
    {
        timeSinceCreation += 0.02f;

        if (bgCanvas.alpha < 1f && timeSinceCreation >= timeUntilFadeIn && buttonIsClicked == 0)
        {
            if (speedOfFadeIn * 0.00390625f + bgCanvas.alpha <= 1f)
            {
                bgCanvas.alpha += speedOfFadeIn * 0.00390625f;  // 1/256
                if (bgCanvas.alpha > 0.95f)
                    bgCanvas.interactable = true;
            }                                           // 1f makes it fully visible, 0f makes it fully transparent.
            else
            {
                bgCanvas.alpha = 1f;
                bgCanvas.interactable = true;
            }
            return;
        }
        if (buttonIsClicked != 0)  // FUNKCJA MA SIĘ WYKONYWAĆ PO WCIŚNIĘCIU DOWOLNEGO GUZIKA
        {
            bgCanvas.interactable = false;
            bgCanvas.alpha -= speedOfFadeOut * 0.00390625f;

            if (bgCanvas.alpha <= 0f)
            {
                bgCanvas.alpha = 0f;
                if (buttonIsClicked <= 20)                      //  0-10 MENU HANDLER
                {
                    if (buttonIsClicked == 2)
                        SaveLoadManager.DoWeLoad = true;
                    SceneManager.LoadScene(buttonIsClicked);
                }
                else if (buttonIsClicked <= 30)                 // 21-29 SAVES LOADING
                {
                    if (false /* save doesn't exist (wasn't created) */ )           
                    {
                        currentSave = buttonIsClicked - 20;     // LOAD SAVE NUMBER >> buttonIsClicked - 20 <<
                    }
                    else  // SAVE EXISTS
                    {
                        currentSave = buttonIsClicked - 20;
                        SaveSystem.LoadPlayer(currentSave);
                    }
                }
                else if (buttonIsClicked <= 40)                    // 31-39 PROGRESS SAVING
                {
                    // SaveLoadManager.DoWeLoad = false; - jeśli wybrana została opcja "New Game"
                    hero.SaveHero();
                }
                else if (buttonIsClicked <= 60)                    // 40-59 CONTROL KEYS SETUP
                {

                }
            }
        }
    }

    public ref int ButtonIsClicked()
    {
         return ref buttonIsClicked;
    }
}

public static class SaveLoadManager
{
    public static bool DoWeLoad { get; set; }
}