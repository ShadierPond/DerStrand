using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    [Header ("Propertie")]
    [SerializeField] int maxProperty, health ,  thirst, hunger, wearyTime, stamina ,staminaRegenerationTime, staminaRegenerationAmount;
  

    // Start is called before the first frame update
    void Start()
    {
        maxProperty = 100;  //Max value for all Properties
        health = maxProperty;       
        thirst = maxProperty;
        hunger = maxProperty;
        wearyTime = maxProperty;    //time to sleep
        stamina = 50;      //says how long you can sprint (endurance/stamina)
        staminaRegenerationTime = 2;
        staminaRegenerationAmount = 1;
        StartCoroutine(RegenerateStamina());
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
            health = health - damage;     //apply the incoming damage to health
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
    //TODO:Evt. make one method for multiple properties with a universal variable for regenerate property
    public void RegenerateHealth(int regeneratedHealth)
    {
        if (health <maxProperty)                    //if health is under maxProperty
        {
            health = health + regeneratedHealth;    //apply the healing to the health
            Debug.Log("Regenerate Health");
            if (health > maxProperty)               //if the health goes over the value of maxProperty 
            {
                health = maxProperty;               //health is set to default value
            }
        }
    }
    public void RegenerateThirst(int fillThirst)
    {
        if (thirst < maxProperty)
        {
            thirst = thirst + fillThirst;
            Debug.Log("Regenerate Thirst");
            if (thirst > maxProperty)
            {
                thirst = maxProperty;
            }
        }
    }
    public void RegenerateHunger(int fillHunger)
    {
        if (hunger < maxProperty)
        {
            hunger = hunger + fillHunger;
            Debug.Log("Regenerate Hunger");
            if (hunger > maxProperty)
            {
                hunger = maxProperty;
            }
        }
    }
    public void RegenerateWearyTime(int fillWearyTime)
    {
        if (wearyTime < maxProperty)
        {
            wearyTime = wearyTime + fillWearyTime;
            Debug.Log("Regenerate WearyTime");  //Sleep 
            if (wearyTime > maxProperty)
            {
                wearyTime = maxProperty;
            }
        }
    }
    //TODO:Evt. change private to public
    private IEnumerator RegenerateStamina()
    {
        Debug.Log(stamina);
        yield return new WaitForSeconds(staminaRegenerationTime);   //wait for staminaRegenerationTime seconds
        while (stamina < maxProperty)                               //While Stamina is < than maxProperty
        {
            stamina = stamina + staminaRegenerationAmount;          //stamina + staminaRegenerationAmount
            yield return new WaitForSeconds(1);
            Debug.Log(stamina);
        }
    }
    
    //Call Methods
    //DealDamage(1);
    //RegenerateHealth(20);
    //RegenerateThirst(20);
    //RegenerateHunger(20);
    //RegenerateWearyTime(20);

    //Call IEnumerator
    //StartCoroutine(RegenerateStamina());
}
