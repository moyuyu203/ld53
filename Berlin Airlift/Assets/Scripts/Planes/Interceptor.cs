using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : Plane
{
    public float Range;
    public float Speed;

    public Transform Target;

    private SpriteRenderer m_spriteRenderer;
    private void Start()
    {
        m_state = PlaneState.Ready;
        m_range = Range;
        m_speed = Speed;
        m_heading = Vector3.right;
        m_group = PlaneGroup.WarsawPact;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.color = Color.red;
        TakeOff();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        m_heading = (Target.position - transform.position).normalized;
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
                Debug.Log("Interception mission succed, return to base");
            }
        }
    }


}
