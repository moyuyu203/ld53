using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlane : Plane
{
    protected override void Awake()
    {
        base.Awake();
        m_group = PlaneGroup.Nato;
        TakeOff();
    }
    
    public override void TakeOff()
    {
     
        base.TakeOff();
        Debug.Log("Dummy plane takes off");
    }
    

    private void HandleChangeState(PlaneState state)
    {
        switch (state)
        {
            case PlaneState.Destroyed:
                Debug.Log("Destroy! ");
                break;
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Plane otherPlane = collision.gameObject.GetComponent<Plane>();
        if (otherPlane)
        {
            if (otherPlane.Group == PlaneGroup.WarsawPact && this.m_group == PlaneGroup.Nato)
            {
                PlaneShotDown();
            }
        }
    }
}
