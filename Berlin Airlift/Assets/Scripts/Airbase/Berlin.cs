using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Berlin : Singleton<Berlin>
{
    public float LastSupplyReductionTimestamp;
    public float SupplyLastInterval = 1.8f;

    private float m_currSupply;

    
    public float CurrentSupply { get { return m_currSupply; } }

    public float RemainTimeToWin = 180.0f;
    private void Start()
    {
        m_currSupply = 50;
    }

    private void Update()
    {
        RemainTimeToWin -= Time.deltaTime;
        if (Time.time - LastSupplyReductionTimestamp > SupplyLastInterval)
        {
            LastSupplyReductionTimestamp = Time.time;
            m_currSupply--;
        }
        if (m_currSupply <= 0)
        {
            SceneManager.LoadScene("Lose");
        }

        if(RemainTimeToWin <= 0)
        {
            if (SceneManager.GetActiveScene().name == "Day1")
            {
                SceneManager.LoadScene("Day2");
            }
            else if (SceneManager.GetActiveScene().name == "Day2")
            {
                SceneManager.LoadScene("Day3");
            }
            else
            {
                SceneManager.LoadScene("End");
            }
        }
        

    }

    public void SupplyBerlin(float amount)
    {
        Debug.Log("Berlin received supplies");
        m_currSupply += amount;
        
    }
}
