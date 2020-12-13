using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public float waitForAnyKey = 2f;
    public GameObject anyKeytext;
    public string mainMenuScene;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        if (anyKeytext.activeInHierarchy)
        {
            anyKeytext.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForAnyKey > 0f)
        {
            waitForAnyKey -= Time.deltaTime;
            if (waitForAnyKey <= 0f)
            {
                anyKeytext.SetActive(true);
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }
    }
}
