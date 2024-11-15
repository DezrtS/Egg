using System;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    [SerializeField] protected ChickenData data;
    protected NavMeshAgent agent;
    private int currentHealth = 0;

    public delegate void ChickenHandler(Chicken chicken);
    public event ChickenHandler OnChickenEnterCoup;
    public event ChickenHandler OnChickenDie;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = data.Maxspeed;
        currentHealth = data.Maxhealth;
    }

    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnChickenDie?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coop"))
        {
            OnChickenEnterCoup?.Invoke(this);
        }
    }
}
