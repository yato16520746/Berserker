using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Behaviour
{
    Enter, Update, Exit
}

public enum BerserkerState
{
    None,
    Idle,
    Walk,
    Dodge,
    Attack1,
    Attack2,
    Attack3,
    Hit,
    ForceIdle,
    Death
}

// lớp Delegate tuyệt đối ko dùng ngoài Berserker Group
public class Berserker_Delegate : MonoBehaviour
{
    [Space]
    [SerializeField] Transform _parentTransform;
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody _rb;
    [SerializeField] AudioSource _audioSource;

    //
    [Header("Movement")]
    [SerializeField] float _walkSpeed = 5.5f;
    float _walkThreshold = 0f;
    [SerializeField] float _dodgeSpeed = 12f;
    [SerializeField] float _speedLerp = 15f;
    [SerializeField] float _rotateLerp = 20f;
    Vector3 _moveDirection;
    BerserkerState _state = BerserkerState.None;

    //
    [Header("Mouse input")]
    [SerializeField] float _mouseRange = 50f;
    [SerializeField] LayerMask _mouseMask;
    Ray _mouseRay;
    RaycastHit _mouseHit;
    Vector3 _mouseInputPosition;

    //
    [Header("Dodge state")]
    [SerializeField] float _dodgeSpeedDown = 2f;
    bool _isDodgeSpeedDown = false;
    Vector3 _dodgeDirection;
    [SerializeField] AudioClip _clipDodge;
    [SerializeField] float _volumeDodge = 1f;

    // 
    [Header("All Attack")]
    [SerializeField] float _timeBetweenAttack = 0.35f;
    Vector3 _attackDirection;
    [SerializeField] BerserkerDamage _damage;

    //
    [Header("Attack 1")]
    [SerializeField] float _attack1MoveSpeed = 3f;
    bool _allowAttack1Moving = false;

    [SerializeField] AudioClip _clipAttack1;
    [SerializeField] float _volumeAttack1 = 1f;
    [SerializeField] AudioClip _clipVoiceAttack1;
    [SerializeField] float _volumeVoiceAttack1 = 1f;

    //
    [Header("Attack 2")]
    [SerializeField] float _attack2MoveSpeed = 3f;
    bool _allowAttack2Moving = false;

    [SerializeField] AudioClip _clipAttack2;
    [SerializeField] float _volumeAttack2 = 1f;
    [SerializeField] AudioClip _clipVoiceAttack2;
    [SerializeField] float _volumeVoiceAttack2 = 1f;

    //
    [Header("Attack 3")]
    [SerializeField] float _attack3MoveSpeed = 3f;
    bool _allowAttack3Moving = false;

    [SerializeField] AudioClip _clipAttack3;
    [SerializeField] float _volumeAttack3 = 1f;
    [SerializeField] AudioClip _clipVoiceAttack3;
    [SerializeField] float _volumeVoiceAttack3 = 1f;

    // hit
    [Header("Hit")]
    [SerializeField] float _hitForce = 25f;
    Vector3 _hitForceDirection;
    bool _stopHitForce = false;

    //
    void Start()
    {
        // mouse
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        //
        Event_TurnOffDamage();
    }

