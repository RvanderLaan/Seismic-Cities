using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingHealth : MonoBehaviour {
    public float startingHealth = 100;
    public float currentHealth;
    public Slider healthSlider;

    // Use this for initialization
    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    public void takeDamage(float amount)
    {
        currentHealth = (currentHealth - amount < 0)? 0 : currentHealth - amount;
        healthSlider.value = currentHealth;
    }

    public float getHealthPercentage()
    {
        return currentHealth / startingHealth;
    }

    public float getDamagePercentage()
    {
        return 1 - getHealthPercentage();
    }
}
