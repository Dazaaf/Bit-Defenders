using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTurretProjectile : TurretProjectile
{
    [Header("Machine Gun Settings")]
    [SerializeField] private bool isDualMachine = false;
    [SerializeField] private float spreadRange = 5f;

    [Header("Shotgun Mode Settings")]
    [SerializeField] private bool isShotgunMode = false;  
    [SerializeField] private int pelletCount = 5;         
    [SerializeField] private float spreadAngle = 30f;     

    protected override void Update()
    {
        if (Time.time > _nextAttackTime)
        {
            if (_turret.CurrentEnemyTarget != null)
            {
                Vector3 dirToTarget = _turret.CurrentEnemyTarget.transform.position - transform.position;
                FireProjectile(dirToTarget);
            }

            _nextAttackTime = Time.time + delayBtwAttacks;
        }
    }

    protected override void LoadProjectile() { }

    private void FireProjectile(Vector3 direction)
    {
        if (isShotgunMode)
        {
            FireShotgun(direction);
        }
        else
        {
            FireNormal(direction);
        }
    }

    private void FireNormal(Vector3 direction)
    {
        GameObject instance = _pooler.GetInstanceFromPool();
        instance.transform.position = projectileSpawnPosition.position;

        MachineProjectile projectile = instance.GetComponent<MachineProjectile>();
        projectile.Direction = direction;
        projectile.Damage = Damage;

        if (isDualMachine)
        {
            float randomSpread = Random.Range(-spreadRange, spreadRange);
            Vector3 spread = new Vector3(0f, 0f, randomSpread);
            Quaternion spreadValue = Quaternion.Euler(spread);
            Vector2 newDirection = spreadValue * direction;
            projectile.Direction = newDirection;
        }

        instance.SetActive(true);
    }

    private void FireShotgun(Vector3 direction)
    {
        
        Vector3 baseDir = direction.normalized;
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (pelletCount - 1);

        for (int i = 0; i < pelletCount; i++)
        {
            GameObject instance = _pooler.GetInstanceFromPool();
            instance.transform.position = projectileSpawnPosition.position;

            float currentAngle = startAngle + angleStep * i;
            Quaternion rotation = Quaternion.Euler(0f, 0f, currentAngle);
            Vector2 newDirection = rotation * baseDir;

            MachineProjectile projectile = instance.GetComponent<MachineProjectile>();
            projectile.Direction = newDirection;
            projectile.Damage = Damage / pelletCount; 

            instance.SetActive(true);
        }
    }
}
