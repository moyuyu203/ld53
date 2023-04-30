using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct InterceptorCount
{
    public Interceptor plane;
    public int count;
}
public class InterceptorBase : Airbase
{
    //public List<Interceptor> InterceptorPrefabs = new List<Interceptor>();

    //private List<Interceptor> m_interceptors = new List<Interceptor>();

    //private Radar m_baseRadar;
    public List<InterceptorCount> PlaneInventory = new List<InterceptorCount>();

    public List<Radar> RadarStations = new List<Radar>();

    private List<Transform> m_trackingTargets = new List<Transform>();

    private void Awake()
    {
        foreach(InterceptorCount pCount in PlaneInventory)
        {
            for (int i = 0; i < pCount.count; i++)
            {
                Plane interceptor = Instantiate(pCount.plane);
                interceptor.SetHomeBase(this);
                LineUp(interceptor);
            }
        }
        Radar baseRadar = GetComponent<Radar>();
        RadarStations.Add(baseRadar);
    }

    private void Start()
    {
        //m_baseRadar.OnFoundTargets += HandleRadarDetection;
        foreach(Radar radar in RadarStations)
        {
            radar.OnFoundTargets += HandleRadarDetection;
        }
    }

    protected override void Update()
    {
        base.Update();
        for(int i = 0; i < m_trackingTargets.Count; i++)
        {
            Plane targetPlane = m_trackingTargets[i].gameObject.GetComponent<Plane>();
            if(targetPlane && targetPlane.State == PlaneState.Destroyed)
            {
                m_trackingTargets.RemoveAt(i);  
            }
        }
    }
    private void HandleRadarDetection(List<Transform> targets)
    {
       
        bool isNewTarget;
        foreach(Transform target in targets)
        {
            isNewTarget = true;
            foreach(Transform trackingTarget in m_trackingTargets)
            {
                
                if(GameObject.ReferenceEquals(trackingTarget.gameObject , target.gameObject))
                {
                    isNewTarget = false;
                }
            }
            if (isNewTarget && m_takeoffQueue.Count > 0 && RunwayClear)
            {
                //missionReadyPlanes[0].Target = target;
                //missionReadyPlanes[0].TakeOff();

                //missionReadyPlanes.RemoveAt(0);

                m_takeoffQueue[0].Target = target;
                m_trackingTargets.Add(target);
                OrderTakeoff();
            }




        }

    }

   

}
