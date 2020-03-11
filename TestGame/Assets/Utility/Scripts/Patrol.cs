using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    NavMeshAgent agent;
    public float waitTime;
    public float startWaitTime;
    public Transform[] moveSpots;
    private int randomSpot;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
        GetComponent<Animator>().SetBool("IsWalking", true);
    }

    void Update()
    {
        
        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < .5f)
        {
            if (waitTime <= 0)
            {                
                randomSpot = Random.Range(0, moveSpots.Length);
                GetComponent<Animator>().SetBool("IsWalking", true);
                waitTime = startWaitTime;
            }
            else
            {
                GetComponent<Animator>().SetBool("IsWalking", false);
                waitTime -= Time.deltaTime;
            }
        }
        agent.SetDestination(moveSpots[randomSpot].position);
    }
}
