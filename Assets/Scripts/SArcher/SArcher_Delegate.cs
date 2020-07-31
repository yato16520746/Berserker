using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SArcherState
{
    Appear,
    Idle,
    ChasingPlayer,
    DrawArrow,
    Shoot,
    RunningRandom,
    Death
}

public class SArcher_Delegate : MonoBehaviour
{

    [Header("Shoot Arrow")]
    [SerializeField] Transform _arrowTransform;
    [SerializeField] GameObject _arrowPref;
    [SerializeField] GameObject _arrowGraphic;

    [SerializeField] Transform _shootTransform;
    public Transform ShootTransform { get { return _shootTransform; } }

    [SerializeField] GameObject _warningLine;
    int _shootArrowCount;

    [Space]
    [SerializeField] Transform _parentTransform;
    public Transform ParentTransform { get { return _parentTransform; } }

    [Header("Game AI")]
    // game AI
    [SerializeField] NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent { get { return _navMeshAgent; } }

    [SerializeField] Collider _collider;
    public Collider Collider { get { return _collider; } }  

    [SerializeField] Animator _animator;

    // trạng thái của SkeletonB
    [HideInInspector] public SArcherState State;
    [HideInInspector] public bool RotatingWhenShootArrow;

    private void Start()
    {
        DisableArrowGraphic();
        Event_DisableWarningLine();
    }

    // Events
    public void ShootArrow()
    {
        GameObject arrow = Instantiate(_arrowPref, _arrowTransform.position, _arrowTransform.rotation);
        arrow.transform.localScale = _arrowTransform.lossyScale;

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.SetUp(_parentTransform.rotation);

        DisableArrowGraphic();

        // check amount of continuos shoot arrow
        _shootArrowCount++;
        if (_shootArrowCount >= 3)
        {
            _animator.SetBool("Running Random", true);
            _shootArrowCount = 0;
        }
        else
        {
            // now keeping shoot arrow or not?
            if (Random.Range(0, 2) < 1)
            {
                _animator.SetBool("Running Random", true);
                _shootArrowCount = 0;
            }
        }

        RotatingWhenShootArrow = false;
    }

    public void EnableArrowGraphic()
    {
        _arrowGraphic.SetActive(true);
    }

    private void DisableArrowGraphic()
    {
        _arrowGraphic.SetActive(false);
    }

    void Event_EnableWarningLine()
    {
        _warningLine.SetActive(true);
    }

    void Event_DisableWarningLine()
    {
        _warningLine.SetActive(false);
    }
    
    void Event_Destroy()
    {
        ParentTransform.gameObject.SetActive(false);
    }
}
