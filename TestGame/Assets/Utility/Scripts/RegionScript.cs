using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RegionScript : MonoBehaviour
{
   
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            GameObject.Find("Region_Name").GetComponent<Text>().text = transform.name;
        }
        
    }
    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            GameObject.Find("Region_Name").GetComponent<Text>().text = "";
        }

    }
}
