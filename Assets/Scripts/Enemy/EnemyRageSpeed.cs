using UnityEngine;

public class EnemyRageSpeed : MonoBehaviour
{
    [SerializeField] private float maxSpeedMultiplier = 2f; 

    private Enemy enemy;
    private EnemyHealth enemyHealth;
    private float baseSpeed;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemy != null)
            baseSpeed = enemy.MoveSpeed; 
    }

    private void Update()
    {
        if (enemy == null || enemyHealth == null) return;

        float healthPercent = enemyHealth.CurrentHealth / enemyHealth.MaxHealth;

        
        float multiplier = 1f + (1f - healthPercent);

        
        multiplier = Mathf.Clamp(multiplier, 1f, maxSpeedMultiplier);

        
        enemy.MoveSpeed = baseSpeed * multiplier;
    }
}
