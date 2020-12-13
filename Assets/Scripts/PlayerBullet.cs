﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    public float speed = 7.5f;
    public Rigidbody2D rb;
    public GameObject inpactEffect;

    public int damageToGive = 50;

    void Start()
    {
        //bulletLifeCountDown = bulletLife;
        AudioManager.instance.PlaySFX(12);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * speed;


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
            AudioManager.instance.PlaySFX(2);
        }
        else
        {
            Instantiate(inpactEffect, transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(4);
        }

        Destroy(gameObject);

    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
