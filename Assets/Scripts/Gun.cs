using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletToFire;
    public Transform firePoint;

    public float timeBetweenShots;
    private float shotsCounter;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPaused)
        {
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
        }
    }
}
