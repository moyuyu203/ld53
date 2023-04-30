using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlaneGroup
{
    Nato,
    WarsawPact
}

public enum PlaneState
{
    Ready,
    OnTask,
    RTB,
    Grounded,
    Destroyed
}

public abstract class Plane : MonoBehaviour
{
    protected string m_name;
    protected string m_info;

    protected PlaneState m_state;
    protected PlaneGroup m_group;

    protected float m_speed;
    protected float m_range;
    protected float m_distanceTraveled;

    protected Vector3 m_heading;

    public Action<PlaneState> OnChangeState;
    public PlaneGroup Group { get { return m_group; } }
    public PlaneState State { get { return m_state; } }
    protected virtual void FixedUpdate()
    {
       // Debug.Log(m_state);
        if (m_state == PlaneState.OnTask)
        {
            transform.position += m_heading * m_speed * Time.deltaTime;
            m_distanceTraveled +=  m_speed * Time.deltaTime;
        }

        if(m_distanceTraveled > m_range && m_state != PlaneState.Destroyed)
        {
            m_state = PlaneState.Destroyed;
            OnChangeState!.Invoke(m_state);
        }
    }
    public virtual void TakeOff()
    {
        this.gameObject.SetActive(true);
        if (m_state == PlaneState.Ready)
        {
            Debug.Log("Plane take off");
            m_state = PlaneState.OnTask;
            OnChangeState?.Invoke(m_state);
        }
        
    }
    public virtual void Land()
    {
        Debug.Log("Plane landed");
        m_state = PlaneState.Grounded;
        m_heading = Vector3.right;
        m_distanceTraveled = 0;
        OnChangeState?.Invoke(m_state);
        this.gameObject.SetActive(false);
    }

    



}
