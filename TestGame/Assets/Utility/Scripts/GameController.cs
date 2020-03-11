using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using QuestSystem;
using System.IO;
using Newtonsoft.Json;
using System;


//TODO: Implement Trading System
public class GameController : MonoBehaviour
{

    public static GameController instance;
    public static GameController Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(GameController)) as GameController;
            }
            return instance;
        }
    }
            
}
