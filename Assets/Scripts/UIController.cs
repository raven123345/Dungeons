using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    #region Health
    [Header("Health")]
    public Slider healthSlider;
    public Text healthText;
    #endregion

    #region Coins
    [Header("Coins")]
    public Text coinText;
    #endregion
    public GameObject deathScreen;

    public string newGameScene, mainMenuScene;

    public Image fadesScreen;
    public float fadeSpeed;

    public GameObject pauseMenu;

    private bool fadeToBlack, fadeOutBlack;



    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        fadeOutBlack = true;
        fadeToBlack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOutBlack)
        {
            fadesScreen.color = new Color(fadesScreen.color.r, fadesScreen.color.b, fadesScreen.color.g, Mathf.MoveTowards(fadesScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadesScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }
        if (fadeToBlack)
        {
            fadesScreen.color = new Color(fadesScreen.color.r, fadesScreen.color.b, fadesScreen.color.g, Mathf.MoveTowards(fadesScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadesScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newGameScene);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }
}
