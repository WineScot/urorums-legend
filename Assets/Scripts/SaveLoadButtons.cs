using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadButtons : MonoBehaviour
{
    public bool doesLoad;            // zmienna określająca czy przy wczytywaniu sceny save/load zapisujemy czy odczytujemy
    public int buttonIndex;
    public bool isTaken = false;
    DescriptionManager descriptionManager;
    void Start()
    {
        if (isTaken)
            this.GetComponentInChildren<Text>().text += " - zajęty";
        else
            this.GetComponentInChildren<Text>().text += " - wolny";

        descriptionManager = GameObject.FindWithTag("Description").GetComponent<DescriptionManager>();
    }

    public void OnClick(int whichButton)
    {
        string descriptionText;
        descriptionText = "Data zapisu: " + System.DateTime.Now;
        descriptionText += "\nPostęp: 0% bo nie mamy tego zaimplementowanego ;P";
        descriptionText += "\nFajnie by było żeby jeszcze się tu wyświetlało eq";

        Debug.Log(descriptionText);

        descriptionManager.ChangeDescriptionText(descriptionText);
        descriptionManager.ShowBackground();                                        // SaveLoadManager jest w skrypcie MainMenuCanvasController
        if (!SaveLoadManager.DoWeLoad)                                              // jeśli nie odczytujemy, to zapisujemy, więc
            whichButton += 10;                                                      // w ButtonIsClicked() trzeba wywołać funkcję o tagu +10
        GetComponent<MainMenuHandler>().OnClickButton(whichButton+20);              // Spowrotem do MainMenuHandler -> MainMenuCanvasController, +20 żeby wykonała się funkcja obsługująca save/load
    }

    void Update()
    {
        
    }


}
