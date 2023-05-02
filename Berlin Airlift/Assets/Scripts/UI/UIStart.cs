using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIStart : MonoBehaviour
{
    public Button StartBtn;
    public Button TutorialBtn;
    public Button ExitBtn;
    public UITutorialPage TutorialPage;
    void Start()
    {
        StartBtn.onClick.AddListener(StartGame);
        TutorialBtn.onClick.AddListener(ShowTutorial);
        ExitBtn.onClick.AddListener(ExitGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene("Day1");
    }
    void ExitGame()
    {
        Application.Quit();
    }
    void ShowTutorial()
    {
        TutorialPage.gameObject.SetActive(true);
    }


    
}
