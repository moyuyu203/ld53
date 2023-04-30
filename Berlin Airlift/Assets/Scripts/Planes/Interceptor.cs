using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : Plane
{
    private Transform m_target;
    private InterceptorBase m_homeBase;
    
    public Transform Target
    {
        get { return m_target; }
        set { m_target = value; }
    }
    protected override void Awake()
    {
        base.Awake();
        m_group = PlaneGroup.WarsawPact;
    }

    public void SetHomeBase(InterceptorBase airbase)
    {
        m_homeBase = airbase;
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (State == PlaneState.OnTask && m_target)
            Heading = m_target.position - transform.position;
        else if (State == PlaneState.RTB && m_homeBase)
            Heading = m_homeBase.transform.position - transform.position;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Plane otherPlane = collision.GetComponent<Plane>();
        if (otherPlane)
        {
            if(otherPlane.Group == PlaneGroup.Nato)
            {
                State = PlaneState.RTB;
                Debug.Log("Interception mission completed, return to base");
            }
        }

        InterceptorBase airbase =  collision.GetComponent<InterceptorBase>();
        if (airbase && State == PlaneState.RTB)
        {
            if(airbase.gameObject.GetInstanceID() == m_homeBase.gameObject.GetInstanceID())
            {
                Land();
            }
        }
    }


}
