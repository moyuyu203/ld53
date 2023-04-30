using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : Plane
{
    public float Range;
    public float Speed;

    public Transform Target;
    private InterceptorBase m_homeBase;
    private SpriteRenderer m_spriteRenderer;
    private void Awake()
    {
        m_state = PlaneState.Ready;
        m_range = Range;
        m_speed = Speed;
        m_heading = Vector3.right;
        m_group = PlaneGroup.WarsawPact;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.color = Color.red;
        //TakeOff();
    }

    public void SetHomeBase(InterceptorBase airbase)
    {
        m_homeBase = airbase;
    }
    protected override void FixedUpdate()
    {
        if (m_state == PlaneState.OnTask)
        {
            base.FixedUpdate();
            m_heading = (Target.position - transform.position).normalized;
        }
        else if(m_state == PlaneState.RTB)
        {
            transform.position += m_heading * m_speed * Time.deltaTime;
            m_distanceTraveled += m_speed * Time.deltaTime;
            m_heading = (m_homeBase.transform.position - transform.position).normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Plane otherPlane = collision.GetComponent<Plane>();
        if (otherPlane)
        {
            if(otherPlane.Group == PlaneGroup.Nato)
            {
                m_state = PlaneState.RTB;
                OnChangeState?.Invoke(m_state);
                Debug.Log("Interception mission completed, return to base");
            }
        }

        InterceptorBase airbase =  collision.GetComponent<InterceptorBase>();
        if (airbase && m_state == PlaneState.RTB)
        {
            if(airbase.gameObject.GetInstanceID() == m_homeBase.gameObject.GetInstanceID())
            {
                Land();
            }
        }
    }


}
