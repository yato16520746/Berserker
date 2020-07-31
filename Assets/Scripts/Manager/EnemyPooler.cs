using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Sword,
    Archer,
    Wizard
}

public class EnemyPooler : MonoBehaviour
{
    static EnemyPooler _instance;
    public static EnemyPooler Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }


    [System.Serializable]
    public class Pool
    {
        public EnemyType Type;
        public GameObject Prefab;
        public int Size;
    }

    public List<Pool> pools;
    public Dictionary<EnemyType, Queue<GameObject>> poolDictionary;

 
    private void Start()
    {
        poolDictionary = new Dictionary<EnemyType, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.Type, objectPool);
        }
    }

    public GameObject SpawnFromPool(EnemyType type, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[type].Dequeue();

        if (objectToSpawn.activeSelf)
        {
            Debug.LogError("Spawn Enemy dang hoat dong");
        }

        objectToSpawn.SetActive(true);

        PooledObject pooledObj = objectToSpawn.GetComponent<PooledObject>();
        if (pooledObj)
        {
            pooledObj.SpawnAt(position);
        }
      
        poolDictionary[type].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
