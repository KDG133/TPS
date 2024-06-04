using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = default;
    public static bool Hasinstance => instance != null;
    public static T TryGetInstance() => Hasinstance ? instance : null;
    public static T Current => instance;
    
    public static T Instance
    {
        get 
        { 
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    protected virtual void InitializeSingleton()
    {
        if(!Application.isPlaying) { return; }
        instance = this as T;
    }
}
