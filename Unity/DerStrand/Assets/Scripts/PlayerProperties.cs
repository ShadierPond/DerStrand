using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    [Header ("Properties")]
    [SerializeField] int health, maxProperty, stamina, thurst, hunger, wearyTime;


    // Start is called before the first frame update
    void Start()
    {
        maxProperty = 100;
        health = 100;
        stamina = 100;
        thurst = 100;
        hunger = 100;
        wearyTime = 100;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void DealDamage(int damage)
    {
        if (health>0)
        {
            health = health - damage;
        }
        else
        {
            if (health <= 0)
            {
//TODO:Death Event
            }
        }
    }
}
