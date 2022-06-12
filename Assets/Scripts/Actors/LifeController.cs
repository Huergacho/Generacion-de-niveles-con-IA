using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class LifeController : MonoBehaviour
{
    [SerializeField] private float _maxLife;
    [SerializeField] private float _currentLife;

    //PROPIEDADES
    public float MaxLife => _maxLife;
    public float CurrentLife => _currentLife;
    public bool IsDead { get; private set; }

    //EVENTS
    public Action OnDie;
    public Action<float, float> UpdateLifeBar;
    public Action OnTakeDamage;
    public Action OnHeal;
    public Action OnRespawn;

    public void SetMaxLife(float maxLife)
    {
        _maxLife = maxLife;
        _currentLife = _maxLife;
    }

    public void Heal(float heal)
    {
        if (_currentLife < MaxLife && _currentLife > 0)
        {
            if (_currentLife < (MaxLife - heal))
                _currentLife += heal;
            else
                _currentLife = MaxLife;

            OnHeal?.Invoke();
            UpdateLifeBar?.Invoke(CurrentLife, MaxLife);
        }
    }

    public void TakeDamage(float damage)
    {
        if (_currentLife > 0)
        {
            _currentLife -= damage;
            OnTakeDamage?.Invoke();
            UpdateLifeBar?.Invoke(CurrentLife, MaxLife);
            CheckCurrentLife();
        }
    }

    public void CheckCurrentLife()
    {
        if (_currentLife <= 0 && !IsDead)
        {
            Die();
        }
    }

    public void Respawn()
    {
        _currentLife = MaxLife;
        IsDead = false;
        UpdateLifeBar?.Invoke(CurrentLife, MaxLife);
        OnRespawn?.Invoke();
    }

    private void Die()
    {
        IsDead = true;
        OnDie?.Invoke();
    }
}
