using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// lấy các EnemyHealth vào trong tầm
public class DetectEnemy : MonoBehaviour
{
    // singleton
    static DetectEnemy _instance;
    public static DetectEnemy Instance { get { return _instance; } }

    // danh sách các EnemyHealth ở gần
    [SerializeField] List<EnemyHealth> _enemyHealths;

    private void Start()
    {
        _instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (enemyHealth && !enemyHealth.IsDead)
        {
            _enemyHealths.Add(enemyHealth);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (enemyHealth && !enemyHealth.IsDead)
        {
            _enemyHealths.Remove(enemyHealth);
        }
    }

    // calculating
    public EnemyHealth Get_Target()
    {
        // dọn danh sách - loại bỏ các null hoặc đã dead 
        int i = 0;
        while (i < _enemyHealths.Count)
        {
            if (_enemyHealths[i] == null || _enemyHealths[i].IsDead)
            {
                _enemyHealths.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        // không có enemy, return
        if (_enemyHealths.Count <= 0)
        {
            return null;
        }

        // mặc định mục tiêu là enemy đầu tiên
        EnemyHealth target = _enemyHealths[0];
        float distance = (transform.position - _enemyHealths[0].transform.position).magnitude;

        // tính toán các enemy còn lại
        for (i = 1; i < _enemyHealths.Count; i++)
        {
            float newDistance = (transform.position - _enemyHealths[i].transform.position).magnitude;

            if (newDistance < distance) // enemy[i] ở gần hơn enemy hiện tại
            {
                target = _enemyHealths[i];
                distance = newDistance;
            }
        }

        return target;
    }
}
