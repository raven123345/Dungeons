using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed;

    public Transform target;

    public Camera mainCamera, bigMapCamera;

    private bool bigMapActive;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("BigMap"))
        {
            if (!bigMapActive)
            {
                ActivateBigMap();
            }
            else
            {
                DeactivateBigmap();
            }
        }
    }

    public void setTarget(Transform target)
    {
        this.target = target;
    }

    public void ActivateBigMap()
    {
        if (!LevelManager.instance.isPaused)
        {
            bigMapActive = true;

            bigMapCamera.enabled = true;
            mainCamera.enabled = false;
            UIController.instance.mapDisplay.SetActive(false);
            UIController.instance.bigMapText.SetActive(true);

            PlayerController.instance.canMove = false;

            Time.timeScale = 0f;
        }
    }

    public void DeactivateBigmap()
    {
        if (!LevelManager.instance.isPaused)
        {
            bigMapActive = false;

            bigMapCamera.enabled = false;
            mainCamera.enabled = true;
            UIController.instance.mapDisplay.SetActive(true);
            UIController.instance.bigMapText.SetActive(false);

            PlayerController.instance.canMove = true;

            Time.timeScale = 1f;
        }
    }
}
