using System;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class EnemyHealth_OneDamage : MonoBehaviour
{
    public static Action<Enemy> OnEnemyKilled;
    public static Action<Enemy> OnEnemyHit;

    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform barPosition;

    [SerializeField] private float baseHealth = 5f;

    public float CurrentHealth { get; private set; }
    private float maxHealth;

    private Image _healthBar;
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();

        maxHealth = baseHealth;
        CurrentHealth = maxHealth;

        CreateHealthBar();
    }

    private void Update()
    {
        if (_healthBar != null)
        {
            _healthBar.fillAmount = Mathf.Lerp(
                _healthBar.fillAmount,
                CurrentHealth / maxHealth,
                Time.deltaTime * 10f
            );
        }
    }

    private void CreateHealthBar()
    {
        GameObject newBar = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
        newBar.transform.SetParent(transform);

        EnemyHealthContainer container = newBar.GetComponent<EnemyHealthContainer>();
        _healthBar = container.FillAmountImage;
    }

    public void DealDamage(float _)
    {
        
        CurrentHealth -= 1;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
        else
        {
            OnEnemyHit?.Invoke(_enemy);
        }
    }

    private void Die()
    {
        OnEnemyKilled?.Invoke(_enemy);

        if (Level2Score.Instance != null)
        {
            Level2Score.Instance.AddScore(10);
        }

        _enemy.OnDeath();
    }
}
