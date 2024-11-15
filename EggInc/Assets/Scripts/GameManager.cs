using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] private float timeBeforeTapReset = 0.5f;
    [SerializeField] private float tapMultiplier = 0.01f;

    [SerializeField] private int goodChickenPoolCount;
    [SerializeField] private int evilChickenPoolCount;
    [Range(0, 100)]
    [SerializeField] private int evilChickenChance = 10;
    public Transform evilChickenDiePoint;

    [SerializeField] Transform goodChickenSpawnPoint;
    [SerializeField] Transform evilChickenSpawnPoint;

    [SerializeField] private GameObject goodChickenPrefab;
    [SerializeField] private GameObject evilChickenPrefab;
    [SerializeField] private List<Coop> coops;

    public ObjectPool goodChickenPool;
    public ObjectPool evilChickenPool;

    private float timeSinceLastTap = 0;
    private float multiplier;

    private int chickensInCoops = 0;
    private float profit = 0;
    [SerializeField] TextMeshProUGUI profitUI;
    [SerializeField] TextMeshProUGUI chickensUI;
    [SerializeField] TextMeshProUGUI multiplierUI;
    [SerializeField] TextMeshProUGUI modeUI;
    [SerializeField] private float profitMultiplier = 1;

    private bool killMode = false;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        goodChickenPool.InitializePool(goodChickenPrefab, goodChickenPoolCount, true);
        evilChickenPool.InitializePool(evilChickenPrefab, evilChickenPoolCount, true);
    }

    public bool Roll(float chance)
    {
        return (Random.Range(0f, 100f) < chance);
    }

    private void Update()
    {
        timeSinceLastTap += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F))
        {
            killMode = !killMode;
            if (killMode)
            {
                modeUI.text = "KILL MODE";
                modeUI.color = Color.red;
            }
            else
            {
                modeUI.text = "Chicken Mode";
                modeUI.color = Color.white;
            }
        } 
        else if (Input.anyKeyDown)
        {
            if (killMode)
            {
                HandleKillMode();
            }
            else
            {
                HandleUpdate();
            }

        }

        if (timeSinceLastTap >= timeBeforeTapReset)
        {
            multiplier = 0;
        }

        profit += chickensInCoops * profitMultiplier * Time.deltaTime;
        profitUI.text = $"Profit: {profit}";
        multiplierUI.text = $"Chicken Multiplier: {1 + multiplier}";
        chickensUI.text = $"Chickens: {chickensInCoops}";
    }

    public void HandleKillMode()
    {
        List<GameObject> evilChickens = evilChickenPool.GetActivePool();

        if (evilChickens.Count > 0)
        {
            int selectedChicken = Random.Range(0, evilChickens.Count);
            evilChickens[selectedChicken].GetComponent<Chicken>().Damage(2);
        }
    }

    public void HandleUpdate()
    {
        timeSinceLastTap = 0;
        float tapValue = 1 + multiplier;
        while (tapValue >= 1)
        {
            if (Roll(tapValue * 100))
            {
                if (Roll(evilChickenChance))
                {
                    GameObject newChicken = evilChickenPool.GetObject();
                    newChicken.transform.SetPositionAndRotation(evilChickenSpawnPoint.position, transform.rotation);
                    Chicken chicken = newChicken.GetComponent<Chicken>();
                    int chosenCoop = Random.Range(0, coops.Count);
                    newChicken.GetComponent<NavMeshAgent>().SetDestination(coops[chosenCoop].GetDestination().position);
                    chicken.OnChickenDie += OnEvilChickenDie;
                }
                else
                {
                    GameObject newChicken = goodChickenPool.GetObject();
                    newChicken.transform.SetPositionAndRotation(goodChickenSpawnPoint.position, transform.rotation);
                    Chicken chicken = newChicken.GetComponent<Chicken>();
                    int chosenCoop = Random.Range(0, coops.Count);
                    newChicken.GetComponent<NavMeshAgent>().SetDestination(coops[chosenCoop].GetDestination().position);
                    chicken.OnChickenDie += OnGoodChickenDie;
                    chicken.OnChickenEnterCoup += OnChickenEnterCoop;
                }
            }
            tapValue -= 1;
        }
        multiplier += Time.deltaTime * tapMultiplier;
    }

    public void OnChickenEnterCoop(Chicken chicken)
    {
        chickensInCoops++;
        chicken.OnChickenDie -= OnGoodChickenDie;
        chicken.OnChickenEnterCoup -= OnChickenEnterCoop;
        goodChickenPool.ReturnToPool(chicken.gameObject);
    }

    public void OnGoodChickenDie(Chicken chicken)
    {
        chicken.OnChickenDie -= OnGoodChickenDie;
        chicken.OnChickenEnterCoup -= OnChickenEnterCoop;
        goodChickenPool.ReturnToPool(chicken.gameObject);
    }

    public void OnEvilChickenDie(Chicken chicken)
    {
        evilChickenPool.ReturnToPool(chicken.gameObject);
        chicken.OnChickenDie -= OnEvilChickenDie;
    }

    public IProjectile SpawnProjectile(Vector3 position, Quaternion rotation, ProjectileData projectileData)
    {
        GameObject projectile = Instantiate(projectileData.Prefab, position, rotation);
        return projectile.GetComponent<IProjectile>();
    }
}
