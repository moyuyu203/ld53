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

    private LineRenderer m_lineRenderer;
    public Transform Target
    {
        get { return m_target; }
        set { m_target = value; }
    }


    public PlaneGroup Group { get { return m_group; } }
    public PlaneState State { get { return m_state; } protected set { m_state = value; } }
    public float Speed { get { return m_speed; } }

    public Vector3 Heading { 
        get { 
            return m_heading; 
        } 
        set { 
            Vector3 v = new Vector3(value.x, value.y, 0);
            m_heading = v.normalized;
        } }
    public float TakeoffRollRatio { get { return 1 - m_remainPrepTime / m_preparationTime; } }

    public float RemainFuel { get { return 1 - m_distanceTraveled / m_range; } }
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

        m_lineRenderer = GetComponent<LineRenderer>();
        if (m_lineRenderer)
        {
            m_lineRenderer.startWidth = 0.03f;
            m_lineRenderer.endWidth = 0.03f;
            m_lineRenderer.startColor = UnityEngine.Color.yellow;
            m_lineRenderer.endColor = UnityEngine.Color.yellow;
            m_lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            m_lineRenderer.enabled = false;
            m_lineRenderer.sortingLayerName = "Middle";
        }
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

        //Handle Movement
        if(m_state == PlaneState.RTB)
        {
            if(m_lineRenderer)
                m_lineRenderer.enabled = false;

            Heading = (m_homeBase.transform.position - transform.position).normalized;
            Vector3 toPosition = transform.position + Heading * Speed * Time.deltaTime;
            m_distanceTraveled += m_speed * Time.deltaTime;
            MovePlane(toPosition);
        }
        else if(m_state == PlaneState.OnTask)
        {
            if (Target)
            {
                Heading = (Target.position - transform.position).normalized;
                Vector3 toPosition = transform.position + Heading * Speed * Time.deltaTime;
                m_distanceTraveled += m_speed * Time.deltaTime;
                MovePlane(toPosition);
                if((Target.position - transform.position).magnitude < 0.05f)
                {
                    //Target Reached.
                    m_lineRenderer.enabled = false;
                    Target = null;
                }

                //Debug.Log("Draw Line");
                if (m_lineRenderer)
                {
                    m_lineRenderer.enabled = true;
                    m_lineRenderer.positionCount = 2;
                    m_lineRenderer.SetPosition(0, transform.position);
                    m_lineRenderer.SetPosition(1, Target.position);
                }
            }
            else
            {
             
                Vector3 toPosition = transform.position + Heading * Speed * Time.deltaTime;
                m_distanceTraveled += m_speed * Time.deltaTime;
                MovePlane(toPosition);
            }
        }
        
        /*
        if (m_state == PlaneState.OnTask || m_state == PlaneState.RTB)
        {
            transform.position += m_heading * m_speed * Time.deltaTime;
            m_distanceTraveled += m_speed * Time.deltaTime;
        }
        */


        //Out of Range fall
        if (m_distanceTraveled > m_range && m_state != PlaneState.Destroyed)
        {
            PlaneShotDown();
        }

        //Draw line
        /*
        if(m_lineRenderer && m_target && State == PlaneState.OnTask)
        {
            //Draw line
            Debug.Log("Draw Line");
            m_lineRenderer.enabled = true;
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(0, transform.position);
            m_lineRenderer.SetPosition(1, Target.position);
        }
        */
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
        if (m_lineRenderer)
        {
            m_lineRenderer.enabled = false;
        }
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
        if (m_lineRenderer)
        {
            m_lineRenderer.enabled = false;
        }
        m_state = PlaneState.Destroyed;
        m_spriteRenderer.color = Color.gray;
        if(m_group == PlaneGroup.Nato)
        {
            USAFCommand.Instance.TotalLostPlanes++;
        }
    }

    public void StartPreparation()
    {
        if(m_state == PlaneState.Grounded)
            m_startPrep = true;
    }
}
