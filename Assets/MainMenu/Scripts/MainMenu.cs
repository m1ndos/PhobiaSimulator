using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject choosingPhobiaWindow;
    public GameObject mainWindow;
    public GameObject buttonAcro;
    public GameObject buttonClau;
    public TextMeshProUGUI selectionText;
    private string choose = null;
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    public void ToMainWindow(GameObject btn)
    {
        choosingPhobiaWindow.SetActive(false);
        if(btn == buttonAcro)
        {
            selectionText.text = "Выбрана акрофобия";
            choose = "акрофобия";
        }else
        {
            selectionText.text = "Выбрана клаустрофобия";
            choose = "клаустрофобия";
        }
        mainWindow.SetActive(true);
    }
    public void ToChoosingPhobiaWindow()
    {
        mainWindow.SetActive(false);
        choose = null;
        choosingPhobiaWindow.SetActive(true);
    }
    public void GoToScene()
    {
        if(choose == "акрофобия")
        {
            SceneManager.LoadScene("AcrophobiaScene");
        }
        else
        {

        }
    }
}
