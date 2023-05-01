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
        
    }
    public void MissionComplete()
    {
        State = PlaneState.RTB;
    }

    public override void Land()
    {
        base.Land();
        m_homeBase.LineUp(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        InterceptorBase airbase =  collision.GetComponent<InterceptorBase>();
        if (airbase && State == PlaneState.RTB)
        {
            if(airbase.gameObject.GetInstanceID() == m_homeBase.gameObject.GetInstanceID())
            {
                Debug.Log("Do interceptor landing");
                Land();
            }
        }
    }


}
