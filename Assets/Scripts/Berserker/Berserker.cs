using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Berserker : MonoBehaviour
{
    // singleton
    static Berserker _instance;
    public static Berserker Instance { get { return _instance; } }

    [SerializeField] Rigidbody _rb;
    public Vector3 RbVelocity { get { return _rb.velocity; } }

    [SerializeField] Animator _animator;

    //
    private void Start()
    {
        _instance = this;
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
