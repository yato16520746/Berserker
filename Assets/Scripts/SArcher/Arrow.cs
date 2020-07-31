using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] LayerMask _solidMask;
    [SerializeField] float _speed = 50;
    [SerializeField] float _delay = 0.1f;
    bool _canFly = false;

    Vector3 _direction = Vector3.up;
    [SerializeField] Transform _checkPoint;
    bool _hit = false;
    PlayerHealth _hitPlayerHealth;
    [SerializeField] GameObject _explosionPref;

    [SerializeField] Animator _animator;

    private void Start()
    {
        StartCoroutine(CanFly());
    }

    private void Update()
    {
        if (_canFly)
        {
            transform.position += _direction * _speed * Time.deltaTime;
        }

        if (!_hit)
        {
            Raycast();
        }
    }

    void Raycast()
    {
        Ray ray = new Ray();
        ray.origin = _checkPoint.position;
        ray.direction = _direction;

        RaycastHit hit;
        float maxDistance = _speed * Time.deltaTime * 2f ;

        if (Physics.Raycast(ray, out hit, maxDistance, _solidMask))
        {
            // có cái gì đó ở phía trước
            _hit = true;
            _hitPlayerHealth = hit.collider.GetComponent<PlayerHealth>();

            Vector3 distance = hit.point - _checkPoint.transform.position;
            distance.y = 0f;
            float delayTime = distance.magnitude / _speed;
            StartCoroutine(Hit(delayTime));
        }
    }

    IEnumerator Hit(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _canFly = false;
        _animator.SetTrigger("Destroy");
        if (_hitPlayerHealth)
        {
            _hitPlayerHealth.AddDamage(-20, transform.position);
        }

        Instantiate(_explosionPref, _checkPoint.position, Quaternion.identity);
    }

    IEnumerator CanFly()
    {
        yield return new WaitForSeconds(_delay);

        _canFly = true;
    }

    public void SetUp(Quaternion skeletonRotation)
    {
        // xoay mũi tên
        Vector3 rotation = skeletonRotation.eulerAngles;
        rotation.x = 90f;
        transform.rotation = Quaternion.Euler(rotation);

        // hướng bay
        _direction = skeletonRotation * Vector3.forward;
    }
}
