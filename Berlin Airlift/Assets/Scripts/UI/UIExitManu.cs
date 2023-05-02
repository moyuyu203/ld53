using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIExitManu : MonoBehaviour
{
    public Button ContBtn;
    public Button ExitBtn;
    public Button MainManuBtn;

   
    public Action OnExitManuHide;

    public void Start()
    {
        MainManuBtn.onClick.AddListener(MainManuBtnClick);
        ContBtn.onClick.AddListener(DoHideExitManuBtnClick);
        ExitBtn.onClick.AddListener(ExitGameBtnClick);

    }
    
    private void MainManuBtnClick()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Start");
    }
    private void ExitGameBtnClick()
    {
        Application.Quit();
    }

    private void DoHideExitManuBtnClick()
    {
        OnExitManuHide?.Invoke();
    }
}
