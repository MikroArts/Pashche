using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoolScript : MonoBehaviour
{
    public static TestBoolScript instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool isLoad;

    void Start()
    {       
        isLoad = false;
    }
}
