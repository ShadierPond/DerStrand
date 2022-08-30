using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerProperties : MonoBehaviour 
{
    [Header ("Propertie")]
    [SerializeField] int maxProperty, propertyData = 0,propertyCase = 0, decreaseTime, health , thirst, hunger, wearyTime, stamina ,staminaRegenerationTime, staminaRegenerationAmount;
    [SerializeField] private int thirstDecrese, hungerDecrease, wearyTimeDecrease, staminaDecrease;
    [SerializeField] private float thirstDecreseInterval, hungerDecreaseInterval, wearyTimeDecreaseInterval, staminaDecreaseInterval;
    [SerializeField] private SaveData saveData;
    
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
        //StartCoroutine(RegenerateStamina());
        //StartCoroutine(DecreaseThirst());
        //StartCoroutine(DecreaseHunger());
        //StartCoroutine(DecreaseWearyTime());
        //StartCoroutine(DecreaseStamina());
        //health = 10;
        //thirst = 10;
        //hunger = 10;
        //wearyTime = 10;

        //RegenerateProperty("health", 5);
        Debug.Log(health);
        
    }
    // Update is called once per frame
    void Update()
    {
        

    }
    public void DealDamage(int damage)
    {
        if (health>0)                     //if the health is more than 0 
        {
            Debug.Log("Damage");   
            health -= damage;     //apply the incoming damage to health
            Debug.Log(health);
        }
        else
        {
            if (health <= 0)
            { 
                //TODO:Death Event 
                Debug.Log("You are Dead");  
            }
        }
    }

    public void RegenerateHealth(int regenerateValue)
    {
        if (health < maxProperty)                    //if health is under maxProperty
        {
            health += regenerateValue;    //apply the healing to the health
            if (health > maxProperty)               //if the health goes over the value of maxProperty 
            {
                health = maxProperty;               //health is set to default value
            }
        }
        Debug.Log("new Health " + health);
      
    }
    public void RegenerateThirst(int regenerateValue)
    {
        if (health < maxProperty)                    //if health is under maxProperty
        {
            health += regenerateValue;    //apply the healing to the health
            if (health > maxProperty)               //if the health goes over the value of maxProperty 
            {
                health = maxProperty;               //health is set to default value
            }
        }
        Debug.Log("new thirst " + thirst);

    }
    public void RegenerateHunger(int regenerateValue)
    {
        if (health < maxProperty)                    //if health is under maxProperty
        {
            health += regenerateValue;    //apply the healing to the health
            if (health > maxProperty)               //if the health goes over the value of maxProperty 
            {
                health = maxProperty;               //health is set to default value
            }
        }
        Debug.Log("new hunger " + hunger);
       
    }
    public void RegenerateWearyTime(int regenerateValue)
    {
        if (health < maxProperty)                    //if health is under maxProperty
        {
            health += regenerateValue;    //apply the healing to the health
            if (health > maxProperty)               //if the health goes over the value of maxProperty 
            {
                health = maxProperty;               //health is set to default value
            }
        }
        Debug.Log("new wearyTime " + wearyTime);
    }
    private IEnumerator RegenerateStamina()
    {
        Debug.Log(stamina);
        yield return new WaitForSeconds(staminaRegenerationTime);   //wait for staminaRegenerationTime seconds
        while (stamina < maxProperty)                               //While Stamina is < than maxProperty
        {
            stamina  += staminaRegenerationAmount;          //stamina + staminaRegenerationAmount
            yield return new WaitForSeconds(1);
            Debug.Log(stamina);
        }
    }

    private IEnumerator DecreaseThirst()
    {
        while (thirst <= maxProperty)                               //While Stamina is < than maxProperty
        {
            thirst += thirstDecrese;          //stamina + staminaRegenerationAmount
            yield return new WaitForSeconds(thirstDecreseInterval);
            Debug.Log("thirst :" + thirst);
        }
    }
    private IEnumerator DecreaseHunger()
    {
        while (hunger <= maxProperty)                               //While Stamina is < than maxProperty
        {
            hunger += hungerDecrease;          //stamina + staminaRegenerationAmount
            yield return new WaitForSeconds(hungerDecreaseInterval);
            Debug.Log("hunger :" + hunger);
        }
    }
    private IEnumerator DecreaseWearyTime()
    {
        while (wearyTime <= maxProperty)                               //While Stamina is < than maxProperty
        {
            wearyTime += wearyTimeDecrease;          //stamina + staminaRegenerationAmount
            yield return new WaitForSeconds(wearyTimeDecreaseInterval);
            Debug.Log("wearyTime :" + wearyTime);
        }
    }
    private IEnumerator DecreaseStamina()
    {
        while (stamina <= maxProperty)                               //While Stamina is < than maxProperty
        {
            stamina += staminaDecrease;          //stamina + staminaRegenerationAmount
            yield return new WaitForSeconds(staminaDecreaseInterval);
            Debug.Log("stamina :" + stamina);
        }
    }
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
}