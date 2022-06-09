using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        resumeButton?.onClick.AddListener(OnResume);
        restartButton?.onClick.AddListener(OnRestart);
        menuButton?.onClick.AddListener(OnMenu);
        quitButton?.onClick.AddListener(OnQuit);
    }

    private void OnResume()
    {
        GameManager.instance.Pause(false);
    }

    private void OnRestart()
    {
        SceneManager.LoadScene(GameManager.instance.LevelScene);
    }

    private void OnMenu()
    {
        SceneManager.LoadScene(GameManager.instance.MenuScene);
    }

    private void OnQuit()
    {
        print("Quit Game");
        Application.Quit();
    }
}
