using System;
using UnityEngine;

// Inherit to create a single, global-accessible instance of a class, available at all times
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    public static bool shouldDestroy = false;

    public static T Instance
    {
        get
        {
            if (_instance == null && !shouldDestroy)
            {
                _instance = GameObject.FindObjectOfType<T>();
                if (_instance == null)
                {
                    var singletonObj = new GameObject();
                    singletonObj.name = typeof(T).ToString();
                    _instance = singletonObj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            shouldDestroy = false;
            return;
        }

        _instance = GetComponent<T>();

        if (_instance == null)
        {
            shouldDestroy = true;
            return;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            shouldDestroy = true;
        }
    }
}