using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TimeSlot
{
    public int timeSlot;
    public GameObject plane;
}
public class NatoAirbase : Waypoint
{
    [SerializeField] private List<TimeSlot> TakeOffTimeTable = new List<TimeSlot>();


    private void Start()
    {
        GlobalTimer.Instance.OnChangeTimetable += OrderTakeOff;
        ProcessTakeOffOrder();
    }

    private void ProcessTakeOffOrder()
    {
        for(int i = 0; i < TakeOffTimeTable.Count; i++)
        {
            TimeSlot slot = TakeOffTimeTable[i];
            slot.timeSlot = i;
            if(slot.plane)
                slot.plane = Instantiate(slot.plane, transform.position, Quaternion.identity);

            TakeOffTimeTable[i] = slot;
        }
    }
    protected void OrderTakeOff(int timeSlot)
    {
        if(timeSlot < TakeOffTimeTable.Count && TakeOffTimeTable[timeSlot].plane != null)
        {
            TakeOffTimeTable[timeSlot].plane.GetComponent<Plane>().TakeOff();
        }
    }
}
