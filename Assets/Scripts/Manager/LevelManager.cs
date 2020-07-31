using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    [SerializeField] GameObject _angelRequest;
    [SerializeField] List<EnemyRound> _enemyRounds;

    [SerializeField] float _spawnRadius = 15f;

    [SerializeField] PaladinGroup _paladinGroup;



    [Header("Audio")]
    [SerializeField] AudioSource _backgroundSource;
    [SerializeField] AudioClip _normalClip;
    [SerializeField] AudioClip _bossFightClip;
   
    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
    }

    public void ActivateEnemySpawner(int index)
    {
        if (index < 3)
        {
            _enemyRounds[index - 1].StartRound();
        }
        else
        {
            _paladinGroup.Fight();
        }
    }

    public void RepareForNextRound()
    {
        _angelRequest.gameObject.SetActive(true);
    }
    
    // hàm này chỉ tạm thời được sử dụng
    public Vector3 getRandomSpawnPosition()
    {
        Vector2 direction2D = Random.insideUnitCircle;
        if (direction2D.x == 0f && direction2D.y == 0)
        {
            return Vector3.zero;
        }

        Vector3 direction = new Vector3(direction2D.x, 0f, direction2D.y);
        Vector3 position = Vector3.zero + direction.normalized * Random.Range(0f, _spawnRadius);

        NavMeshHit hit;
        NavMesh.SamplePosition(position, out hit, _spawnRadius, 1);
        return hit.position;
    }

    public void PlayBossFightAudio()
    {
        _backgroundSource.clip = _bossFightClip;
        _backgroundSource.Play();
    }

    public GameObject CallingSpawnEnemy(EnemyType type)
    {
        Vector3 position = getRandomSpawnPosition();
        EnemyPooler pool = EnemyPooler.Instance;

        return pool.SpawnFromPool(type, position);        
    }
}
