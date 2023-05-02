using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Radar : MonoBehaviour, IMouseInput
{
    [SerializeField] float m_scanInterval = 0.2f;
    [SerializeField] PlaneGroup m_group;
    [SerializeField] float m_detectionRadius = 3.0f;

    private float m_lastScanTimeStamp;
    //private List<Transform> m_targets = new List<Transform>();
    public Action<List<Transform>> OnFoundTargets;
    //public List<Transform> Targets { get { return m_targets; } }

    public const int numSegments = 30;
    public const float IndicatorHideTime = 0.1f;
    private LineRenderer m_lineRenderer;
    private float m_remainIndicatorTime;
    private void Awake()
    {
        m_lastScanTimeStamp = Time.time;
        m_lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        if (m_lineRenderer)
        {
            CreateCircle();
            m_lineRenderer.enabled = false;
        }
    }
    private void Update()
    {
        if(Time.time - m_lastScanTimeStamp > m_scanInterval)
        {
            m_lastScanTimeStamp = Time.time;
            Scan();
        }
        if (m_lineRenderer)
        {
            if (m_remainIndicatorTime <= 0)
            {
                m_lineRenderer.enabled = false;
            }
            else
            {
                m_remainIndicatorTime -= Time.deltaTime;
            }
        }
    }

    public void MouseHover()
    {
        if (m_lineRenderer)
        {
            m_remainIndicatorTime = IndicatorHideTime;
            m_lineRenderer.enabled = true;
        }
    }

    
    public void MouseClick()
    {

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

    void CreateCircle()
    {

        m_lineRenderer.positionCount = numSegments + 1; // Add one extra point to close the circle
        m_lineRenderer.useWorldSpace = false; // Set to true if you want to draw the circle in world space
        m_lineRenderer.material = new Material(Shader.Find("Sprites/Default")); ;
        m_lineRenderer.startWidth = 0.1f;
        m_lineRenderer.endWidth = 0.1f;
        
        float angle = 360f / numSegments;

        

        for (int i = 0; i <= numSegments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle * i) * m_detectionRadius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle * i) * m_detectionRadius;
            m_lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }

    }
}
