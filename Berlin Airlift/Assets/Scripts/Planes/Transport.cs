using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : Plane
{
    [SerializeField] private float m_payload;
   
    protected override void Awake()
    {
        base.Awake();
        m_group = PlaneGroup.Nato;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //Heading = (Berlin.Instance.transform.position - transform.position).normalized;
        if(Target == null)
        {
            Target = Berlin.Instance.transform;
        }
    }

    public override void TakeOff()
    {

        base.TakeOff();
        
        Debug.Log("Transport plane takes off");
    }

    public void OrderRetreat()
    {
        State = PlaneState.RTB;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Plane otherPlane = collision.gameObject.GetComponent<Plane>();
        if (otherPlane)
        {
            if (otherPlane.Group == PlaneGroup.WarsawPact && this.m_group == PlaneGroup.Nato && State != PlaneState.Destroyed && otherPlane.State == PlaneState.OnTask)
            {
                //PlaneShotDown();
                OrderRetreat();
            }
        }
        Berlin berlinAirField = collision.gameObject.GetComponent<Berlin>();
        if (berlinAirField)
        {
            berlinAirField.SupplyBerlin(m_payload);
            Land();
            gameObject.SetActive(false);
        }
        if (State == PlaneState.RTB)
        {
            TransportBase alliesBase = collision.gameObject.GetComponent<TransportBase>();
            if (alliesBase)
            {
                Land();
            }
        }
    }
}
