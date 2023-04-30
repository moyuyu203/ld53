using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NatoAirbase : Waypoint, IMouseInput
{
    [SerializeField] private List<Transport> TakeOffTimeTable = new List<Transport>();


    private void Start()
    {
    }


    private void Update()
    {
        if(TakeOffTimeTable.Count > 0)
        {
            TakeOffTimeTable[0].StartPreparation();
            if (TakeOffTimeTable[0].State == PlaneState.Ready)
            {
                TakeOffTimeTable[0].TakeOff();
                TakeOffTimeTable.RemoveAt(0);
            }
        }
    }



    public void MouseHover()
    {
        //Debug.Log("Mouse is hover");

    }
    public void MouseClick()
    {
        Debug.Log("Mouse click");
        TryDeployment();
    }

    private void TryDeployment()
    {
        
        Transport plane = USAFCommand.Instance.RequestDeployment();
        
        if (plane)
        {
            //plane.TakeOff();
            plane.transform.position = transform.position;
            TakeOffTimeTable.Add(plane);
           
        }
    }
}
