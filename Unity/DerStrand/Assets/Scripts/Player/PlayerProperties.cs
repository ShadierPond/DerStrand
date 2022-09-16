using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PlayerProperties : MonoBehaviour
{
    [Header("Properties")] [SerializeField] private int maxProperty;
    [SerializeField] private int
        decreaseTime, health, thirst, hunger, 
        wearyTime, staminaRegenerationAmount, 
        healthRegenerationAmaunt,thirstDecrese ,
        hungerDecrease, wearyTimeDecrease, 
        staminaDecrease, healthDecrease ;
    [SerializeField] public int stamina;
    [SerializeField] private float 
        thirstDecreseInterval, hungerDecreaseInterval, 
        wearyTimeDecreaseInterval, staminaDecreaseInterval, 
        staminaRegenerationTime, healthDecreaseInterval;
    [SerializeField] GameObject 
        healthBar, staminaBar, hungerBar, 
        thirstBar, wearyTimeBar;
    private Image 
        healthBarImage, staminaBarImage, hungerBarImage,
        thirstBarImage, wearyTimeBarImage;
    public bool staminaFull;
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
    void Start()
    {
        saveData = SaveSystem.Instance.saveData;
        if(!SaveSystem.Instance.newGame)Load();
        else New();
        //UI Load
        healthBarImage = healthBar.GetComponent<Image>();           //get the Image component
        staminaBarImage = staminaBar.GetComponent<Image>();         
        hungerBarImage = hungerBar.GetComponent<Image>();           
        thirstBarImage = thirstBar.GetComponent<Image>();           
        wearyTimeBarImage = wearyTimeBar.GetComponent<Image>();     
        //Start Properties
        StartCoroutine(DecreaseThirst());                     //start the decrease from thirst
        StartCoroutine(DecreaseHunger());                     //start the decrease from hunger
        StartCoroutine(DecreaseWearyTime());                  //start the decrease from wearyTime
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
            health -= damage;               //apply the incoming damage to health
            Debug.Log(health);
        }
        else if(health <= 0)
        {
            //TODO:Death Event 
            Debug.Log("You are Dead");
            health = 0;
        }
    }
    //---Regenerate---
    public void RegenerateHealth(int regenerateValue)
    {
        if (health <= maxProperty)                                //if health is under maxProperty
        {
            health += regenerateValue;                            //apply the healing to the health
            if (health > maxProperty)health = maxProperty;        //if the health goes over the value of maxProperty
        }
        //Debug.Log("new Health " + health);
    }
    public void RegenerateThirst(int regenerateValue)
    {
        if (thirst <= maxProperty)                                //if health is under maxProperty
        {
            thirst += regenerateValue;                            //apply the healing to the health
            if (thirst > maxProperty)thirst = maxProperty;        //if the thirst goes over the value of maxProperty thirst is set to maxProperty
        }
        if (thirst > maxProperty) thirst = maxProperty;
        //Debug.Log("new thirst " + thirst);
    }
    public void RegenerateHunger(int regenerateValue)
    {
        if (hunger <= maxProperty)                                //if health is under maxProperty
        {
            hunger += regenerateValue;                            //apply the healing to the health
            if (hunger > maxProperty)hunger = maxProperty;        //if the hunger goes over the value of maxProperty hunger is set to maxProperty
        }
        if (hunger > maxProperty) hunger = maxProperty;
        //Debug.Log("new hunger " + hunger);
    }
    public void RegenerateWearyTime(int regenerateValue)
    {
        if (wearyTime <= maxProperty)                             //if health is under maxProperty
        {
            wearyTime += regenerateValue;                         //apply the healing to the health
            if (wearyTime > maxProperty)wearyTime = maxProperty;  //if the wearyTime goes over the value of maxProperty the wearyTime is set to maxProperty
        }
        if (wearyTime > maxProperty) wearyTime = maxProperty;     //that the value does not expand over the Max property
        //Debug.Log("new wearyTime " + wearyTime);
    }
    public IEnumerator RegenerateStamina()
    {
        Debug.Log(stamina);
        yield return new WaitForSeconds(staminaRegenerationTime);       //wait for staminaRegenerationTime seconds
        while (stamina <= maxProperty && !Player.Instance.isSprinting)  //While Stamina is < than maxProperty
        {
            stamina  += staminaRegenerationAmount;                      //stamina + staminaRegenerationAmount
            hunger -= hungerDecrease;                                   //if you sprint you louse more hunger
            thirst -= thirstDecrese;                                    //if you sprint you louse more thirst
            yield return new WaitForSeconds(1);
            //Debug.Log(stamina);
        }
        if (stamina > maxProperty)stamina = maxProperty;
    }
    //---Regenerate-END---
    //---Decrease----
    private IEnumerator DecreaseThirst()
    {
        while (true)
        {
            if (thirst <= maxProperty && thirst > 0)                               //While Stamina is < than maxProperty
            {
                thirst -= thirstDecrese;          //thirst - thirstDecrease
                yield return new WaitForSeconds(thirstDecreseInterval);
                //Debug.Log("thirst :" + thirst);
            }
            if (thirst >= 50) RegenerateHealth(healthRegenerationAmaunt);
            if (thirst <= 0)
            {
                yield return new WaitForSeconds(healthDecreaseInterval);
                DecreaseHealth();
                thirst = 0;
            }
        }
    }
    private IEnumerator DecreaseHunger()
    {
        while (true)
        {
            if (hunger <= maxProperty && hunger > 0)                               //While Stamina is < than maxProperty
            {
                hunger -= hungerDecrease;          
                yield return new WaitForSeconds(hungerDecreaseInterval);
                //Debug.Log("hunger :" + hunger);
            }
            if (thirst >= 50) RegenerateHealth(healthRegenerationAmaunt);
            if (hunger <= 0)
            {
                yield return new WaitForSeconds(healthDecreaseInterval);
                DecreaseHealth();
                hunger = 0;
            }  
        }
    }
    private IEnumerator DecreaseWearyTime()
    {
        while (true)
        {
            if (wearyTime <= maxProperty && wearyTime > 0)  //While Stamina is < than maxProperty
            {
                wearyTime -= wearyTimeDecrease;         
                yield return new WaitForSeconds(wearyTimeDecreaseInterval);
                //Debug.Log("wearyTime :" + wearyTime);
            }
            if (wearyTime <= 0)
            {
                yield return new WaitForSeconds(healthDecreaseInterval);
                DecreaseHealth();
                wearyTime = 0;
            }
        }

    }
    private IEnumerator DecreaseStamina()
    {
        while (stamina <= maxProperty && stamina >= 0 && Player.Instance.isSprinting)   //While Stamina is < than maxProperty
        {
            stamina -= staminaDecrease;                                                 //stamina + staminaRegenerationAmount
            yield return new WaitForSeconds(staminaDecreaseInterval);
            //Debug.Log("stamina :" + stamina);
        }
        if (stamina <= 0)stamina = 0;
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