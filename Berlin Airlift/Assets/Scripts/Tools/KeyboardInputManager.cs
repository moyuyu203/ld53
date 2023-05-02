using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputManager : MonoBehaviour
{
    private bool m_pause;
   
    public UIExitManu ExitManu;

    private bool m_showExitPanel;

    private void Start()
    {
        ExitManu.OnExitManuHide += HideExitManu;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!m_showExitPanel)
            {
                ShowExitManu();
            }
            else
            {
                HideExitManu();
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_pause)
            {
                m_pause = false;
                Time.timeScale = 1.0f;
            }
            else
            {
                m_pause = true;
                Time.timeScale = 0;
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Retreat");
            foreach(Transport plane in FindObjectOfType<MouseInputManager>().selectedUnits)
            {
                //plane.State = PlaneState.RTB;
                //Debug.Log("Plane Retreat");
                Debug.Log("retreating plane");
                plane.OrderRetreat();
            }
        }
    }
    private void ShowExitManu()
    {
        Time.timeScale = 0;
        m_showExitPanel = true;
        ExitManu.gameObject.SetActive(true);
    }
    public void HideExitManu()
    {

        Time.timeScale = 1.0f;
        m_showExitPanel = false;
        ExitManu.gameObject.SetActive(false);
    }

}
