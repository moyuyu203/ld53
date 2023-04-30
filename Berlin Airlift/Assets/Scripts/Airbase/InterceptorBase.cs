using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptorBase : MonoBehaviour
{
    public List<Interceptor> InterceptorPrefabs = new List<Interceptor>();

    private List<Interceptor> m_interceptors = new List<Interceptor>();
    private Radar m_baseRadar;

    private void Awake()
    {
        foreach(var interceptorPrefab in InterceptorPrefabs)
        {
            Interceptor instance = Instantiate(interceptorPrefab, transform.position, Quaternion.identity);
            instance.SetHomeBase(this);
            instance.gameObject.SetActive(false);
            m_interceptors.Add(instance);
        }
        m_baseRadar = GetComponentInChildren<Radar>();
    }

    private void Start()
    {
        m_baseRadar.OnFoundTargets += HandleRadarDetection;
    }

    private void HandleRadarDetection()
    {
        List<Interceptor> missionReadyPlanes = new List<Interceptor>();
        List<Interceptor> onTaskPlanes = new List<Interceptor>();
        foreach(Interceptor plane in m_interceptors)
        {
            if(plane.State == PlaneState.Ready)
            {
                missionReadyPlanes.Add(plane);
            }
            if(plane.State == PlaneState.OnTask)
            {
                onTaskPlanes.Add(plane);
            }
        }

        if(missionReadyPlanes.Count == 0)
        {
            return;
        }
        bool isNewTarget;
        foreach(Transform target in m_baseRadar.Targets)
        {
            isNewTarget = true;
            foreach(Interceptor onTaskPlane in onTaskPlanes)
            {
                Transform trackingTarget = onTaskPlane.Target;
                Debug.Log(trackingTarget.GetInstanceID());
                Debug.Log(target.GetInstanceID());

                if(GameObject.ReferenceEquals(trackingTarget.gameObject , target.gameObject))
                {
                    isNewTarget = false;
                }
            }
            if (isNewTarget && missionReadyPlanes.Count > 0)
            {
                missionReadyPlanes[0].Target = target;
                missionReadyPlanes[0].TakeOff();

                missionReadyPlanes.RemoveAt(0);
            }




        }

    }

}
