using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarController : MonoBehaviour
{
    [SerializeField] private GameObject bar;
    [SerializeField] private Image barImage;
    [SerializeField] private Text percentaje;

    private EntityModel _owner;  

    public bool IsVisible { get; private set; }

    public void SetOwner(EntityModel owner)
    {
        _owner = owner;
        _owner.LifeController.UpdateLifeBar += UpdateLifeBar;
    }

    public void UpdateLifeBar(float currentHealth, float maxHealth)
    {
        if (barImage != null)
            barImage.fillAmount = (float)currentHealth / maxHealth;

        if(percentaje != null)
            percentaje.text = $"{((currentHealth / maxHealth) * 100).ToString()}%";
    }

    public void SetBarVisible(bool value)
    {
        bar.SetActive(value);
        IsVisible = value;
    }

    private void OnDestroy()
    {
        _owner.LifeController.UpdateLifeBar -= UpdateLifeBar;
    }
}
