using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _range;

    [Space]
    [SerializeField] GameObject _explosionPref;

    Vector3 _direction;
    float _minX;
    float _maxX;
    float _minZ;
    float _maxZ;

    private void Start()
    {
        Ray shootRay = new Ray();
        RaycastHit shootHit;

        // cài đặt để ray cast
        int shootableMask = LayerMask.GetMask("Enemy Health");
        shootRay.origin = transform.position;
        shootRay.direction = _direction;

        Vector3 destination;

        if (Physics.Raycast(shootRay, out shootHit, _range, shootableMask))
        {
            // bắn trúng 1 vật nào đó tại shootHit
            destination = shootHit.point;

            // thời gian trễ để đạn bay đến đích: waitTime = khoảng cách / vận tốc
            float waitTime = (destination - transform.position).magnitude / _speed;

            // tạo vụ nổ
            StartCoroutine(SpawnExplosion(destination, waitTime));

            // gây sát thương cho enemy, nếu có
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                StartCoroutine(DealDamage(enemyHealth, waitTime));
            }
        }
        else
        {
            // không bắn trúng cái gì cả
            destination = transform.position + _direction * _range;
        }

        // lấy các giá trị để cố định đích đến
        _minX = Mathf.Min(transform.position.x, destination.x);
        _maxX = Mathf.Max(transform.position.x, destination.x);
        _minZ = Mathf.Min(transform.position.z, destination.z);
        _maxZ = Mathf.Max(transform.position.z, destination.z);
    }

    void Update()
    {
        Vector3 position = transform.position;
        position += _direction * _speed * Time.deltaTime;

        // cố định tọa độ không vượt quá đích đến
        position.x = Mathf.Clamp(position.x, _minX, _maxX);
        position.z = Mathf.Clamp(position.z, _minZ, _maxZ);

        // đưa giá trị tọa độ vào
        transform.position = position;
    }

    // hướng di chuyển của nhân vật
    public void SetDirection(Vector3 direction) 
    {
        _direction = direction.normalized;
        Vector3 random = Random.insideUnitSphere;
        random.y = 0f;
        _direction += random * 0.05f;
        _direction = _direction.normalized;
    }

    IEnumerator SpawnExplosion(Vector3 position, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // tạo vụ nổ tại position
        Instantiate(_explosionPref, position, Quaternion.identity);
    }

    IEnumerator DealDamage(EnemyHealth enemyHealth, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // gây dame cho enemy
        enemyHealth.AddDamage(10);
    }
}
