using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarController : MonoBehaviour
{
    [Header("LifeBar")]
    [SerializeField] private GameObject bar;
    [SerializeField] private Image barImage;
    [SerializeField] private Text percentaje;
    [SerializeField] private bool animated;
    [SerializeField] private float lifebarTime = .1f;

    private float _currentHealth;
    private float _nextHealth;
    private float _maxHealth;
    private EntityModel _owner;
    private bool runningCoroutine;

    public bool IsVisible { get; private set; }

    public void SetOwner(EntityModel owner, bool isVisible = true)
    {
        _owner = owner;
        _owner.LifeController.UpdateLifeBar += UpdateLifeBar;
        _maxHealth = _owner.LifeController.MaxLife;
        _currentHealth = _maxHealth;
        SetBarVisible(isVisible);
    }

    public void UpdateLifeBar(float currentHealth, float maxHealth)
    {
        if (barImage != null)
        {
            SetBarVisible(true);

            _nextHealth = currentHealth;
            _maxHealth = maxHealth;
            if(animated)
                StartCoroutine(LifeBarAnimation());
            else
                barImage.fillAmount = currentHealth / maxHealth;
        }


        if(percentaje != null)
        {
            if (!animated)
                percentaje.text = $"{((currentHealth / maxHealth) * 100).ToString()}%";
        }

    }

    public void SetBarVisible(bool value)
    {
        bar.SetActive(value);
        IsVisible = value;
    }

    #region Private
    private IEnumerator LifeBarAnimation()
    {
        GameManager.instance.IsSceneReadyToChange = false;
        if (!runningCoroutine)
        {
            runningCoroutine = true;
            if (_nextHealth < _currentHealth)
            {
                yield return StartCoroutine(DecreaseLife());
            }
            else
            {
                yield return StartCoroutine(IncreaseLife());
            }
        }
        yield return null;
    }

    private IEnumerator DecreaseLife()
    {
        while (_nextHealth < _currentHealth && _currentHealth >= 0)
        {
            _currentHealth--;
            InternalUpdateLifeBar();
            yield return new WaitForSeconds(lifebarTime);
        }

        if (_nextHealth != _currentHealth)
        {
            _currentHealth = _nextHealth;
            InternalUpdateLifeBar();
            yield return new WaitForSeconds(lifebarTime);
        }

        GameManager.instance.IsSceneReadyToChange = true;
        runningCoroutine = false;
        yield return null;
    }

    private IEnumerator IncreaseLife()
    {
        while (_nextHealth > _currentHealth && _currentHealth <= _maxHealth)
        {
            _currentHealth++;
            InternalUpdateLifeBar();
            yield return new WaitForSeconds(lifebarTime);
        }

        if (_nextHealth != _currentHealth)
        {
            _currentHealth = _nextHealth;
            InternalUpdateLifeBar();
            yield return new WaitForSeconds(lifebarTime);
        }

        GameManager.instance.IsSceneReadyToChange = true;
        runningCoroutine = false;
        yield return null;
    }

    private void InternalUpdateLifeBar() //cuz this code repeats too much
    {
        barImage.fillAmount = _currentHealth / _maxHealth;

        if (percentaje != null)
            percentaje.text = $"{((_currentHealth / _maxHealth) * 100).ToString()}%";
    }


    private void OnDestroy()
    {
        if(_owner != null)
           _owner.LifeController.UpdateLifeBar -= UpdateLifeBar;
    }
    #endregion
}
