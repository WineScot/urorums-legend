using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    MainMenuCanvasController canvasControl;

    private void Start()
    {
        canvasControl = this.transform.parent.GetComponent<MainMenuCanvasController>();
        //canvasControl = GameObject.Find("Canvas").GetComponent<MainMenuCanvasController>();
    }

    public void OnClickButton(int whichButton)
    {
        canvasControl.ButtonIsClicked() = whichButton;
    }
}