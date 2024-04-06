using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool continueGame = false;
    public bool startGame = false;

    public static MainMenu instance;

    private void Start()
    {
        Debug.Log("MainMenu 실행시에");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        instance = this;
    }

    public void onContinueButton()
    {
        Debug.Log("Continue");
        continueGame = true;
        SceneManager.LoadScene("Town");
    }

    public void OnStartButton()
    {
        Debug.Log("Starting");
        startGame = true;
        SceneManager.LoadScene("Town");
    }

    public void OnQuitButton()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
