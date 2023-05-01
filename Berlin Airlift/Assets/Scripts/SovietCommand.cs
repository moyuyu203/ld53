using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
public class SovietCommand : Singleton<SovietCommand>
{
    [Serializable]
    private class PlaneTarget
    {
        public Plane Target;
        private Interceptor m_interceptor;

        
        public bool Neutralized
        {
            get
            {
                if (!(Target.State == PlaneState.OnTask))
                    return true;
                else
                    return false;
            }
        }
        public Interceptor Interceptor
        {
            get { 
                return m_interceptor; 
            }
            set{
                if (value)
                {
                    m_interceptor = value;
                    m_interceptor.OutOfFuel += HandleOOF;
                    //Do value oof;
                }
                else
                {
                    m_interceptor = value;
                }
            }
        }
        private void HandleOOF()
        {
            m_interceptor = null;
        }
    }

    private List<PlaneTarget> m_targetList = new List<PlaneTarget>();
    private List<InterceptorBase> m_bases = new List<InterceptorBase>();
    
    // Start is called before the first frame update
    void Start()
    {
        Radar[] radars = FindObjectsOfType<Radar>();
        foreach(Radar radar in radars)
        {
            radar.OnFoundTargets += HandleRadarDetection;
        }
        m_bases.AddRange(FindObjectsOfType<InterceptorBase>());
    }

    // Update is called once per frame
    void Update()
    {
        foreach(PlaneTarget target in m_targetList)
        {
            if(!target.Neutralized && target.Interceptor == null)
            {
                //To do order takeoff;
                foreach(InterceptorBase interceptorBase in m_bases)
                {
                    Interceptor interceptor = (Interceptor)interceptorBase.ReceiveOrder(target.Target.transform);
                    if (interceptor)
                    {
                        target.Interceptor = interceptor;
                        break;
                    }
                }
            }
            else if(target.Neutralized && target.Interceptor != null)
            {
                target.Interceptor.MissionComplete();
                target.Interceptor = null;
            }

            Assert.IsFalse(target.Neutralized && target.Interceptor != null);
            
        }
    }

    private void HandleRadarDetection(List<Transform> targets)
    {

        bool isNewTarget;
        foreach (Transform target in targets)
        {
            isNewTarget = true;
            foreach (PlaneTarget trackingTarget in m_targetList)
            {

                if (GameObject.ReferenceEquals(trackingTarget.Target.gameObject, target.gameObject))
                {
                    isNewTarget = false;
                }
            }
            if (isNewTarget)
            {
                PlaneTarget planeTarget = new PlaneTarget();
                planeTarget.Target = target.gameObject.GetComponent<Plane>();
                m_targetList.Add(planeTarget);

            }
        }

    }
}
