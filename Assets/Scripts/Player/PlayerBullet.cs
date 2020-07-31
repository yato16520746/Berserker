using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] GameObject _parent;
    [SerializeField] GameObject _explosion;
    [SerializeField] TrailRenderer _trail;
    [SerializeField] float _speed;
    [SerializeField] float _maxRange;
    [SerializeField] float _misdirection = 0.04f;
    public float TimeMoving { get { return _maxRange / _speed; } }
    bool _stopMoving;

    [Header("Damage to enemy")]
    [SerializeField] int _damage = 10;
    [SerializeField] LayerMask _shootableMask;
    [SerializeField] float _radius = 0.1f;
    RaycastHit _hit;
    bool _canCastDamage;
    Ray _beginRay;
    bool _begin;

    [SerializeField] bool _debug;

    private void Start()
    {
        _beginRay = new Ray();
    }

    public void WhenEnable()
    {
        transform.position = _parent.transform.position;
        _canCastDamage = true;
        _stopMoving = false;
        //_trail.emitting = true;
        _explosion.SetActive(false);
        _begin = true;

        StartCoroutine(StopMovingAfter(TimeMoving));
        StartCoroutine(StopCastDamageAfter(TimeMoving - (1 / 50f)));
    }

    private void Update()
    {
        if (_begin)
        {
            _begin = false;
            BeginCastDamage();
        }
        if (_canCastDamage)
        {
            CastingDamage();
        }

        if (!_stopMoving)
        {
            Vector3 position = transform.position;
            position += transform.forward * _speed * Time.deltaTime;

            transform.position = position;
        }
    }

    void BeginCastDamage()
    {
        _beginRay.origin = transform.position - transform.forward * 0.7f;
        _beginRay.direction = transform.forward;
        float distance = 0.7f + _radius;

        bool hitSomthing = Physics.Raycast(_beginRay, out _hit, distance, _shootableMask);
        if (hitSomthing)
        {
            Vector3 destination = _hit.point;

            // explode 
            _explosion.SetActive(true);
            _explosion.transform.position = destination;

            // deal damage after
            EnemyHealth enemyHealth = _hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                StartCoroutine(DealDamage(enemyHealth, 0f));
            }

            // stop moving 
            _stopMoving = true;
            StartCoroutine(DisableParentAfter(1f));

            _canCastDamage = false;
        }
    }

    void CastingDamage()
    {
        float _distance = _speed * (1 / 50f);

        bool hitSomething = Physics.SphereCast(transform.position, _radius, transform.forward, out _hit, _distance, _shootableMask);
        if (hitSomething)
        {
            Vector3 destination = _hit.point;
            float waitTime = 0; /*(destination - transform.position).magnitude / _speed;*/

            // explode after
            StartCoroutine(Explode(destination, waitTime));

            // deal damage after
            EnemyHealth enemyHealth = _hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                StartCoroutine(DealDamage(enemyHealth, waitTime));
            }

            // stop moving after
            StartCoroutine(StopMovingAfter(waitTime));

            _canCastDamage = false;
        }

    }

    public void SetDirection(Vector3 direction)
    {
        direction.y = 0f;
        direction = direction.normalized;
        Vector3 random = Random.insideUnitSphere;
        random.y = 0f;
        direction += random * _misdirection;

        // rotate to direction
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    IEnumerator StopMovingAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!_stopMoving)
        {
            _stopMoving = true;
            //_trail.emitting = false;

            StartCoroutine(DisableParentAfter(1f));
        }
    }

    IEnumerator StopCastDamageAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        _canCastDamage = false;
    }

    IEnumerator DisableParentAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        _parent.SetActive(false);
    }

    IEnumerator DealDamage(EnemyHealth enemyHealth, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // gây dame cho enemy
        enemyHealth.AddDamage(_damage);
    }

    IEnumerator Explode(Vector3 position, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _explosion.SetActive(true);
        _explosion.transform.position = position;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!_debug)
        {
            return;
        }

        float _distance = _speed * (1 / 40f);

        RaycastHit debugHit;
        bool isHit = Physics.SphereCast(transform.position, _radius, transform.forward, out debugHit, _distance, _shootableMask);
        if (isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * debugHit.distance);
            Gizmos.DrawWireSphere(transform.position + transform.forward * debugHit.distance, _radius);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * _distance);
        }
    }

#endif
}
