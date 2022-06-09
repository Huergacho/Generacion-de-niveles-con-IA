using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private bool _isGamePaused;

    [Header("Scene Names")]
    [SerializeField] private Dictionary<Scene, float> levels = new Dictionary<Scene, float>(); //TODO: move this to a level manager that does the randomnizer inside the level?
    [SerializeField] private string levelScene = "Level";
    [SerializeField] private string menuScene = "MainMenu";
    [SerializeField] private string victoryScene = "Victory";
    [SerializeField] private string gameOverScene = "GameOver";


    [Header("to move")] //MOVE SOMEWHERE ELSE
    [SerializeField]private int actualEnemies;
    [SerializeField] private int maxEnemies;
    [SerializeField] private TextMeshProUGUI tmPro;
    
    private bool isPlayerAlive;

    //Properties
    public bool IsGamePaused { get => _isGamePaused; private set => _isGamePaused = value; } //TODO: Make Everytone check if game is paused to update or something. 
    public PlayerModel Player { get; private set; }
    public string MenuScene => menuScene;
    public string LevelScene => levelScene;
    public bool IsPlayerAlive => isPlayerAlive;

    //Events
    public Action OnPause;
    public Action<PlayerModel> OnPlayerInit;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R)) 
        //{
        //    RestartGame();
        //}
        if (Input.GetKeyDown(KeyCode.Escape)) //TODO: Make Input Controller??
        {
            Pause(!IsGamePaused);
        }
    }
    public void SetMaxEnemies(int maxItemsToSet)
    {
        maxEnemies = maxItemsToSet;
    }
    public void KilledEnemie()
    {
        actualEnemies++;
    }

    public void PlayerIsDead()
    {
        isPlayerAlive = false;
        GameOver();
    }

    //private void RestartGame() //Moved to PauseMenu. 
    //{
    //    SceneManager.LoadScene(0);
    //}

    public void UiItems()
    {
        tmPro.text = actualEnemies.ToString() + " / " + maxEnemies.ToString();
    }
    public void LevelChange()
    {
        var levelSelected = MyEngine.MyRandom.GetRandomWeight(levels);

        SceneManager.LoadScene(levelSelected.buildIndex);

        levels.Remove(levelSelected);
    }

    public void Pause(bool value)
    {
        IsGamePaused = value;
        //SetCursorActive(value);
        if (value)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        OnPause?.Invoke();
    }

    public void SetCursorActive(bool value)
    {
        if (value)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetPlayer(PlayerModel player)
    {
        Player = player;
        OnPlayerInit?.Invoke(Player);
    }

    private void Victory()
    {
        SceneManager.LoadScene(victoryScene);
    }

    private void GameOver()
    {
        SceneManager.LoadScene(gameOverScene);
    }

}
