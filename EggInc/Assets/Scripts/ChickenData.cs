using UnityEngine;

[CreateAssetMenu(fileName = "ChickenData", menuName = "Scriptable Objects/ChickenData")]
public class ChickenData : ScriptableObject
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float maxSpeed;
    [SerializeField] private int damage;

    public int Maxhealth => maxHealth;
    public float Maxspeed => maxSpeed;
    public int Damage => damage;
}