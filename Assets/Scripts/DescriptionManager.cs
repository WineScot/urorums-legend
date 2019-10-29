using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionManager : MonoBehaviour
{
    public float fadeTime = 0.3f;
    Color bgColor;
    Text descriptionText;
    Image descriptionButtonImage;
    Text descriptionButtonText;

    // Start is called before the first frame update
    void Start()
    {
        descriptionText = transform.Find("Description").GetComponent<Text>();
        descriptionButtonImage = transform.Find("ConfirmButton").GetComponent<Image>();
        descriptionButtonText = transform.Find("ConfirmButton").transform.GetComponentInChildren<Text>();
        bgColor = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(1, 1, 1, 0f);
        descriptionButtonImage.color = new Color(1, 1, 1, 0f);
        descriptionText.text = "";
        descriptionButtonText.text = "";
    }

    IEnumerator FadeTo(float alphaValue)
    {
        float currentAlpha = GetComponent<Image>().color.a;
        for (float t = 0.01f; t>0f && t<1f; t += Time.deltaTime / fadeTime)
        {
            float alpha = Mathf.Lerp(currentAlpha, alphaValue, t);
            GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            descriptionButtonImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
    
    


    public void ShowBackground()
    {
        StartCoroutine(FadeTo(1f));
        // bgColor.a = 1f;
        // GetComponent<Image>().color = bgColor;
    }

    public void HideBackground()
    {
        StartCoroutine(FadeTo(0f));
        // bgColor.a = 0f;
        // GetComponent<Image>().color = bgColor;
        descriptionText.text = "";
        descriptionButtonText.text = "";

    }

    public void ChangeDescriptionText(string textToShow)
    {
        descriptionText.text = textToShow;
        descriptionButtonText.text = "I choose you!";
    }
}