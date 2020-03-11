using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject cam;
    public Transform player;
    public Image playerIcon;    

    //mouse related
    public Texture2D cursor;

    //panels
    public GameObject map;
    public GameObject mainMenu;
    public GameObject inventoryPanel;
    public GameObject activeQuestPanel;
    public GameObject travelPanel;
    private bool active = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        
        EnableOrDisableMainCamera();
        ShowMap();
        ShowInventory();
        ShowActiveQuests();
        SetCursorVisibility();


        if (Input.GetButtonDown("Cancel"))
        {
            CloseActivePanelsFirst(map, inventoryPanel, activeQuestPanel, travelPanel);
            if (active)
                return;
            ShowMainMenu();
            if (GameObject.Find("OptionsPanel"))
                GameObject.Find("OptionsPanel").SetActive(false);            
        }
        
    }

    private void CloseActivePanelsFirst(GameObject map, GameObject inventory, GameObject activeQuestPanel, GameObject travelPanel)
    {
        if (map.activeSelf)
        {
            map.SetActive(false);
            Time.timeScale = 1f;
            active = true;
        }
        else if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
            Time.timeScale = 1f;
            active = true;
        }
        else if (activeQuestPanel.activeSelf)
        {
            activeQuestPanel.SetActive(false);
            Time.timeScale = 1f;
            active = true;
        }
        else if (travelPanel.activeSelf)
        {
            travelPanel.SetActive(false);
            Time.timeScale = 1f;
            active = true;
        }
        else
        {
            Time.timeScale = 0f;
            active = false;
        }
    }

    private void EnableOrDisableMainCamera()
    {
        if (Time.timeScale == 0)
        {
            cam.GetComponent<ThirdPersonCamera>().enabled = false;
        }
        else
        {
            cam.GetComponent<ThirdPersonCamera>().enabled = true;
        }
    }
    private static void SetCursorVisibility()
    {
        if (Time.timeScale == 1f)
        {            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void ShowMap()
    {
        if (Input.GetButtonDown("Map"))
        {
            if (mainMenu.activeSelf)
                return;
            if (map.activeSelf)
            {
                map.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                map.SetActive(true);
                Time.timeScale = 0f;
            }
            float x = player.position.x;
            float y = player.position.z;

            playerIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y);
            playerIcon.transform.rotation = new Quaternion(0, 0, -player.rotation.y, player.rotation.w);
        }
    }
    public void ShowMainMenu()
    {        
        if (mainMenu.activeSelf)
        {            
            if (GameObject.Find("OptionsPanel") && Input.GetKey(KeyCode.Escape))
                return;

            mainMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {            
            mainMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    private void ShowInventory()
    {
        if (mainMenu.activeSelf)
            return;
        if (Input.GetButtonDown("Inventory"))
        {
            if (inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                inventoryPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }

    }
    private void ShowActiveQuests()
    {
        if (mainMenu.activeSelf)
            return;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (activeQuestPanel.activeSelf)
            {
                activeQuestPanel.SetActive(false);
                Time.timeScale = 1f;
            }
            else if(!activeQuestPanel.activeSelf)
            {
                activeQuestPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }    

}
