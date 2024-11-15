using UnityEngine;

public interface IProjectile
{
    ProjectileData ProjectileData { get; }
    public abstract void Initialize(ProjectileData projectileData);
    public abstract void Fire(Vector3 direction);
    public abstract void Destroy();
}
