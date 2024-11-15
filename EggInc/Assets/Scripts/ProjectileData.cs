using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public float FireSpeed;
    public float LifetimeDuration;
    public float Damage;
    public float Gravity;
    public LayerMask LayerMask;
    public GameObject Prefab;
}
