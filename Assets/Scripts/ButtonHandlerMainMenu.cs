using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHandlerMainMenu : MonoBehaviour
{
    public void OnClickNewGame()
    {
        //CanvasGroup canvasControl = base.transform.parent.GetComponent<CanvasGroup>();
        SceneManager.LoadScene(1);
    }
}
