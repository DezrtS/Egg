using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private GameObject objectPrefab;
    private List<GameObject> pool = new();
    private List<GameObject> activePool = new();
    private bool spawnNewIfEmpty = false;
    private GameObject poolContainer;
    private int spawnCount = 0;

    public void InitializePool(GameObject objectPrefab, int objectCount, bool spawnNewIfEmpty)
    {
        this.objectPrefab = objectPrefab;
        poolContainer = new GameObject("Pool Container");

        for (int i = 0; i < objectCount; i++)
        {
            GameObject instance = Instantiate(objectPrefab, poolContainer.transform);
            instance.name = $"Chicken {spawnCount}";
            spawnCount++;
            instance.SetActive(false);
            pool.Add(instance);
        }

        this.spawnNewIfEmpty = spawnNewIfEmpty;
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject activeObject = pool[0];
            pool.RemoveAt(0);
            activeObject.SetActive(true);
            activePool.Add(activeObject);
            if (pool.Contains(activeObject))
            {
                Debug.LogError("Object WAS NOT REMOVED");
            }
            return activeObject;
        }
        else if (spawnNewIfEmpty)
        {
            GameObject instance = Instantiate(objectPrefab, poolContainer.transform);
            instance.name = $"Chicken {spawnCount}";
            spawnCount++;
            activePool.Add(instance);
            return instance;
        }

        return null;
    }

    public void ReturnToPool(GameObject gameObject)
    {
        if (pool.Contains(gameObject))
        {
            Debug.LogError("Object Already In Pool");
        }
        activePool.Remove(gameObject);
        gameObject.SetActive(false);
        pool.Add(gameObject);
    }

    public List<GameObject> GetActivePool()
    {
        return activePool;
    }
}
