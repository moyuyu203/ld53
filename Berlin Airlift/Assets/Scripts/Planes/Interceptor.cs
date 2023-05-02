using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : Plane
{
    public Action OutOfFuel;

    protected override void Awake()
    {
        base.Awake();
        m_group = PlaneGroup.WarsawPact;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (RemainFuel < 0.5f)
        {
            State = PlaneState.RTB;
            OutOfFuel?.Invoke();
        }

        if(State == PlaneState.RTB && Vector2.Distance(transform.position, m_homeBase.transform.position) < 0.1)
        {
            Debug.Log("Do interceptor landing");
            Land();
        }
        

    }
    public void MissionComplete()
    {
        State = PlaneState.RTB;
    }

    public override void Land()
    {
        base.Land();
        //m_homeBase.LineUp(this);
    }
    


}
