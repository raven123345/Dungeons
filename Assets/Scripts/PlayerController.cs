using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;


    public float moveSpeed;
    private Vector2 moveDir;

    public Rigidbody2D rb;

    public Transform gunArm;

    private Camera cam;

    public Animator anim;

    public GameObject bulletToFire;
    public Transform firePoint;

    public GameObject damageEffect;

    public float timeBetweenShots;
    private float shotsCounter;

    public SpriteRenderer body;

    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashlength = .5f, dashCooldown = 1f, dashInvinsibility = .5f;
    private float dashCoolCounter;

    [HideInInspector]
    public float dashCounter;

    [HideInInspector]
    public bool canMove = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
        activeMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {
            moveDir.x = Input.GetAxisRaw("Horizontal");
            moveDir.y = Input.GetAxisRaw("Vertical");
            moveDir.Normalize();


            rb.velocity = moveDir * activeMoveSpeed;


            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = cam.WorldToScreenPoint(transform.localPosition);

            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunArm.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                gunArm.localScale = Vector3.one;
            }

            //Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            Vector3 offset = mousePos - screenPoint;
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            gunArm.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                shotsCounter = timeBetweenShots;
            }

            if (Input.GetMouseButton(0))
            {
                shotsCounter -= Time.deltaTime;
                if (shotsCounter <= 0f)
                {
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    shotsCounter = timeBetweenShots;
                }
            }



            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashCounter <= 0f && dashCoolCounter <= 0f)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashlength;
                    anim.SetTrigger("dash");

                    PlayerHealthController.instance.makeInvicible(dashInvinsibility);
                }
            }
            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0f)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                    AudioManager.instance.PlaySFX(8);
                }
            }
            if (dashCooldown > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }



            if (moveDir != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }

    }

    public void DamagePlayer()
    {
        Instantiate(damageEffect, transform.position, transform.rotation);
    }
}
