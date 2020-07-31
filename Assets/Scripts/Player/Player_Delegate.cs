using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle, 
    Run,
    Idle_Aiming,
    Walk_Aiming,
    DiveForward,
    LargeHit,
    Death
}

public class Player_Delegate : MonoBehaviour
{
    [SerializeField] Player _parent;
    public Player Parent { get { return _parent; } }

    [SerializeField] Rigidbody _rb;
    public Rigidbody Rb { get { return _rb; } }

    [SerializeField] Animator _animator;

    [Space]
    [HideInInspector] public PlayerState State;
    [HideInInspector] public bool SlowDive;

    [Header("Movement")]

    [SerializeField] float _runSpeed; // 6
    public float RunSpeed { get { return _runSpeed; } }

    [SerializeField] float _walkSpeed; // 3
    public float WalkSpeed { get { return _walkSpeed; } }

    [SerializeField] float _diveSpeed = 10;
    public float DiveSpeed { get { return _diveSpeed; } }

    [SerializeField] float _hitForce = 10f;
    public float HitForce { get { return _hitForce; } }
    [HideInInspector] public Vector3 HitForceDirection;

    // for velocity
    [SerializeField] float _speedLerp; // 20
    public float SpeedLerp { get { return _speedLerp; } }

    // for quaternion
    [SerializeField] float _rotateLerp; // 20
    public float RotateLerp { get { return _rotateLerp; } }

    [Header("Blend Tree Animation")]
    [SerializeField] Transform _gunLook;
    public Transform GunLook { get { return _gunLook; } }

    Vector3 _walkDirection;

    [Header("Shoot bullet")]
    [SerializeField] ObjectPool _bulletPool;

    readonly float _mouseRange = 0.3f;
    public float MouseRange { get { return _mouseRange; } }

    [SerializeField] Transform _shootPoint;

    [Space]
    [SerializeField] PlayerStamina _stamina;
    public PlayerStamina Stamina { get { return _stamina; } }

    [Header("Audio")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _diveForwardClip;


    private void Update()
    {
        // fire
        if (State == PlayerState.Idle_Aiming || State == PlayerState.Walk_Aiming)
        {
            _animator.SetBool("Fire", true);
        }
        else
        {
            _animator.SetBool("Fire", false);
        }
    }

    private void LateUpdate()
    {
        // rotate GunLook
        Vector3 mousePosition = Parent.MousePosition;
        Vector3 vector = mousePosition - GunLook.position;
        vector.y = 0f;
        if (vector.magnitude > 0.1f)
        {
            GunLook.rotation = Quaternion.LookRotation(vector);
        }

        // set blend tree animation
        Vector3 moveDirection = Parent.MoveDirection;
        Vector3 designedWalkDirection = GunLook.InverseTransformDirection(moveDirection);
        _walkDirection = Vector3.Lerp(_walkDirection, designedWalkDirection, 5f * Time.deltaTime);

        _animator.SetFloat("Direction X", _walkDirection.x);
        _animator.SetFloat("Direction Z", _walkDirection.z);
    }

    public void Event_ShootBullet()
    {
        // không cho bắn đạn lúc chuyển trạng thái
        if (State != PlayerState.Idle_Aiming && State != PlayerState.Walk_Aiming)
        {
            return;
        }

        // tạo hiệu ứng lúc ra đạn
        //Instantiate(_shootFXPref, _shootPoint.position, transform.rotation);

        // bắn đạn
        GameObject bullet = _bulletPool.Spawn(_shootPoint.position, transform.rotation);
        _stamina.Shoot();

        _animator.SetBool("Cannot Stop Fire", false);
    }

    public void Event_Dead()
    {
        State = PlayerState.Death;
    }

    public void Event_SlowDive()
    {
        SlowDive = true;
    }

    public void Event_CheckMouseError()
    {
        // Mouse Input
        bool fire = Input.GetMouseButton(0);
        if (fire && !_stamina.CanShoot)
        {
            Parent.PlayErrorAudio();
        }
    }

    void Event_DiveAudio()
    {
        _audioSource.PlayOneShot(_diveForwardClip, 0.4f);
    }
}
