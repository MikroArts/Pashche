using InventorySystem;
using Newtonsoft.Json;
using QuestSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class Player : MonoBehaviour
{
    public int level;
    public int experience;
    public int coin;
    public int health;
    public int hitDamage;
    public float attackSpeed;
    public Vector3 position;

    public Slider experienceFill;
    float minValue;
    public float maxValue;
    public Slider healthFill;
    public Text healthText;
    public Text xpText;
    public Text coinText;
    public Text levelText;

    TestBoolScript tbs;

    void Start()
    {      
        health = 100;
        experience = 0;
        level = 1;
        experienceFill.maxValue = 100;
        Time.timeScale = 1;
    }
    public void FillPlayerData(int xp, int coin)
    {
        experience += xp;
        CalculateXp();
        this.coin += coin;
    }

    public void CalculateXp()
    {
        minValue = (level - 1) * level * 100;
        maxValue = level * level * 100;
        if (experience >= maxValue)
        {
            level++;
            StartCoroutine("LevelUpTextInfo", 4);
            healthFill.maxValue = health += 10;
            minValue = maxValue;
            experienceFill.minValue = minValue;
            maxValue = level * level * 100;
            experienceFill.maxValue = maxValue;
        }
    }

    public void GetPosition(Vector3 currentPosition)
    {
        position = currentPosition;
    }
    public void Update()
    {
        GetPosition(transform.position);
        healthFill.value = health;
        if (health <= 0)
        {            
            GetComponent<Animator>().SetBool("IsDead", true);
        }
        experienceFill.value = experience;
        coinText.text = coin.ToString();
        levelText.text = "Lvl. " + level.ToString();
        healthText.text = health.ToString() + "/" + healthFill.maxValue.ToString();
        xpText.text = experience.ToString() + "/" + experienceFill.maxValue.ToString();        
    }
    IEnumerator LevelUpTextInfo()
    {
        Inventory.instance.pickUpText.GetComponentInChildren<Text>().text = "LEVEL UP!";
        Inventory.instance.pickUpText.GetComponentInChildren<Text>().fontSize = 22;
        Inventory.instance.pickUpText.SetActive(true);
        yield return new WaitForSeconds(4f);
        Inventory.instance.pickUpText.GetComponentInChildren<Text>().fontSize = 14;
        Inventory.instance.pickUpText.SetActive(false);
    }
}
