using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
[SerializeField] GameObject healthBar;
private Image healthBarImage;
[SerializeField] private int health;
[SerializeField] private int maxHealth;


[SerializeField] GameObject staminaBar;
[SerializeField] GameObject hungerBar;
[SerializeField] GameObject thirstBar;
[SerializeField] GameObject wearyTimeBar;

    private void Start()
    {
        healthBarImage = healthBar.GetComponent<Image>();
    }

    private void UpdateUI()
    {
        healthBarImage.fillAmount = (float)Math.Clamp(health, 0, maxHealth) / maxHealth;
    }

    private void Update()
    {
        UpdateUI();
    }
}
