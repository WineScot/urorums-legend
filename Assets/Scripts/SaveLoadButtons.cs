using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadButtons : MonoBehaviour
{
    public int buttonIndex;
    public bool isTaken = false;
    SavePlayerData data;

    [SerializeField]
    GameObject confirmButton;
    [SerializeField]
    GameObject Xbutton;

    DescriptionManager descriptionManager;
    void Start()
    {
        if (buttonIndex >= 10)      // jeśli buttony to "X" do usuwania save'ów
            return;

        descriptionManager = GameObject.FindWithTag("Description").GetComponent<DescriptionManager>();
        data = SaveSystem.LoadSaveData(buttonIndex);


        if (data != null)       // save exists
        {
            this.GetComponentInChildren<Text>().text += " - zajęty";
        }
        else                    // save doesn't exist
        {
            this.GetComponentInChildren<Text>().text += " - wolny";
            try
            {
                Xbutton.SetActive(false);
            }
            catch (NullReferenceException)
            {
                // jeśli Xbutton nie znaleziony, to mamy do czynienia z buttonami w scenie Save Game, które nie mają swoich Xbuttonów - jest okej ;)
            }
        }
    }

    public void OnClickSave(int whichButton)    // buttony-sloty - OnClick
    {
        if (data != null)       // save exists
        {
            string descriptionText;
            descriptionText = "Data zapisu: " + data.saveDate;
            descriptionText += "\nPostęp: 0% bo nie mamy tego zaimplementowanego ;P";
            descriptionText += "\nFajnie by było żeby jeszcze się tu wyświetlało eq";

            descriptionManager.ChangeDescriptionText(descriptionText);
            confirmButton.GetComponent<SaveLoadButtons>().buttonIndex = whichButton;

            descriptionManager.ShowBackground();
        }
        else            // button nie przechowuje żadnego save'a, więc można od razu zapisywać
        {
            PlayerPrefs.SetInt("CurrentSave", whichButton);
            SaveSystem.SavePlayer(whichButton);
        }
    }

    public void OnClickLoad(int whichButton)    // buttony-sloty - OnClick
    {
        {
            if (data != null)       // save exists
            {
                string descriptionText;
                descriptionText = "Data zapisu: " + data.saveDate;
                descriptionText += "\nPostęp: 0% bo nie mamy tego zaimplementowanego ;P";
                descriptionText += "\nFajnie by było żeby jeszcze się tu wyświetlało eq";

                confirmButton.GetComponent<SaveLoadButtons>().buttonIndex = whichButton;     // przekazujemy nr save'a do descriptionManager, żeby button w description miał do niego dostęp
                                                                                                  // dodajemy +20, żeby w MainMenuHandler -> MainMenuCanvasController wykonała się funkcja obsługująca save/load
                descriptionManager.ChangeDescriptionText(descriptionText);
                descriptionManager.ShowBackground();
            }
            else            // save doesn't exist
            {
                descriptionManager.HideBackground();
            }
        }
    }

    public void DeleteOnClick(int whichButton)
    {
        SaveSystem.DeleteSave(whichButton);
        this.gameObject.SetActive(false);
        transform.parent.GetComponentInChildren<Text>().text = "Slot " + whichButton + " - wolny";
        descriptionManager.HideBackground();
    }

    public void BackgroundOnClick()
    {
        descriptionManager.HideBackground();
    }

    public void LoadHeroOnClick()   // ConfirmButton w scenie Load Game
    {
        SaveSystem.LoadPlayer(buttonIndex);
    }

    public void SaveHeroOnClick()   // ConfirmButton w scenie Save Game
    {
        SaveSystem.SavePlayer(buttonIndex);
    }
}