    //
    void Update()
    {
        // AWSD input
        float horizontal = 0f;
        float vertical = 0f;

        if ((Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) ||
            (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)))
        {
            horizontal = 0;
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                horizontal = 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                horizontal = -1;
            }
        }

        if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) ||
           (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)))
        {
            vertical = 0;
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                vertical = 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                vertical = -1;
            }
        }

        _moveDirection = new Vector3(horizontal, 0f, vertical);
        _moveDirection = _moveDirection.normalized;

        // walking input
        if (_moveDirection.x != 0f || _moveDirection.z != 0f)
        {
            _animator.SetBool("Walking", true);
            _walkThreshold = 1f;
        }
        else
        {
            _walkThreshold = Mathf.Lerp(_walkThreshold, 0f, 10f * Time.deltaTime);
            if (_walkThreshold < 0.3f)
            {
                _animator.SetBool("Walking", false);
            }
        }

        // mouse - attack input
        if (Input.GetMouseButtonDown(0) && _state != BerserkerState.Hit && _state != BerserkerState.Attack3 && 
            _state != BerserkerState.ForceIdle)
        {
            _mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_mouseRay, out _mouseHit, _mouseRange, _mouseMask))
            {
                _mouseInputPosition = _mouseHit.point;
            }

            _animator.SetBool("Attack", true);
        }

        // dodge input
        if (Input.GetKeyDown(KeyCode.Space) && _state != BerserkerState.Hit && _state != BerserkerState.Dodge && 
            _state != BerserkerState.ForceIdle)
        {
            _animator.SetTrigger("Dodge");
        }
    }

    // state machine behaviour
    public void OnStateBehaviour(Behaviour behaviour)
    {
        switch (_state)
        {
            case BerserkerState.ForceIdle:
                if (behaviour == Behaviour.Update)
                {
                    _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _speedLerp * Time.deltaTime);
                }
                break;

            case BerserkerState.Idle:
                if (behaviour == Behaviour.Update)
                {
                    _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _speedLerp * Time.deltaTime);
                }
                break;

            case BerserkerState.Walk:
                if (behaviour == Behaviour.Update)
                {
                    Vector3 velocity = _moveDirection * _walkSpeed;
                    _rb.velocity = Vector3.Lerp(_rb.velocity, velocity, _speedLerp * Time.deltaTime);

                    Vector3 look = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
                    if (look.magnitude > 0.001f)
                    {
                        Quaternion rotation = Quaternion.LookRotation(look);
                        _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, _rotateLerp * Time.deltaTime);
                    }
                }
                break;

            case BerserkerState.Attack1:
                if (behaviour == Behaviour.Enter)
                {
                    _damage.Damage = 25;

                    _animator.SetBool("Attack", false);

                    _animator.SetBool("Attack Transition", false);
                    StartCoroutine(AllowAttackTransition(_timeBetweenAttack));

                    _attackDirection = _mouseInputPosition - transform.position;
                    _attackDirection.y = 0f;
                    _attackDirection = _attackDirection.normalized;

                    Event_Attack1_StopMove();
                }
                else if (behaviour == Behaviour.Update)
                {
                    if (_allowAttack1Moving)
                    {
                        _rb.velocity = Vector3.Lerp(_rb.velocity, _attackDirection/*_moveDirection*/ * _attack1MoveSpeed, _speedLerp * Time.deltaTime);
                    }
                    else
                    {
                        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _speedLerp * Time.deltaTime);
                    }

                    Quaternion rotation = Quaternion.LookRotation(_attackDirection);
                    _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, _rotateLerp * Time.deltaTime);
                }
                else if (behaviour == Behaviour.Exit)
                {
                    Event_TurnOffDamage();
                }
                break;

            case BerserkerState.Attack2:
                if (behaviour == Behaviour.Enter)
                {
                    _damage.Damage = 25;

                    _animator.SetBool("Attack", false);

                    _animator.SetBool("Attack Transition", false);
                    StartCoroutine(AllowAttackTransition(_timeBetweenAttack));

                    _attackDirection = _mouseInputPosition - transform.position;
                    _attackDirection.y = 0f;
                    _attackDirection = _attackDirection.normalized;

                    Event_Attack2_StopMove();
                }
                else if (behaviour == Behaviour.Update)
                {
                    if (_allowAttack2Moving)
                    {
                        _rb.velocity = Vector3.Lerp(_rb.velocity, _attackDirection /*_moveDirection */* _attack2MoveSpeed, _speedLerp * Time.deltaTime);
                    }
                    else
                    {
                        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _speedLerp * Time.deltaTime);
                    }

                    Quaternion rotation = Quaternion.LookRotation(_attackDirection);
                    _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, _rotateLerp * Time.deltaTime);
                }
                else if (behaviour == Behaviour.Exit)
                {
                    Event_TurnOffDamage();
                }
                break;

            case BerserkerState.Attack3:
                if (behaviour == Behaviour.Enter)
                {
                    _damage.Damage = 35;
                    _animator.SetBool("Attack", false);

                    _attackDirection = _mouseInputPosition - transform.position;
                    _attackDirection.y = 0f;
                    _attackDirection = _attackDirection.normalized;

                    Event_Attack3_StopMove();
                }
                else if (behaviour == Behaviour.Update)
                {
                    if (_allowAttack3Moving)
                    {
                        _rb.velocity = Vector3.Lerp(_rb.velocity, _attackDirection * _attack3MoveSpeed, _speedLerp * Time.deltaTime);
                    }
                    else
                    {
                        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _speedLerp * Time.deltaTime);
                    }

                    Quaternion rotation = Quaternion.LookRotation(_attackDirection);
                    _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, _rotateLerp * Time.deltaTime);
                }
                else if (behaviour == Behaviour.Exit)
                {
                    _animator.SetBool("Attack", false);
                    Event_TurnOffDamage();
                }
                break;

            case BerserkerState.Dodge:
                if (behaviour == Behaviour.Enter)
                {
                    Event_PlayerDodgeClip();

                    if (_moveDirection.x == 0f && _moveDirection.z == 0f)
                    {
                        _dodgeDirection = _parentTransform.forward;
                    }
                    else
                    {
                        _dodgeDirection = _moveDirection;
                    }

                    _animator.SetBool("Attack", false);
                    _isDodgeSpeedDown = false;
                }
                else if (behaviour == Behaviour.Update)
                {
                    if (!_isDodgeSpeedDown)
                    {
                        Vector3 velocity = _dodgeDirection * _dodgeSpeed;
                        _rb.velocity = Vector3.Lerp(_rb.velocity, velocity, _speedLerp * Time.deltaTime);
                    }
                    else
                    {
                        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _dodgeSpeedDown * Time.deltaTime);
                    }

                    Quaternion rotation = Quaternion.LookRotation(_dodgeDirection);
                    _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, _rotateLerp * Time.deltaTime);
                }
                break;

            case BerserkerState.Hit:
                if (behaviour == Behaviour.Enter)
                {
                    _stopHitForce = false;
                }
                else if (behaviour == Behaviour.Update)
                {
                    if (!_stopHitForce)
                    {
                        Vector3 velocity = _hitForceDirection.normalized * _hitForce;
                        _rb.velocity = Vector3.Lerp(_rb.velocity, velocity, _speedLerp * Time.deltaTime);
                    }
                    else
                    {
                        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _speedLerp * Time.deltaTime);
                    }

                    Quaternion rotation = Quaternion.LookRotation(-_hitForceDirection);
                    _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, _rotateLerp * Time.deltaTime);
                }
                break;

            case BerserkerState.Death:
                if (behaviour == Behaviour.Enter)
                {
                    LevelLoader.Instance.LoadCurrentScene();
                }
                else if (behaviour == Behaviour.Update)
                {
                    _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _speedLerp * Time.deltaTime);
                }
                break;

            default:
                break;
        }
    }

    //
    public void setState(BerserkerState state)
    {
        _state = state;
    }

    // hit
    public void Hit(Vector3 hitForceDirection)
    {
        _hitForceDirection = hitForceDirection;
    }

    void Event_StopHitForce()
    {
        _stopHitForce = true;
    }

    // dodge state
    void Event_Dodge_SpeedDown()
    {
        _isDodgeSpeedDown = true;
    }

    void Event_PlayerDodgeClip()
    {
        if (_clipDodge)
        {
            _audioSource.PlayOneShot(_clipDodge, _volumeDodge);
        }
    }

    // all attack
    IEnumerator AllowAttackTransition(float delay)
    {
        yield return new WaitForSeconds(delay);

        _animator.SetBool("Attack Transition", true);
    }

    void Event_TurnOnDamage()
    {
        _damage.gameObject.SetActive(true);
    }

    void Event_TurnOffDamage()
    {
        _damage.gameObject.SetActive(false);
    }

    // attack 1 state
    void Event_Attack1_Move()
    {
        _allowAttack1Moving = true;
    }

    void Event_Attack1_StopMove()
    {
        _allowAttack1Moving = false;
    }

    void Event_PlayAttack1Clip()
    {
        if (_clipAttack1)
        {
            _audioSource.PlayOneShot(_clipAttack1, _volumeAttack1);
        }
        if (_clipVoiceAttack1)
        {
            _audioSource.PlayOneShot(_clipVoiceAttack1, _volumeVoiceAttack1);
        }
    }

    // attack 2 state
    void Event_Attack2_Move()
    {
        _allowAttack2Moving = true;
    }

    void Event_Attack2_StopMove()
    {
        _allowAttack2Moving = false;
    }

    void Event_PlayAttack2Clip()
    {
        if (_clipAttack2)
        {
            _audioSource.PlayOneShot(_clipAttack2, _volumeAttack2);
        }
        if (_clipVoiceAttack2)
        {
            _audioSource.PlayOneShot(_clipVoiceAttack2, _volumeVoiceAttack2);
        }
    }

    // attack 3 state
    void Event_Attack3_Move()
    {
        _allowAttack3Moving = true;
    }

    void Event_Attack3_StopMove()
    {
        _allowAttack3Moving = false;
    }

    void Event_PlayAttack3Clip()
    {
        if (_clipAttack3)
        {
            _audioSource.PlayOneShot(_clipAttack3, _volumeAttack3);
        }
        if (_clipVoiceAttack3)
        {
            _audioSource.PlayOneShot(_clipVoiceAttack3, _volumeVoiceAttack3);
        }
    }
}
