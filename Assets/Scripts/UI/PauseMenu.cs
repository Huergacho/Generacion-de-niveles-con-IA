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
        resumeButton?.onClick.AddListener(OnClickResumeHandler);
        restartButton?.onClick.AddListener(OnClickRestartHandler);
        menuButton?.onClick.AddListener(OnClickMenuHandler);
        quitButton?.onClick.AddListener(OnClickQuitHandler);
    }

    private void OnClickResumeHandler()
    {
        GameManager.instance.Pause(false);
    }

    private void OnClickRestartHandler()
    {
        SceneManager.LoadScene(GameManager.instance.LevelScene);
    }

    private void OnClickMenuHandler()
    {
        SceneManager.LoadScene(GameManager.instance.MenuScene);
    }

    private void OnDestroy()
    {
        resumeButton?.onClick.RemoveListener(OnClickResumeHandler);
        restartButton?.onClick.RemoveListener(OnClickRestartHandler);
        menuButton?.onClick.RemoveListener(OnClickMenuHandler);
        quitButton?.onClick.RemoveListener(OnClickQuitHandler);
    }

    private void OnClickQuitHandler()
    {
        print("Quit Game");
        Application.Quit();
    }
}
