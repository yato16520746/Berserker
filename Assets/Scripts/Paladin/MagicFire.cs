using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFire : MonoBehaviour
{
    [SerializeField] float _speed = 4f;
    [SerializeField] int _damage = 5;
    public Transform OwnerTransform;
    [SerializeField] LayerMask _playerHealthMask;
    [SerializeField] LayerMask _mapMask;
    [SerializeField] Collider _collider;
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] float _timeMoving;

    bool _moving;

    private void OnEnable()
    {
        _moving = true;
        _particleSystem.Play();
        _collider.enabled = true;

        StartCoroutine(StopMovingAfter(_timeMoving));
    }

    void Update()
    {
        if (_moving)
        {
            Vector3 position = transform.position;
            position += transform.forward * _speed * Time.deltaTime;

            transform.position = position;
        }

    }

    IEnumerator StopMovingAfter(float time)
    {
        yield return new WaitForSeconds(time);

        _moving = false;
        _particleSystem.Stop();
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckTrigger(other);
    }

    void CheckTrigger(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerHealthMask) != 0)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth)
            {
                if (playerHealth.CanGetDamage)
                {
                    _moving = false;
                    _particleSystem.Stop();
                    _collider.enabled = false;

                    playerHealth.AddDamage(-_damage, OwnerTransform.position);
                }
            }
        }

        if (((1 << other.gameObject.layer) & _mapMask) != 0)
        {
            if (other.gameObject.tag != "Enemy")
            {

                _moving = false;
                _particleSystem.Stop();
                _collider.enabled = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CheckTrigger(other);
    }
}
