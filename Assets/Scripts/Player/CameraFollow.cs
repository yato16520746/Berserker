using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;

    [SerializeField] Transform _target;
    Transform _player;

    [SerializeField] float _smoothSpeed = 22.5f;
    float _originSmoothSpeed;
    [SerializeField] Vector3 offset;

    [Space]
    [SerializeField] float _top;
    [SerializeField] float _bottom;
    [SerializeField] float _right;
    [SerializeField] float _left;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _player = _target;
        _originSmoothSpeed = _smoothSpeed;
        //offset = transform.position - _target.position;
    }

    public void ChangeTargetForTime(Transform newTarget, float time)
    {
        _target = newTarget;
        _smoothSpeed = 3f;

        StartCoroutine(ReturnToPlayer(time - 0.8f));
    }

    IEnumerator ReturnToPlayer(float time)
    {
        yield return new WaitForSeconds(time);

        StartCoroutine(OriginSmooth(0.8f));
        _target = _player;
    }

    IEnumerator OriginSmooth(float time)
    {
        yield return new WaitForSeconds(time);

        _smoothSpeed = _originSmoothSpeed;
    }

    void FixedUpdate()
    {
        Vector3 newTarget = _target.position;
        if (newTarget.z > _top)
        {
            newTarget.z = _top;
        }
        if (newTarget.z < _bottom)
        {
            newTarget.z = _bottom;
        }
        if (newTarget.x > _right)
        {
            newTarget.x = _right;
        }
        if (newTarget.x < _left)
        {
            newTarget.x = _left;
        }

        Vector3 desiredPosition = newTarget + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPosition;

        //transform.LookAt(newTarget);
    }
}
