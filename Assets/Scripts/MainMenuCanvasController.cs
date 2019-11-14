using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCanvasController : MonoBehaviour
{
    CanvasGroup bgCanvas;

    private float timeSinceCreation = 0f;
    private int buttonIsClicked = 0; // 0-not clicked, 1-new game, 2-load game, 3-settings, 4- controls, 5-about us, 6-save game
    public float timeUntilFadeIn = 0.5f;    
    public int speedOfFadeIn = 6;
    public int speedOfFadeOut = 15;

    // string[] keys = {"up", "down", "left", "right", "space", "x", "s"};
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
                bgCanvas.alpha = 0f;
        }
    }
}