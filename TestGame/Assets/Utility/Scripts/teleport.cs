using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
   
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.transform.position = new Vector3(449.272f, 45.08f, 56.75f);
        }
        
    }
}
