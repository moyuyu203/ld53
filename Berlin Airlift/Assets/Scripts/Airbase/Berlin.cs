using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Berlin : Singleton<Berlin>
{
    [SerializeField] private float m_totalSupplyNeeded;

    private float m_currSupply;

    public void SupplyBerlin(float amount)
    {
        Debug.Log("Berlin received supplies");
        m_currSupply += amount;
        if(m_currSupply > m_totalSupplyNeeded)
        {
            Debug.Log("Win!");
            //Time.timeScale = 0;
            SceneManager.LoadScene("End");
            
        }
    }
}
