using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TransportBase : Airbase, IMouseInput
{
    public void MouseHover()
    {
        //Debug.Log("Mouse is hover");

    }
    public void MouseClick()
    {
        Debug.Log("Mouse click");
        TryDeployment();
    }
    protected override void Update()
    {
        base.Update();
        if (m_currTakeoffPlane == null)
        {
            OrderTakeoff();
        }
        if(m_currTakeoffPlane && (m_currTakeoffPlane.gameObject.activeInHierarchy == false || m_currTakeoffPlane.State == PlaneState.Destroyed))
        {
            m_currTakeoffPlane = null;
        }
    }
    private void TryDeployment()
    {

        Plane plane = USAFCommand.Instance.RequestDeployment();

        if (plane)
        {
            //plane.TakeOff();
            //m_takeoffQueue.Add(plane);
            //OrderTakeoff();
            plane.SetHomeBase(this);
            LineUp(plane);

        }
    }
}
