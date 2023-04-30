using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlaneCount
{
    public Transport transport;
    public int count;
}

public class USAFCommand : Singleton<USAFCommand>
{
    
    public List<PlaneCount> TransportInventory = new List<PlaneCount>();
    private int m_planeIndex;

    public void SelectDeployment(int planeIndex)
    {
        Debug.Log("Selected Small plane");
        m_planeIndex = planeIndex;
    }

    public Transport RequestDeployment()
    {
        if (TransportInventory[m_planeIndex].count > 0)
        {
            PlaneCount newPlaneCount = TransportInventory[m_planeIndex];
            newPlaneCount.count--;
            TransportInventory[m_planeIndex] = newPlaneCount;
            Transport instance = Instantiate(newPlaneCount.transport, Vector3.zero, Quaternion.identity);
            
            Debug.Log(instance);
            return instance;

        }
        else
        {
            return null;
        }
    }
    
}
