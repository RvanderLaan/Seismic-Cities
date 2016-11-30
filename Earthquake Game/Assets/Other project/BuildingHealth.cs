using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingHealth : MonoBehaviour {
    public int startingHealth = 100;
    public int currentHealth;
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

    public void takeDamage(int amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;

    }
}
