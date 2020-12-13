using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    public GameObject impactEffect;
    public int damage;

    void Start()
    {
        direction = PlayerController.instance.transform.position - transform.position;
        direction.Normalize();
        AudioManager.instance.PlaySFX(13);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.DamagePlayer();
            PlayerHealthController.instance.DamagePlayer(damage);
        }

        Destroy(gameObject);
    }
      
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
