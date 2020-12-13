using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int currentHealth;
    public int maxHealth;

    public float damageInvincLength = 1f;
    private float invinceCount;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (invinceCount > 0.0f)
        {
            invinceCount -= Time.deltaTime;
        }
        if (invinceCount <= 0f)
        {
            PlayerController.instance.body.color = new Color(PlayerController.instance.body.color.r, PlayerController.instance.body.color.g, PlayerController.instance.body.color.b, 1f);
        }
    }

    public void DamagePlayer(int damage)
    {
        if (invinceCount <= 0f)
        {
            currentHealth -= damage;
            makeInvicible(damageInvincLength);
            AudioManager.instance.PlaySFX(11);

            //invinceCount = damageInvincLength;
            //PlayerController.instance.body.color = new Color(PlayerController.instance.body.color.r, PlayerController.instance.body.color.g, PlayerController.instance.body.color.b, 0.5f);

            if (currentHealth <= 0)
            {
                PlayerController.instance.gameObject.SetActive(false);
                UIController.instance.deathScreen.SetActive(true);
                AudioManager.instance.PlaySFX(10);
                AudioManager.instance.PlayGameOver();

            }

            UIController.instance.healthSlider.maxValue = maxHealth;
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }

    }

    public void makeInvicible(float invinceLength)
    {
        invinceCount = invinceLength;
        PlayerController.instance.body.color = new Color(PlayerController.instance.body.color.r, PlayerController.instance.body.color.g, PlayerController.instance.body.color.b, 0.5f);
    }

    public void healPlayer(int healAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        } 
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;


        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
