using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsChangeButtonHandler : MonoBehaviour
{
    private bool interactable;

    public void Start()
    {
        interactable = base.GetComponent<Button>().interactable;
    }

    public void OnClickButton(int keyID)    // 40-up, 41-down, 42-left, 43-right, 44-jump, 45-dash, 46-save
    {
        interactable = false;
        switch (keyID-40)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:                
                return;
        }


    }

}
