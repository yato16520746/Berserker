using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSword : MonoBehaviour
{
    enum SwordState
    {
        FollowPaladin,
        LookPlayer,
        AttackPlayer,

    }

    [SerializeField] Transform _paladinTransform;
    [SerializeField] Transform _followTransform;

    [SerializeField] Rigidbody _rb;
    [SerializeField] float _speed = 10f;
    [SerializeField] float _speedLerp = 4f;

    SwordState _state;

    void Start()
    {
        _state = SwordState.FollowPaladin;
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == SwordState.FollowPaladin)
        {
            Vector3 direction = _followTransform.position - transform.position;
            if (direction.magnitude > 1f)
            {


                Vector3 velocity = direction.normalized * _speed;

                _rb.velocity = Vector3.Lerp(_rb.velocity, velocity, _speedLerp * Time.deltaTime);
            }
        }
        else
        {
            // look player
            Player player = Player.Instance;
            Vector3 targetPos = player.transform.position + new Vector3(0, 1.87562f / 2f, 0);

            Vector3 vector = targetPos - transform.position;

            Quaternion rotation = Quaternion.LookRotation(vector);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10f * Time.deltaTime);
        }


    }
}
