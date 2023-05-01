using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct TransportCount
{
    public Transport plane;
    public int count;
}

public class USAFCommand : Singleton<USAFCommand>
{
    
    public List<TransportCount> TransportInventory = new List<TransportCount>();
    private int m_totalPlanes;
    public int TotalLostPlanes;
    private int m_planeIndex;

    public int PlaneIndex { get { return m_planeIndex; } }

    private void Start()
    {
        m_totalPlanes = 0;
        TotalLostPlanes = 0;
        foreach(TransportCount tCount in TransportInventory)
        {
            m_totalPlanes += tCount.count;
        }
    }
    private void Update()
    {
        if(m_totalPlanes == TotalLostPlanes)
        {
            //Game over.
            SceneManager.LoadScene("Lose");
        }
    }

    public void SelectDeployment(int planeIndex)
    {
        Debug.Log("Selected Small plane");
        m_planeIndex = planeIndex;
    }

    public Transport RequestDeployment()
    {
        if (TransportInventory[m_planeIndex].count > 0)
        {
            TransportCount newPlaneCount = TransportInventory[m_planeIndex];
            newPlaneCount.count--;
            TransportInventory[m_planeIndex] = newPlaneCount;
            Transport instance = Instantiate(newPlaneCount.plane, Vector3.zero, Quaternion.identity);
            
            //Debug.Log(instance);
            return instance;

        }
        else
        {
            return null;
        }
    }
    
}
