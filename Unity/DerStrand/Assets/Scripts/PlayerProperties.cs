using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PlayerProperties : MonoBehaviour
{
    [Header("Properties")] [SerializeField] private int maxProperty;
    [SerializeField] private int
        decreaseTime, health, thirst, hunger, 
        wearyTime, stamina, staminaRegenerationTime,
        staminaRegenerationAmount, thirstDecrese,
        hungerDecrease, wearyTimeDecrease, 
        staminaDecrease, healthDecrease;
    [SerializeField] private float 
        thirstDecreseInterval, hungerDecreaseInterval, 
        wearyTimeDecreaseInterval, staminaDecreaseInterval, 
        healthDecreaseInterval;
    [SerializeField] GameObject 
        healthBar, staminaBar, hungerBar, 
        thirstBar, wearyTimeBar;
    private Image 
        healthBarImage, staminaBarImage, hungerBarImage,
        thirstBarImage, wearyTimeBarImage;


    [SerializeField] private SaveData saveData;
    [SerializeField] public bool tempTrigger;
    

    public static PlayerProperties Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Save()
    {
        saveData.health = health;
        saveData.thirst = thirst;
        saveData.hunger = hunger;
        saveData.wearyTime = wearyTime;
    }

    private void Load()
    {
        health = saveData.health;
        thirst =saveData.thirst;
        hunger =saveData.hunger;
        wearyTime =saveData.wearyTime;
    }

    private void New()
    {
        health = maxProperty;       
        thirst = maxProperty;
        hunger = maxProperty;
        wearyTime = maxProperty;    //time to sleep
        stamina = maxProperty;      //says how long you can sprint (endurance/stamina)
    }


    // Start is called before the first frame update
    
    void Start()
    {
        saveData = SaveSystem.Instance.saveData;
        if(!SaveSystem.Instance.newGame)
            Load();
        else
            New();
        staminaRegenerationTime = 2;
        staminaRegenerationAmount = 1;

        //UI Load
        healthBarImage = healthBar.GetComponent<Image>();
        staminaBarImage = staminaBar.GetComponent<Image>();
        hungerBarImage = hungerBar.GetComponent<Image>();
        thirstBarImage = thirstBar.GetComponent<Image>();
        wearyTimeBarImage = wearyTimeBar.GetComponent<Image>();
        //Start Properties
        StartCoroutine(DecreaseThirst());
        StartCoroutine(DecreaseHunger());
        StartCoroutine(DecreaseWearyTime());
        

    }
    void Update()
    {
        UpdateUI();
        if (Player.Instance.isSprinting && !tempTrigger)
        {
            tempTrigger = true;
            StartCoroutine(DecreaseStamina());
        }
    }
    //---Deal-Damage---
    public void DealDamage(int damage)
    {
        if (health > 0)                     //if the health is more than 0 
        {
            Debug.Log("Damage");   
            health -= damage;     //apply the incoming damage to health
            Debug.Log(health);
        }
        else if(health <= 0)
        {
            //TODO:Death Event 
            Debug.Log("You are Dead");
        }
    }
    //---Regenerate---
    public void RegenerateHealth(int regenerateValue)
    {
        if (health <= maxProperty)                    //if health is under maxProperty
        {
            health += regenerateValue;    //apply the healing to the health
            if (health > maxProperty)               //if the health goes over the value of maxProperty 
            {
                health = maxProperty;               //health is set to default value
            }
        }
        //Debug.Log("new Health " + health);
      
    }
    public void RegenerateThirst(int regenerateValue)
    {
        if (health <= maxProperty)                    //if health is under maxProperty
        {
            health += regenerateValue;    //apply the healing to the health
            if (health > maxProperty)               //if the health goes over the value of maxProperty 
            {
                health = maxProperty;               //health is set to default value
            }
        }
        //Debug.Log("new thirst " + thirst);

    }
    public void RegenerateHunger(int regenerateValue)
    {
        if (health <= maxProperty)                    //if health is under maxProperty
        {
            health += regenerateValue;    //apply the healing to the health
            if (health > maxProperty)               //if the health goes over the value of maxProperty 
            {
                health = maxProperty;               //health is set to default value
            }
        }
        //Debug.Log("new hunger " + hunger);
       
    }
    public void RegenerateWearyTime(int regenerateValue)
    {
        if (health <= maxProperty)                    //if health is under maxProperty
        {
            health += regenerateValue;    //apply the healing to the health
            if (health > maxProperty)               //if the health goes over the value of maxProperty 
            {
                health = maxProperty;               //health is set to default value
            }
        }
        Debug.Log("new wearyTime " + wearyTime);
    }
    public IEnumerator RegenerateStamina()
    {
        Debug.Log(stamina);
        yield return new WaitForSeconds(staminaRegenerationTime);   //wait for staminaRegenerationTime seconds
        while (stamina <= maxProperty && stamina >= 0 && !Player.Instance.isSprinting)                               //While Stamina is < than maxProperty
        {
            stamina  += staminaRegenerationAmount;          //stamina + staminaRegenerationAmount
            yield return new WaitForSeconds(1);
            //Debug.Log(stamina);
        }
    }
    //---Regenerate-END---
    //---Decrease----
    private IEnumerator DecreaseThirst()
    {
        while (true)
        {
            if (thirst <= maxProperty && thirst > 0)                               //While Stamina is < than maxProperty
            {
                thirst -= thirstDecrese;          //stamina + staminaRegenerationAmount
                yield return new WaitForSeconds(thirstDecreseInterval);
                //Debug.Log("thirst :" + thirst);
            }
            if (thirst <= 0)
            {
                yield return new WaitForSeconds(healthDecreaseInterval);
                DecreaseHealth();
            }
        }
    }
    private IEnumerator DecreaseHunger()
    {
        while (true)
        {
            if (hunger <= maxProperty && hunger > 0)                               //While Stamina is < than maxProperty
            {
                hunger -= hungerDecrease;          //stamina + staminaRegenerationAmount
                yield return new WaitForSeconds(hungerDecreaseInterval);
                //Debug.Log("hunger :" + hunger);
            }
            if (hunger <= 0)
            {
                yield return new WaitForSeconds(healthDecreaseInterval);
                DecreaseHealth();
            }  
        }
    }
    private IEnumerator DecreaseWearyTime()
    {
        while (true)
        {
            if (wearyTime <= maxProperty && wearyTime > 0)                               //While Stamina is < than maxProperty
            {
                wearyTime -= wearyTimeDecrease;          //stamina + staminaRegenerationAmount
                yield return new WaitForSeconds(wearyTimeDecreaseInterval);
                //Debug.Log("wearyTime :" + wearyTime);
            }
            if (wearyTime <= 0)
            {
                yield return new WaitForSeconds(healthDecreaseInterval);
                DecreaseHealth();
            }
        }

    }
    private IEnumerator DecreaseStamina()
    {
        while (stamina <= maxProperty && stamina >= 0&& Player.Instance.isSprinting)                               //While Stamina is < than maxProperty
        {
            stamina -= staminaDecrease;          //stamina + staminaRegenerationAmount
            yield return new WaitForSeconds(staminaDecreaseInterval);
            //Debug.Log("stamina :" + stamina);
        }
    }
    private void DecreaseHealth()
    {
        health -= healthDecrease;
    }
    //---Decrease-END---
    //---UI-Integration---
    private void UpdateUI()
    {
        healthBarImage.fillAmount = (float)Math.Clamp(health, 0, maxProperty) / maxProperty;
        staminaBarImage.fillAmount = (float)Math.Clamp(stamina, 0, maxProperty) / maxProperty;
        hungerBarImage.fillAmount = (float)Math.Clamp(hunger, 0, maxProperty) / maxProperty;
        thirstBarImage.fillAmount = (float)Math.Clamp(thirst, 0, maxProperty) / maxProperty;
        wearyTimeBarImage.fillAmount = (float)Math.Clamp(wearyTime, 0, maxProperty) / maxProperty;
    }
    //---UI-Integration-END---
    //Call Methods
    //DealDamage(1);
    //RegenerateProperty("health",20);
    //RegenerateProperty("thirst",20);
    //RegenerateProperty("hunger",20);
    //RegenerateProperty("wearyTime",20);

    //Call IEnumerator
    //StartCoroutine(RegenerateStamina());
    //StartCoroutine(DecreaseThirst());
    //StartCoroutine(DecreaseHunger());
    //StartCoroutine(DecreaseWearyTime());
    //StartCoroutine(DecreaseStamina());
    //StartCoroutine(DecreaseHealth());
}