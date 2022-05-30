using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelEnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Transform[] levelWayPoints;
    private void Start()
    {
        GameManager.Instance.SetMaxEnemies(enemies.Length);
        GameManager.Instance.UiItems();

    }
    public void DiscountEnemie()
    {
        GameManager.Instance.KilledEnemie();
        GameManager.Instance.UiItems();
    }

}
