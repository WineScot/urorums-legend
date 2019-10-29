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
    SavePlayerData data;

    DescriptionManager descriptionManager;
    void Start()
    {
        if (buttonIndex > 10) return;
        
        descriptionManager = GameObject.FindWithTag("Description").GetComponent<DescriptionManager>();
        data = SaveSystem.LoadSaveData(buttonIndex);

        if (data != null)       // save exists
        {
                this.GetComponentInChildren<Text>().text += " - zajęty";
        }
        else                    // save doesn't exist
        {
                this.GetComponentInChildren<Text>().text += " - wolny";
                this.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void OnClick(int whichButton)
    {
        if (data != null)       // save exists
        {
            string descriptionText;
            descriptionText = "Data zapisu: " + data.saveDate;
            descriptionText += "\nPostęp: 0% bo nie mamy tego zaimplementowanego ;P";
            descriptionText += "\nFajnie by było żeby jeszcze się tu wyświetlało eq";
                                                                                        // SaveLoadManager jest w skrypcie MainMenuCanvasController
            if (!SaveLoadManager.DoWeLoad)                                              // jeśli nie odczytujemy, to zapisujemy, więc
                whichButton += 10;                                                      // w ButtonIsClicked() trzeba wywołać funkcję o tagu +10
            GameObject.Find("ConfirmButton").GetComponent<SaveLoadButtons>().buttonIndex = whichButton + 20;     // przekazujemy nr save'a do descriptionManager, żeby button w description miał do niego dostęp
                                                                                                                 // dodajemy +20, żeby w MainMenuHandler -> MainMenuCanvasController wykonała się funkcja obsługująca save/load
            descriptionManager.ChangeDescriptionText(descriptionText);
            descriptionManager.ShowBackground();                                        
        }                                                                                                       
        else            // save doesn't exist
        {
            descriptionManager.HideBackground();
        }
    }

    public void DeleteOnClick(int whichButton)
    {
        SaveSystem.DeleteSave(whichButton);
        this.gameObject.SetActive(false);
        transform.parent.GetComponentInChildren<Text>().text = "Slot " + whichButton + " - wolny";
        GameObject.FindWithTag("Description").GetComponent<DescriptionManager>().HideBackground();
    }

    public void BackgroundOnClick()
    {
        GameObject.FindWithTag("Description").GetComponent<DescriptionManager>().HideBackground();
    }

    public void ConfirmOnClick()
    {
        //GetComponent<MainMenuHandler>().OnClickButton(this.buttonIndex);
        this.transform.parent.transform.parent.GetComponent<MainMenuCanvasController>().ButtonIsClicked() = buttonIndex;
    }
}
