using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplayManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject hud;

    void Start()
    {
        GameManager.instance.OnPause += OnPause;
        GameManager.instance.Pause(false);
    }

    private void OnPause()
    {
        hud.SetActive(!GameManager.instance.IsGamePaused);
        pauseMenu.SetActive(GameManager.instance.IsGamePaused);
    }

    private void OnDestroy()
    {
        GameManager.instance.OnPause -= OnPause;
    }
}
