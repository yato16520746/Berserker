using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] float _size;

    Queue<GameObject> _objectQueue;

    private void Start()
    {
        _objectQueue = new Queue<GameObject>();

        for (int i = 0; i < _size; i++)
        {
            GameObject obj = Instantiate(_prefab, transform);
            obj.SetActive(false);
            _objectQueue.Enqueue(obj);
        }

    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = _objectQueue.Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        _objectQueue.Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
