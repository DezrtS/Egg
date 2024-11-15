using System.Collections.Generic;
using UnityEngine;

public class EvilChicken : Chicken
{
    private List<GameObject> goodChickenPool;

    private Transform selectedChicken;
    private bool hasChicken = false;

    private void Start()
    {
        GameManager instance = GameManager.Instance;
        goodChickenPool = instance.goodChickenPool.GetActivePool();
        ChooseNewChicken();
    }

    private void FixedUpdate()
    {
        if (hasChicken)
        {
            agent.destination = selectedChicken.position;
        }
    }

    public void ChooseNewChicken()
    {
        if (goodChickenPool.Count == 0)
        {
            agent.SetDestination(GameManager.Instance.evilChickenDiePoint.transform.position);
        }
        else
        {
            hasChicken = true;
            int chosenChicken = Random.Range(0, goodChickenPool.Count);
            selectedChicken = goodChickenPool[chosenChicken].transform;
            Chicken chicken = selectedChicken.GetComponent<Chicken>();
            chicken.OnChickenDie += OnChickenVoid;
            chicken.OnChickenEnterCoup += OnChickenVoid;
        }
    }

    public void OnChickenVoid(Chicken chicken)
    {
        hasChicken = false;
        chicken.OnChickenDie -= OnChickenVoid;
        chicken.OnChickenEnterCoup -= OnChickenVoid;
        ChooseNewChicken();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chicken"))
        {
            other.GetComponent<Chicken>().Damage(data.Damage);
        }
        else if (other.CompareTag("KillZone"))
        {
            Die();
        }
    }
}
