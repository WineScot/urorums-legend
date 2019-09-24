using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvasController : MonoBehaviour
{
    CanvasGroup bgImage;
    private float timeSinceCreation = 0f;

    public float timeUntilFadeIn = 0.5f;
    public int speedOfFadeIn = 20;

    void Start ()
    {
        bgImage = GameObject.Find("Canvas").GetComponent<CanvasGroup>();
        bgImage.alpha = 0f;
    }

    void FixedUpdate ()
    {
        timeSinceCreation += 0.02f;
        
        if (bgImage.alpha < 1f && timeSinceCreation >= timeUntilFadeIn)
        {
            if (speedOfFadeIn * 0.00390625f + bgImage.alpha <= 1f)
            {
                bgImage.alpha += speedOfFadeIn * 0.00390625f;  // 1/256
                if (bgImage.alpha > 0.9f)
                    bgImage.interactable = true;
            }                                           // 1f makes it fully visible, 0f makes it fully transparent.
            else
            {
                bgImage.alpha = 1f;
                bgImage.interactable = true;
            }
        }
    }
}

