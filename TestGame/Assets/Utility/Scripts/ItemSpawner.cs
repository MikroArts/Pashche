using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    public List<GameObject> listOfObjects;
    public Transform player;
    Vector3 offset;
    float a,b;
    void Start()
    {
        
    }
    public void SpawnItem(string item)
    {        
        foreach (GameObject go in listOfObjects)
        {
            a = Random.Range(-1f, 1f);
            b = Random.Range(-1f, 1f);
            offset = new Vector3(a, 1, b);
            if (go.name == item.ToLower())
            {
                GameObject newObject = Instantiate(go,player.root.position + offset, go.transform.rotation);
                newObject.name = newObject.name.Replace("(Clone)", "");
                return;
            }
        }       
    }
    public GameObject SpanGameObject(string objectToInstantiate)
    {
        foreach (GameObject go in listOfObjects)
        {
            if (go.name == objectToInstantiate.ToLower())
            {
                GameObject newObject = Instantiate(go);
                newObject.name = newObject.name.Replace("(Clone)", "");
                return newObject;
            }            
        }
        return null;
    }
}
