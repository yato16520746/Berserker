using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeteor : MonoBehaviour
{
    enum MeteorState
    {
        ChasePlayer,
        StopMoving
    }

    //
    [Header("Boom movement")]
    [SerializeField] Transform _boomTransform;
    [SerializeField] Rigidbody _boomRb;
    [SerializeField] float _speed = 10f;
    [SerializeField] float _height = 10f;
    [SerializeField] float _randomRange = 3f;
    [SerializeField] float _lerpSpeed = 1f;
    [SerializeField] float _playerMoveRange = 1f;
    [SerializeField] float _explodeHeight = 0.5f;
    Vector3 _originDirection;
    MeteorState _state;

    //
    [Header("Boom graphic")]
    [SerializeField] Animator _boomAnimator;

    //
    [Header("Explosion")]
    [SerializeField] GameObject _explosion;
    [SerializeField] SphereCastDamage _castDamage;

    //
    [Header("Audio")]
    [SerializeField] AudioClip _clip;
    [SerializeField] AudioSource _audioSource;

    // gọi hàm này sau khi Wizard gọi Meteor active lên
    public void AfterSetActive()
    {
        // đưa boom lên trên đầu player
        Vector3 playerPos = Berserker.Instance.transform.position;

        Vector3 insideSphere = Random.insideUnitSphere;
        insideSphere.y = 0f;

        Vector3 boomPos = playerPos + new Vector3(0f, _height, 0f) + insideSphere * _randomRange;
        _boomTransform.position = boomPos;

        // hướng di chuyển ban đầu
        _originDirection = playerPos/* + new Vector3(0f, _explodeHeight * 0.8f, 0f)*/ - boomPos;
        _originDirection = _originDirection.normalized;
        _boomRb.velocity = Vector3.zero;

        //
        _explosion.SetActive(false);
        _state = MeteorState.ChasePlayer;
    }

    void Update()
    {
        if (_state == MeteorState.ChasePlayer)
        {
            Vector3 playerPos = Berserker.Instance.transform.position;
            Vector3 direction = playerPos + new Vector3(0f, _explodeHeight * 0.6f, 0f) - _boomTransform.position;

            Vector3 velocity = direction.normalized * _speed;

            _boomRb.velocity = Vector3.Lerp(_boomRb.velocity, velocity, _lerpSpeed * Time.deltaTime);

            if (_boomTransform.position.y < _explodeHeight)
            {
                // phát nổ ngay tại đây
                _state = MeteorState.StopMoving;

                _boomAnimator.SetTrigger("Disappear");

                _explosion.transform.position = _boomTransform.position;
                _castDamage.gameObject.SetActive(true);
                _explosion.SetActive(true);
          

                _audioSource.PlayOneShot(_clip, 0.3f);

                StartCoroutine(TurnOffCastDamage(0.4f));
                StartCoroutine(Deactive(1f));
            }
        }
        else
        {
            _boomRb.velocity = Vector3.zero;
        }
    }

    IEnumerator TurnOffCastDamage(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _castDamage.gameObject.SetActive(false);
    }

    IEnumerator Deactive(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        gameObject.SetActive(false);
    }
}
