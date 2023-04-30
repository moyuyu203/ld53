using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : Plane
{
    protected override void Awake()
    {
        base.Awake();
        m_group = PlaneGroup.WarsawPact;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (State == PlaneState.OnTask && m_target)
            Heading = m_target.position - transform.position;
        else if (State == PlaneState.RTB && m_homeBase)
            Heading = m_homeBase.transform.position - transform.position;
        if (Target)
        {
            Plane targetPlane = Target.gameObject.GetComponent<Plane>();
            if (targetPlane && State == PlaneState.OnTask && targetPlane.State == PlaneState.Destroyed || targetPlane.State == PlaneState.Grounded || targetPlane.State == PlaneState.Ready)
            {
                //Target lost, rtb
                Target = null;
                State = PlaneState.RTB;
            }
        }
    }
    public override void Land()
    {
        base.Land();
        m_homeBase.LineUp(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Plane otherPlane = collision.GetComponent<Plane>();
        if (otherPlane)
        {
            if(otherPlane.Group == PlaneGroup.Nato && (otherPlane.State == PlaneState.OnTask || otherPlane.State == PlaneState.RTB))
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
                Debug.Log("Do interceptor landing");
                Land();
            }
        }
    }


}
