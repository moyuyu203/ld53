using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : Singleton<GlobalTimer>
{
    [SerializeField] private float TimeSlotPerSecond;
    private float m_currTime;
    private int m_currtimeSlot;
    private bool m_started;

    public Action<int> OnChangeTimetable;

    private void Start()
    {
        StartGlobalTimer();
    }
    public void StartGlobalTimer()
    {
        m_currTime = 0;
        m_currtimeSlot = 0;
        m_started = true;

    }

    private void FixedUpdate()
    {
        m_currTime += Time.deltaTime; 
        if (m_started)
        {
            if(m_currTime > (m_currtimeSlot + 1)* TimeSlotPerSecond)
            {
                m_currtimeSlot++;
                OnChangeTimetable?.Invoke(m_currtimeSlot);
            }
        }
    }

}
