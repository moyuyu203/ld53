using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct InterceptorCount
{
    public Interceptor plane;
    public int count;
}
public class InterceptorBase : Airbase
{
    public List<InterceptorCount> PlaneInventory = new List<InterceptorCount>();

   
    private void Awake()
    {
        foreach(InterceptorCount pCount in PlaneInventory)
        {
            for (int i = 0; i < pCount.count; i++)
            {
                Plane interceptor = Instantiate(pCount.plane);
                interceptor.SetHomeBase(this);
                LineUp(interceptor);
            }
        }
        
    }

    public Plane ReceiveOrder(Transform target)
    {
        if(RunwayClear && m_takeoffQueue.Count > 0)
        {
            OrderTakeoff();
            m_currTakeoffPlane.Target = target;
            return m_currTakeoffPlane;
        }
        else
        {
            return null;
        }
    }
    

  
}
