using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
    [SerializeField] private float m_preparationTime;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_range;

    private float m_distanceTraveled;
    private float m_remainPrepTime;
    private bool m_startPrep;

    private SpriteRenderer m_spriteRenderer;
    private PlaneState m_state;

    private Vector3 m_heading;
    private Rigidbody2D m_rb;
    protected PlaneGroup m_group;
    protected Airbase m_homeBase;
    //public Action<PlaneState> OnChangeState;

    protected Transform m_target;


    public Transform Target
    {
        get { return m_target; }
        set { m_target = value; }
    }


    public PlaneGroup Group { get { return m_group; } }
    public PlaneState State { get { return m_state; } protected set { m_state = value; } }
    public float Speed { get { return m_speed; } }

    public Vector3 Heading { get { return m_heading; } set { m_heading = value.normalized; } }
    public float TakeoffRollRatio { get { return 1 - m_remainPrepTime / m_preparationTime; } }
    protected virtual void Awake()
    {
        InstanceInit();

    }

    public void InstanceInit()
    {
        m_remainPrepTime = m_preparationTime;
        m_heading = Vector3.right;
        m_state = PlaneState.Grounded;
        //Debug.Log(m_remainPrepTime);
        m_rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();


        if (m_group == PlaneGroup.Nato)
        {
            m_spriteRenderer.color = Color.blue;
        }
        else if (m_group == PlaneGroup.WarsawPact)
        {
            m_spriteRenderer.color = Color.red;
        }

    }
    public void MovePlane(Vector3 toPosition)
    {
        m_rb.MovePosition(toPosition);
    }
    public void SetHomeBase(Airbase airbase)
    {
        m_homeBase = airbase;
    }
    protected virtual void FixedUpdate()
    {
        // Debug.Log(m_state);
        if (m_startPrep)
        {
            m_remainPrepTime -= Time.deltaTime;
            if (m_remainPrepTime <= 0 && m_state == PlaneState.Grounded)
            {
                m_state = PlaneState.Ready;
            }
        }
        if (m_state == PlaneState.OnTask || m_state == PlaneState.RTB)
        {
            transform.position += m_heading * m_speed * Time.deltaTime;
            m_distanceTraveled += m_speed * Time.deltaTime;
        }

        if (m_distanceTraveled > m_range && m_state != PlaneState.Destroyed)
        {
            PlaneShotDown();


        }
    }
    public virtual void TakeOff()
    {

        if (m_state == PlaneState.Ready)
        {
            //Debug.Log("Plane Take off");
            this.gameObject.SetActive(true);
            m_state = PlaneState.OnTask;

        }

    }
    public virtual void Land()
    {
        //Debug.Log("Plane landed");
        Assert.IsTrue(m_state == PlaneState.RTB || m_state == PlaneState.OnTask);

        m_startPrep = false;
        m_state = PlaneState.Grounded;
        m_heading = Vector3.right;
        m_distanceTraveled = 0;
        m_remainPrepTime = m_preparationTime;
        //this.gameObject.SetActive(false);
        //m_homeBase.LineUp(this);
    }

    public virtual void PlaneShotDown()
    {
        m_state = PlaneState.Destroyed;
        m_spriteRenderer.color = Color.gray;
    }

    public void StartPreparation()
    {
        if(m_state == PlaneState.Grounded)
            m_startPrep = true;
    }
}
