﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource levelMusic, gameOverMusic, winMusic;
    public AudioSource[] SFX;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGameOver()
    {
        levelMusic.Stop();
        gameOverMusic.Play();
    }
    public void PlayeLevelWin()
    {
        levelMusic.Stop();
        winMusic.Play();
    }

    public void PlaySFX(int sfxIndex)
    {
        SFX[sfxIndex].Stop();
        SFX[sfxIndex].Play();
    }
}
