using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{

    [SerializeField]
    private float _currentHealth;

    [SerializeField]
    private float _maximumHealth;

    public float ReaminingHealtPercentage
    {
        get 
        {
            return _currentHealth / _maximumHealth; 
        }
    }

    public bool isInvincible { get; set; }

    public UnityEvent OnDie;

    public UnityEvent OnDamage;

    

    public void TakeDamage(float damageAmount)
    {
        if (_currentHealth == 0)
        { 
            return;
        }

        if (isInvincible)
        {
            return;
        }

        _currentHealth -= damageAmount;

        if(_currentHealth < 0 )
        {
            _currentHealth = 0;
        }

        if(_currentHealth == 0)
        {
            OnDie.Invoke();

        }
        else
        {
            OnDamage.Invoke();
        }
    }

    public void AddHealth(float amountToAdd)
    {
        if (_currentHealth == _maximumHealth)
        { 
            return ;
        }

        _currentHealth += amountToAdd;

        if(_currentHealth > _maximumHealth)
        {
            _currentHealth = _maximumHealth;
        }
    }


}
