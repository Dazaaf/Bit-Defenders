using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public static Action<Enemy> OnEnemyKilled;
    public static Action<Enemy> OnEnemyHit;

    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform barPosition;

    [SerializeField] private float baseHealth = 10f;

    public float CurrentHealth { get; set; }

    
    public float MaxHealth => maxHealth;

    private float maxHealth;

    private Image _healthBar;
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();

        
        float difficultyMultiplier = 1f;

        if (DifficultyManager.Instance != null)
        {
            switch (DifficultyManager.Instance.SelectedDifficulty)
            {
                case Difficulty.Easy:
                    difficultyMultiplier = 1f;
                    break;
                case Difficulty.Hard:
                    difficultyMultiplier = 2f;
                    break;
            }
        }

        maxHealth = baseHealth * difficultyMultiplier;
        CurrentHealth = maxHealth;

        CreateHealthBar();
    }

    private void Update()
    {
        _healthBar.fillAmount = Mathf.Lerp(
            _healthBar.fillAmount,
            CurrentHealth / maxHealth,
            Time.deltaTime * 10f
        );

        
        if (Input.GetKeyDown(KeyCode.P))
            DealDamage(5f);
    }

    private void CreateHealthBar()
    {
        GameObject newBar = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
        newBar.transform.SetParent(transform);

        EnemyHealthContainer container = newBar.GetComponent<EnemyHealthContainer>();
        _healthBar = container.FillAmountImage;
    }

    public void DealDamage(float damageReceived)
    {
        CurrentHealth -= damageReceived;

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

    public void ResetHealth()
    {
        CurrentHealth = maxHealth;
        _healthBar.fillAmount = 1f;
    }

    private void Die()
    {
        
        AchievementManager.Instance.AddProgress("Kill20", 1);
        AchievementManager.Instance.AddProgress("Kill50", 1);
        AchievementManager.Instance.AddProgress("Kill100", 1);

        OnEnemyKilled?.Invoke(_enemy);

        
        if (Level2Score.Instance != null)
        {
            Level2Score.Instance.AddScore(10);
        }

        
        _enemy.OnDeath();
    }
}


