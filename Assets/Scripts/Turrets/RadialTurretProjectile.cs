using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialTurretProjectile : TurretProjectile
{
    [Header("🔸 Configuración del disparo radial")]
    [SerializeField] private int projectileCount = 12;      
    [SerializeField] private float detectionRadius = 3f;    
    [SerializeField] private float coneAngleOffset = 0f;    

    protected override void Update()
    {
        if (Time.time > _nextAttackTime)
        {
            
            if (_turret.CurrentEnemyTarget != null || AnyEnemyNearby())
            {
                FireRadialBurst();
                _nextAttackTime = Time.time + delayBtwAttacks;
            }
        }
    }

    protected override void LoadProjectile() { }

    private bool AnyEnemyNearby()
    {
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) <= detectionRadius)
                return true;
        }
        return false;
    }

    private void FireRadialBurst()
    {
        float angleStep = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float currentAngle = coneAngleOffset + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(0f, 0f, currentAngle);

            
            GameObject instance = _pooler.GetInstanceFromPool();
            instance.transform.position = projectileSpawnPosition.position;

            
            Vector2 direction = rotation * Vector2.right;

            
            MachineProjectile projectile = instance.GetComponent<MachineProjectile>();
            projectile.Direction = direction;
            projectile.Damage = Damage;

            instance.SetActive(true);
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
