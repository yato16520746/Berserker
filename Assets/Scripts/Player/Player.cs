using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{   
    // singleton
    static Player _instance;
    public static Player Instance { get { return _instance; } }

    [SerializeField] Animator _animator;

    // movement
    Vector3 _moveDirection;
    public Vector3 MoveDirection { get { return _moveDirection; } }

    [SerializeField] Rigidbody _rb;
    public Vector3 RbVelocity { get { return _rb.velocity; } }

    float threshold = 0f;

    [SerializeField] PlayerStamina _stamina;

    [Header("Mouse input")]
    [SerializeField] float _rangeMouse = 30f;
    [SerializeField] LayerMask _mouseMask;
    Ray _mouseRay;
    RaycastHit _mouseHit;

    Vector3 _mousePosition;
    public Vector3 MousePosition { get { return _mousePosition; } }

    [Header("Mouse cursor")]
    [SerializeField] Texture2D _cursorTexture;
    [SerializeField] CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] Vector2 _hotSpot = Vector2.zero;

    [Header("Audio")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _errorClip;

    // pistol transform default
    // -0.06474289   -0.02103081   0.001257472
    // 0   290   90.00001
    // 1   1   1


    private void Awake()
    {
        // set singleton for Player
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);

        }
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        //Cursor.SetCursor(_cursorTexture, _hotSpot, cursorMode);
    }

    private void Update()
    {
        // A-D-W-S Input
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
        if (_moveDirection != Vector3.zero)
        {
            _animator.SetBool("Moving", true);
            threshold = 1f;
        }
        else
        {
            threshold = Mathf.Lerp(threshold, 0f, 10f * Time.deltaTime);
            if (threshold < 0.3f)
            {
                _animator.SetBool("Moving", false);
            }
            //_animator.SetBool("Moving", false);
        }



        _mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_mouseRay, out _mouseHit, _rangeMouse, _mouseMask))
        {
            _mousePosition = _mouseHit.point;
        }

        // dive forward input
        bool dive = Input.GetKeyDown(KeyCode.Space);
        if (dive && _stamina.CanDive)
        {
            _animator.SetBool("Dive Forward", true);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _audioSource.PlayOneShot(_errorClip);
            }
            _animator.SetBool("Dive Forward", false);
        }
    }

    private void LateUpdate()
    {
        // Mouse Input
        bool fire = Input.GetMouseButton(0);
        if (fire && _stamina.CanShoot)
        {
            _animator.SetBool("Firing", true);
            _animator.SetBool("Cannot Stop Fire", true);
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _audioSource.PlayOneShot(_errorClip);
            }
            _animator.SetBool("Firing", false);
        }
    }

    public Vector3 RandomPositionNearMe(float radius)
    {
        Vector3 direction = Random.insideUnitSphere;
        direction.y = 0f;
        Vector3 position = transform.position + direction.normalized * radius;

        NavMeshHit hit;
        NavMesh.SamplePosition(position, out hit, radius, 1);

        return hit.position;
    }

    public void PlayErrorAudio()
    {
        _audioSource.PlayOneShot(_errorClip);
    }

    public void ForceIdle(float time)
    {
        _animator.SetBool("Force Idle", true);

        StartCoroutine(StopForceIdleAfter(time));
    }

    IEnumerator StopForceIdleAfter(float time)
    {
        yield return new WaitForSeconds(time);

        _animator.SetBool("Force Idle", false);
    }
}
