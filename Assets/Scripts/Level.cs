using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Level : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
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
