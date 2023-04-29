using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlane : Plane
{
    
    public float Range;
    public float Speed;

    private SpriteRenderer m_spriteRenderer;
    private void Start()
    {
        
        m_state = PlaneState.Ready;
        m_range = Range;
        m_speed = Speed;
        m_heading = Vector3.right;
        m_group = PlaneGroup.Nato;
       

        OnChangeState += HandleChangeState;

        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.color = Color.blue;
        Debug.Log(m_state);
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
                m_state = PlaneState.Destroyed;
                m_spriteRenderer.color = Color.gray;
                OnChangeState?.Invoke(m_state);
            }
        }
    }
}
