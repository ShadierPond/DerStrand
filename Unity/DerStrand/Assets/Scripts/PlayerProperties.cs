using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerProperties : MonoBehaviour
{
    [Header ("Propertie")]
    [SerializeField] int maxProperty, propertyData = 0,propertyCase = 0, health , thirst, hunger, wearyTime, stamina ,staminaRegenerationTime, staminaRegenerationAmount;
  

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
        //StartCoroutine(RegenerateStamina());
        health = 10;
        thirst = 10;
        hunger = 10;
        wearyTime = 10;

        RegenerateProperty(5, "health");
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
    public void RegenerateProperty(int regenerateValue,string propertyName)
    {
        Debug.Log(propertyData);
        switch (propertyName)
        {
            case "health":
                propertyData = health;
                Debug.Log("Health case");
                propertyCase = 1;
                break;
            case "thirst":
                propertyData = thirst;
                Debug.Log("thirst case");
                propertyCase = 2;
                break;
            case "hunger":
                propertyData = hunger;
                Debug.Log("hunger case");
                propertyCase = 3;
                break;
            case "wearyTime":
                propertyData = wearyTime;
                Debug.Log("wearyTime case");
                propertyCase = 4;
                break;
            default:
                propertyData = 0;
                propertyCase = 0;
                break;
        }
        if (propertyData < maxProperty)                    //if health is under maxProperty
        {
            propertyData += regenerateValue;    //apply the healing to the health
            if (propertyData > maxProperty)               //if the health goes over the value of maxProperty 
            {
                propertyData = maxProperty;               //health is set to default value
            }
            switch (propertyCase)
            {
                case 0:
                    Debug.Log("Error");
                    break;
                case 1:
                    Debug.Log("Regenerate health");
                    health = propertyData;
                    break;
                case 2:
                    Debug.Log("Regenerate thirst");
                    thirst = propertyData;
                    break;
                case 3:
                    Debug.Log("Regenerate hunger");
                    hunger = propertyData;
                    break;
                case 4:
                    Debug.Log("Regenerate wearyTime");
                    wearyTime = propertyData;
                    break;
                default :
                    Debug.Log("Nothing");
                    break;
            }
            Debug.Log("new Health " + health);
            Debug.Log("new thirst " + thirst);
            Debug.Log("new hunger " + hunger);
            Debug.Log("new wearyTime " + wearyTime);
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
    //RegenerateProperty(20,"health");
    //RegenerateProperty(20,"thirst");
    //RegenerateProperty(20,"hunger");
    //RegenerateProperty(20,"wearyTime");

    //Call IEnumerator
    //StartCoroutine(RegenerateStamina());
}
