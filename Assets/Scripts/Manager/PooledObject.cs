using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [SerializeField] GameObject _mainObject;
    [SerializeField] EnemyHealth _enemyHealth;

    public void SpawnAt(Vector3 position)
    {
        _mainObject.transform.position = position;

        // quay người về phía Player
        Vector3 vector = Berserker.Instance.transform.position - position;
        vector.y = 0f;
        if (vector.magnitude > 0.001f)
        {
            _mainObject.transform.rotation = Quaternion.LookRotation(vector);
        }

        _enemyHealth.FullHealth();
    }

    private void OnDisable()
    {
        if (EnemyRound.CurrentRound)
            EnemyRound.CurrentRound.WhenAnEnemyDisable();
    }
}
