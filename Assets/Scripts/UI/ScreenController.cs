using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button levelButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        levelButton.onClick.AddListener(OnClickLevelHandler);
        menuButton.onClick.AddListener(OnClickMenuHandler);
        quitButton.onClick.AddListener(OnClickQuitHandler);
    }

    private void OnClickLevelHandler()
    {
        SceneManager.LoadScene(GameManager.instance.LevelScene);
    }

    private void OnClickMenuHandler()
    {
        SceneManager.LoadScene(GameManager.instance.MenuScene);
    }

    private void OnClickQuitHandler()
    {
        print("Cerramos el juego");
        Application.Quit();
    }
}
