using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject credits;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button goBackButton;


    void Awake()
    {
        playButton.onClick.AddListener(OnClickPlayHandler);
        creditsButton.onClick.AddListener(OnClickCreditsHandler);
        quitButton.onClick.AddListener(OnClickQuitHandler);
        goBackButton.onClick.AddListener(OnClickGoBackHandler);

        GoBack();
    }

    private void Start()
    {
        GameManager.instance.OnPause += GoBack;
    }

    private void GoBack()
    {
        menu.SetActive(true);
        credits.SetActive(false);
    }

    private void OnClickPlayHandler()
    {
        SceneManager.LoadScene(GameManager.instance.LevelScene);
    }

    private void OnClickCreditsHandler()
    {
        menu.SetActive(false);
        credits.SetActive(true);
    }
    private void OnClickGoBackHandler()
    {
        GoBack();
    }

    private void OnClickQuitHandler()
    {
        print("Cerramos el juego");
        Application.Quit();
    }

    private void OnDestroy()
    {
        GameManager.instance.OnPause -= GoBack;
    }
}
