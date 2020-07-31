using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SWizard_State
{
    Idle,
    ChasePlayer,
    SummonFire,
    RunningRandom,
    Death
}

public enum SWizard_Mode
{
    RunRandom,
    SummonFire
}

public class SWizard_Delegate : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _rotateSpeed = 5f;
    public float RotateSpeed { get { return _rotateSpeed; } }

    [SerializeField] Rigidbody _rb;
    public Rigidbody Rb { get { return _rb; } }

    [Header("Game AI")]
    [SerializeField] NavMeshAgent _agent;
    public NavMeshAgent Agent { get { return _agent; } }

    [Header("Other")]
    [SerializeField] SWizard _parent;
    public SWizard Parent { get { return _parent; } }

    [SerializeField] Animator _animator;

    [SerializeField] List<GameObject> _meteorPrefs;


    // game Logic
    Player _player;
    [HideInInspector] public SWizard_State State;
    [HideInInspector] public SWizard_Mode Mode;
    [HideInInspector] public int _summonCount;

    private void Start()
    {
        _player = Player.Instance;

        Mode = SWizard_Mode.SummonFire;
        _agent.isStopped = true;
    }

    private void Update()
    {
        // rb
    }


    public void Event_SummonMeteor()
    {
        _summonCount++;
        if (_summonCount < 2)
        {
            if (Random.Range(0, 2) == 0)
            {
                Mode = SWizard_Mode.RunRandom;
                _summonCount = 0;
            }
        }
        else
        {
            Mode = SWizard_Mode.RunRandom;
            _summonCount = 0;
        }

        // spawn
        foreach (GameObject obj in _meteorPrefs)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                EnemyMeteor meteor = obj.GetComponent<EnemyMeteor>();
                meteor.AfterSetActive();
                break;
            }
        }
    }

    void Event_Disable()
    {
        _parent.gameObject.SetActive(false);
    }
}
