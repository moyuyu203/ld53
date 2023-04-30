using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Radar : MonoBehaviour
{
    [SerializeField] float m_scanInterval = 0.2f;
    [SerializeField] PlaneGroup m_group;
    [SerializeField] float m_detectionRadius = 3.0f;

    private float m_lastScanTimeStamp;
    //private List<Transform> m_targets = new List<Transform>();
    public Action<List<Transform>> OnFoundTargets;
    //public List<Transform> Targets { get { return m_targets; } }
    
    private void Awake()
    {
        m_lastScanTimeStamp = Time.time;
     
    }
    private void Update()
    {
        if(Time.time - m_lastScanTimeStamp > m_scanInterval)
        {
            m_lastScanTimeStamp = Time.time;
            Scan();
        }
    }

    private void Scan()
    {
        
        List<Transform> targets = new List<Transform>();    
        Collider2D[] unknownObjects = Physics2D.OverlapCircleAll(transform.position, m_detectionRadius);
        foreach(Collider2D unknownObject in unknownObjects)
        {
            Plane plane = unknownObject.gameObject.GetComponent<Plane>();
            if (plane && plane.Group == PlaneGroup.Nato && plane.State != PlaneState.Destroyed)
            {
                targets.Add(unknownObject.gameObject.transform);
                
            }
        }

        if (targets.Count > 0)
        {
            OnFoundTargets?.Invoke(targets);
        }

    }
}
