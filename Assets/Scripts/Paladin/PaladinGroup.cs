using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PaladinGroup : MonoBehaviour
{
    [SerializeField] Animator _statueAnimator;
    [SerializeField] GameObject _paladin;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] NavMeshObstacle _obstacle;
    [SerializeField] Collider _statueCollider;

    [Space]
    [SerializeField] float _delayTime = 1.25f;
    bool _trigger = false;

    private void Start()
    {
        _statueAnimator.gameObject.SetActive(true);
        _paladin.SetActive(false);

        _agent.enabled = false;
    }

    public void Fight()
    {
        if (!_trigger)
        {
            _trigger = true;
            Trigger();

            if (Berserker.Instance)
                Berserker.Instance.ForceIdle(7.5f);
            CameraFollow.Instance.ChangeTargetForTime(_paladin.transform, 7.5f);
        }
    }

    void Trigger()
    {
        _statueAnimator.SetTrigger("Dissapear");
        _statueAnimator.SetTrigger("Move");
        _obstacle.enabled = false;
        _statueCollider.enabled = false;

        _paladin.SetActive(true);

        StartCoroutine(PaladinAppear(0.2f));
    }

    IEnumerator PaladinAppear(float delay)
    {
        yield return new WaitForSeconds(delay);

        //_statueAnimator.gameObject.SetActive(false);
        _agent.enabled = true;
    }
}
