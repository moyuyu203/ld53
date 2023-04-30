using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airbase : MonoBehaviour
{
    [SerializeField] protected Transform m_runwayStart;
    [SerializeField] protected Transform m_runwayEnd;

    protected List<Plane> m_takeoffQueue = new List<Plane>();
    protected Plane m_currTakeoffPlane = null;
    // Start is called before the first frame update
    
    public bool RunwayClear { get { return m_currTakeoffPlane == null; } }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (m_currTakeoffPlane)
        {
            if(m_currTakeoffPlane.State == PlaneState.Ready)
            {
                m_currTakeoffPlane.TakeOff();
                m_currTakeoffPlane = null;
            }
            else
            {
                Vector3 rollToPosition = Vector3.Lerp(m_runwayStart.position, m_runwayEnd.position, m_currTakeoffPlane.TakeoffRollRatio);
                m_currTakeoffPlane.MovePlane(rollToPosition);
            }
        }
    }

    protected virtual void OrderTakeoff()
    {
        if(m_takeoffQueue.Count > 0)
        {
            Plane planeToTakeoff = m_takeoffQueue[0];
            m_takeoffQueue.RemoveAt(0);
            planeToTakeoff.StartPreparation();
            m_currTakeoffPlane = planeToTakeoff;
        }
    }

    public void LineUp(Plane plane)
    {
        plane.transform.position = m_runwayStart.position;
        m_takeoffQueue.Add(plane);
        plane.MovePlane(m_runwayStart.position);
    }
}
