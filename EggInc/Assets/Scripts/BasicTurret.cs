using UnityEngine;

public class BasicTurret : MonoBehaviour
{
    [SerializeField] private TurretData turretData;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private ProjectileData projectileData;
    private float fireRateTimer;

    private void FixedUpdate()
    {
        fireRateTimer += Time.fixedDeltaTime;
        if (fireRateTimer >= turretData.fireRate)
        {
            fireRateTimer = 0f;
            IProjectile projectile = GameManager.Instance.SpawnProjectile(projectileSpawnPoint.position, projectileSpawnPoint.rotation, projectileData);
            projectile.Initialize(projectileData);
            projectile.Fire(transform.forward);
        }
    }
}
