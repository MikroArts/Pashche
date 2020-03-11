using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isOpen;
    public GameObject chest;
    public Animator animator;
    void Start()
    {
        isOpen = false;
        chest = gameObject;
        animator = chest.GetComponent<Animator>();
    }

    //void Update()
    //{
    //    OpenCloseChest(isOpen);
    //}
    
    public void OpenCloseChest(bool is_Open)
    {
        if (Input.GetButtonDown("Action") && GameObject.Find("Player").GetComponent<RayCastPickUp>().ColliderName == name)
        {
            if (is_Open)
            {
                animator.SetBool("isOpen", false);
                isOpen = false;
            }
            else
            {
                animator.SetBool("isOpen", true);
                isOpen = true;
            }
        }       
    }
}
