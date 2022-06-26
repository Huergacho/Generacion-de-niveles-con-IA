using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject hud;
    [SerializeField] private UIBarController lifeBar;

    [Header("Collectable")]
    [SerializeField] private GameObject collectableContainer;
    [SerializeField] private Text counterText;

    [Header("Timer")]
    [SerializeField] private Text timerText;

    private void Awake()
    {
        GameManager.instance.OnPause += OnPause;
        GameManager.instance.OnPlayerInit += OnPlayerAssing;
    }

    void Start()
    {
        GameManager.instance.Pause(false);

        LevelManager.instance.OnCollectable += UpdateCollectableCounter;
        LevelManager.instance.OnTimerUpdate += UpdateTimer;
        collectableContainer.SetActive(false);
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

    public void UpdateCollectableCounter(int value)
    {
        if (!collectableContainer.activeInHierarchy)
            collectableContainer.SetActive(true);

        counterText.text = value.ToString();
    }

    public void UpdateTimer(double value)
    {
        //timerText.text = value.ToString();
        timerText.text = TimeSpan.FromSeconds(value).ToString(@"mm\:ss");
    }

    private void OnDestroy()
    {
        GameManager.instance.OnPause -= OnPause;
        LevelManager.instance.OnCollectable -= UpdateCollectableCounter;
        LevelManager.instance.OnTimerUpdate -= UpdateTimer;
    }
}
