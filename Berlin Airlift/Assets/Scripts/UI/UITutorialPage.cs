using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorialPage : MonoBehaviour
{
    public Button BackBtn;

    private void Start()
    {
        BackBtn.onClick.AddListener(GoBack);

        
    }
    void GoBack()
    {
        this.gameObject.SetActive(false);
    }
}
