using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject hud;
    [SerializeField] private UIBarController lifeBar;

    void Start()
    {
        GameManager.instance.OnPause += OnPause;
        GameManager.instance.OnPlayerInit += OnPlayerAssing;
        GameManager.instance.Pause(false);
    }

    private void OnPause()
    {
        hud.SetActive(!GameManager.instance.IsGamePaused);
        pauseMenu.SetActive(GameManager.instance.IsGamePaused);
    }

    private void OnPlayerAssing(PlayerModel playerModel)
    {
        lifeBar.SetOwner(playerModel);
    }

    private void OnDestroy()
    {
        GameManager.instance.OnPause -= OnPause;
    }
}
