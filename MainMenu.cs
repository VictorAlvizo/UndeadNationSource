﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame()
    {
        SoundManager.instance.PlayOneShot(SoundManager.instance.coryHurt);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreditButton()
    {
        SoundManager.instance.PlayOneShot(SoundManager.instance.coryHurt);
    }

    public void QuitGame()
    {
        SoundManager.instance.PlayOneShot(SoundManager.instance.coryHurt);
        Application.Quit();
    }
}
