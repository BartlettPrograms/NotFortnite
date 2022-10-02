using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject tapContinueButton;
    [SerializeField] private GameObject MainUI;

    public void TapToContinuePressed()
    {
        tapContinueButton.SetActive(false);
        MainUI.SetActive(true);
    }

    public void PlayGamePressed()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
