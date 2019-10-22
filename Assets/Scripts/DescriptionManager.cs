using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionManager : MonoBehaviour
{
    Color bgColor;
    string descriptionText;

    // Start is called before the first frame update
    void Start()
    {
        descriptionText = GetComponentInChildren<Text>().text;
        // descriptionText = transform.GetChild(0).GetComponent<Text>().text;

        bgColor = GetComponent<Image>().color;
        HideBackground();
    }

    public void ShowBackground()
    {
        bgColor.a = 1f;
        GetComponent<Image>().color = bgColor;
    }

    public void HideBackground()
    {
        bgColor.a = 0f;
        GetComponent<Image>().color = bgColor;
    }

    public void ChangeDescriptionText(string textToShow)
    {
        descriptionText = textToShow;
    }
}