using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;

    public float rangeToChasePlayer;
    private Vector3 moveDir;

    [Header("Chase Player")]
    public bool shouldChasePlayer;

    #region COWARD
    [Header("Runaway")]
    public bool shouldRunAway;
    public float runawayRange;
    #endregion

    #region BLOB
    [Header("Wandering")]
    public bool shouldWander;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDir;
    #endregion

    #region Fire
    [Header("Partolling")]
    public bool shouldPartol;
    public Transform[] partolPoints;
    private int curPatrolPoint;
    #endregion

    [Header("Drop Items")]
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    [Header("Shooting")]
    public bool shouldShoot;

    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;

    public float shootRange;

    [Header("Variables")]
    public SpriteRenderer spr;
    public Animator anim;

    public int health = 150;

    public GameObject[] deathSplatter;
    public GameObject enemyDamageEffect;

    void Start()
    {
        if (shouldWander)
        {

            pauseCounter = Random.Range(0f, pauseLength * 1.25f);
        }
    }


    void Update()
    {
        if (spr.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDir = Vector3.zero;
            //Moving
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChasePlayer)
            {
                moveDir = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                if (shouldWander)
                {
                    if (wanderCounter > 0f)
                    {
                        wanderCounter -= Time.deltaTime;

                        //move the enemy
                        moveDir = wanderDir;

                        if (wanderCounter <= 0f)
                        {
                            pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
                        }
                    }
                    if (pauseCounter > 0f)
                    {
                        pauseCounter -= Time.deltaTime;
                        if (pauseCounter <= 0f)
                        {
                            wanderCounter = Random.Range(wanderLength * 0.75f, wanderLength * 1.25f);
                            wanderDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), .0f);
                        }
                    }
                }

                if (shouldPartol)
                {
                    moveDir = partolPoints[curPatrolPoint].position - transform.position;

                    if (Vector3.Distance(transform.position, partolPoints[curPatrolPoint].position) < 0.2f)
                    {
                        curPatrolPoint++;
                        if (curPatrolPoint >= partolPoints.Length)
                        {
                            curPatrolPoint = 0;
                        }
                    }
                }
            }



            if (shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runawayRange)
            {
                moveDir = transform.position - PlayerController.instance.transform.position;
            }


            //else
            //{
            //    moveDir = Vector3.zero;
            //}

            moveDir.Normalize();
            rb.velocity = moveDir * moveSpeed;

            if (moveDir != Vector3.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }

            //Shooting
            if (shouldShoot && Vector3.Distance(gameObject.transform.position, PlayerController.instance.transform.position) <= shootRange)
            {
                fireCounter -= Time.deltaTime;
                if (fireCounter <= 0.0f)
                {
                    fireCounter = fireRate;
                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                }
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            anim.SetBool("isMoving", false);
        }
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;

        Instantiate(enemyDamageEffect, transform.position, transform.rotation);

        if (health <= 0)
        {
            Destroy(gameObject);

            int i = Random.Range(0, deathSplatter.Length);
            int rotation = Random.Range(0, 4);

            Instantiate(deathSplatter[i], transform.position, Quaternion.Euler(0f, 0f, 90f * rotation));
            AudioManager.instance.PlaySFX(1);

            if (shouldDropItem)
            {
                float dropChanse = Random.Range(0f, 100f);

                if (dropChanse < itemDropPercent)
                {
                    int itemsIndex = Random.Range(0, itemsToDrop.Length);
                    Instantiate(itemsToDrop[itemsIndex], transform.position, transform.rotation);
                }
            }
        }
    }
}
