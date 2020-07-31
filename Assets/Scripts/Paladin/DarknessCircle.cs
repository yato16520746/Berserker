using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessCircle : MonoBehaviour
{
    float _countAlive;

    [Space]
    [SerializeField] Transform _mainTransform;
    [SerializeField] List<Transform> _spawnTransforms;

    [Space]
    [SerializeField] float _timeBetweenSpawn = 0.5f;
    float _countSpawn;

    [Header("Pool")]
    [SerializeField] GameObject _magicFirePrefab;
    [SerializeField] int _size;
    Queue<GameObject> _objectQueue;

    [Header("Audio")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _spawnClip;    

    private void Start()
    {
        PoolStart();
    }

    private void Update()
    {
        _countAlive -= Time.deltaTime;
        if (_countAlive < 0)
        {
            return;
        }

        _countSpawn -= Time.deltaTime;
        if (_countSpawn < 0)
        {
            SpawnMagicFires();
            _audioSource.PlayOneShot(_spawnClip);

            _countSpawn = _timeBetweenSpawn;
        }
    }

    public void StartSpawn(Vector3 position, float aliveTime)
    {
        _mainTransform.position = position;
        _countAlive = aliveTime;
        _countSpawn = 0f;
    }

    void SpawnMagicFires()
    {
        foreach (Transform transf in _spawnTransforms)
        {
            SpawnFromPool(transf.position, transf.rotation);
        }
    }


    // Pool
    void PoolStart()
    {
        _objectQueue = new Queue<GameObject>();

        for (int i = 0; i < _size; i++)
        {
            GameObject obj = Instantiate(_magicFirePrefab, transform);

            MagicFire magicFire = obj.GetComponent<MagicFire>();
            magicFire.OwnerTransform = transform;

            obj.SetActive(false);
            _objectQueue.Enqueue(obj);
        }
    }

    GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = _objectQueue.Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        _objectQueue.Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
    