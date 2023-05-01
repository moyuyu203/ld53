using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UILose : MonoBehaviour
{
    // Start is called before the first frame update
    public Button RetryBtn;
    void Start()
    {
        RetryBtn.onClick.AddListener(RetryGame);
    }

    void RetryGame()
    {
        SceneManager.LoadScene("Start");
    }
}
