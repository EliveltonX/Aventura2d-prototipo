using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public int MaxHealth = 100;
    
    public float launchForce = 3f;
    public int currentHealth;

    public Image healthFiller;
    public MenusController mnControl;

    private void Update()
    {
        float health = (currentHealth * 100) / MaxHealth;
        healthFiller.fillAmount = health/100;


        if (currentHealth <= 0) 
        {
            mnControl.DieMenuShower();
           
            
        }
    }

}
