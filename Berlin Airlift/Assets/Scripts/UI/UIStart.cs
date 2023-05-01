using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIStart : MonoBehaviour
{
    public Button StartBtn;
    void Start()
    {
        StartBtn.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    
}
