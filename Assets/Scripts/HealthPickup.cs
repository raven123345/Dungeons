using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;
    public float waitToBeCollected = .5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (waitToBeCollected > 0f)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0f && PlayerHealthController.instance.currentHealth < PlayerHealthController.instance.maxHealth)
        {
            PlayerHealthController.instance.healPlayer(healAmount);
            AudioManager.instance.PlaySFX(7);

            if (PlayerHealthController.instance.currentHealth > PlayerHealthController.instance.maxHealth)
            {
                PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
            }

            Destroy(gameObject);
        }
    }
}
