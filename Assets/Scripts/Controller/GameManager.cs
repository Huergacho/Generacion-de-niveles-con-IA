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
    public static GameManager Instance;
    [SerializeField]private int actualEnemies;
    [SerializeField] private int maxEnemies;
    [SerializeField] private PlayerController _player;
    [SerializeField] private TextMeshProUGUI tmPro;
    private bool isPlayerAlive;
    public bool IsPlayerAlive => isPlayerAlive;
    [SerializeField] private Dictionary<Scene, float> levels = new Dictionary<Scene, float>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        isPlayerAlive = true;
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
    private void WinGame()
    {
        SceneManager.LoadScene(1);
    }
    public void PlayerIsDead()
    {
        isPlayerAlive = false;
        LostScene();
    }
    private void LostScene()
    {
        SceneManager.LoadScene(0);
    }
    public void AssignPlayer(PlayerController player)
    {
        _player = player;
    }
    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
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

}
