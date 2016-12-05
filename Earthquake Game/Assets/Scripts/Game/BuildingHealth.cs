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

    // Update is called once per frame
    void Update()
    {
    }

    public void takeDamage(float amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;

    }
}
