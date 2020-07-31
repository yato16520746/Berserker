using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWave : MonoBehaviour
{
    [System.Serializable]
    struct EnemyAmount
    {
        public EnemyType Type;
        public int Amount;
    }

    public int TotalEnemyAmount
    {
        get
        {
            int count = 0;
            foreach (EnemyAmount eAmount in _enemyAmounts)
            {
                count += eAmount.Amount;
            }

            return count;
        }
      
    }

    [SerializeField] List<EnemyAmount> _enemyAmounts;

    public void SpawnEnemy()
    {
        LevelManager manager = LevelManager.Instance;
        EnemyPooler pool = EnemyPooler.Instance;

        foreach(EnemyAmount eAmount in _enemyAmounts)
        {
            for (int i = 0; i < eAmount.Amount; i++)
            {
                Vector3 position = manager.getRandomSpawnPosition();

                pool.SpawnFromPool(eAmount.Type, position);
            }
        }

        gameObject.SetActive(false);
    }
}
