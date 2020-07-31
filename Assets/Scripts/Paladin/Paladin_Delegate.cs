using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Paladin_State
{
    Idle,
    Walk,
    Attack1,
    Attack2,
    Attack3,
    Attack4,
    Attack5,
    Dash,
    Death
}

public class Paladin_Delegate : MonoBehaviour
{
    [Header("AI movement")]
    [SerializeField] NavMeshAgent _agent;
    public NavMeshAgent Agent { get { return _agent; } }

    [SerializeField] Animator _animator;

    [HideInInspector] public Paladin_State State;

    [SerializeField] float _walkSpeed;
    public float WalkSpeed { get { return _walkSpeed; } }

    [SerializeField] float _speedLerp = 30;
    public float SpeedLerp { get { return _speedLerp; } }

    [SerializeField] float _rotateLerp = 20;
    public float RotateLerp { get { return _rotateLerp; } }

    [HideInInspector] public float StateSpeed;

    float _originAngularSpeed;


    [Header("Attack 2")]

    [SerializeField] float _distance_Attack2 = 1.3f;
    public float Distance_Attack2 { get { return _distance_Attack2; } }

    [SerializeField] PaladinAttack2Warning _attack2_warning;
    public PaladinAttack2Warning Attack2_warning { get { return _attack2_warning; } }

    [HideInInspector] public bool Moving_Attack2;


    [Header("Attack 3")]

    [SerializeField] PaladinCheckAttack _checkAttack3;
    public PaladinCheckAttack CheckAttack3 { get { return _checkAttack3; } }

    [SerializeField] float _attack3_moveSpeed1;
    public float Attack3_moveSpeed1 { get { return _attack3_moveSpeed1; } }

    [SerializeField] float _attack3_moveSpeed2;
    public float Attack3_moveSpeed2 { get { return _attack3_moveSpeed2; } }

    [HideInInspector] public int Attack3_MovingType;


    [Header("Attack 4")]
    [SerializeField] Transform _circleTransform;
    [SerializeField] DarknessCircle _darknessCircle;

    //
    [Header("Attack 5")]
    [SerializeField] ParticleSystem _handTrails1;
    [SerializeField] ParticleSystem _handTrails2;

    //
    [Header("Attack 1")]

    [SerializeField] PaladinCheckAttack _checkAttack1;
    public PaladinCheckAttack CheckAttack1 { get { return _checkAttack1; } }

    [HideInInspector] public bool StopMoving_Attack1;

    [SerializeField] float _attack1_MoveSpeed = 20f;
    public float Attack1_MoveSpeed { get { return _attack1_MoveSpeed; } }

    //
    [Header("Calculate combo")]
    [SerializeField] EnemyHealth _bossHealth;
    int _combo;
    [HideInInspector] public bool KeepingIdle;
    int _attack1Amount;
    int _cannotAttack1;
    int _attack2Amount;
    int _cannotAttack2;
    int _cannotAttack3;
    int _cannotAttack4;
    float _cannotSummon;
    [HideInInspector] public GameObject Monster;

    //
    [Header("Effect")]
    [SerializeField] GameObject _explosion1;
    [SerializeField] GameObject _explosion2;
    [SerializeField] ParticleSystem _swordTrails;
    [SerializeField] SphereCastDamage _swordDamage;


    [Header("Audio")]
    [SerializeField] AudioClip _summonClip;
    [SerializeField] AudioClip _demoClip;
    [SerializeField] AudioClip _clip;
    [SerializeField] AudioClip _attackClip;
    [SerializeField] AudioClip _attack4Clip;
    [SerializeField] AudioSource _source;

    //
    void Start()
    {
        _originAngularSpeed = _agent.angularSpeed;
        _combo = 2;

        // ko chạy, vì nó bị disable rồi
        CheckAttack1.gameObject.SetActive(false); 
        CheckAttack3.gameObject.SetActive(false);

        Event_SwordTrailsStop();
        Event_TurnOfSwordDamage();

        Event_HandTrailsStop();
        //

        Monster = null;

        _cannotAttack1 = 0;
        _cannotAttack2 = 0;
        _cannotAttack3 = 0;
        _cannotAttack4 = 0;
    }

    private void Update()
    {
        _cannotSummon -= Time.deltaTime;
        if (Monster != null && Monster.activeSelf)
        {
            _cannotSummon = 7f;
        }
    }

    // Idle
    public void Event_Idle_CalculateCombo()
    {
        // rest + reset combo
        if (_combo == 0)
        {
            KeepingIdle = true;

            // HP: 66% - 100%
            if (!_bossHealth.IsLessThan(0.66f))
            {
                _combo = 1;
            }
            // HP: 25 % - 66%
            else if (!_bossHealth.IsLessThan(0.15f))
            {
                _combo = Random.Range(2,4);
            }
            // HP < 15%
            else
            {
                _combo = 999999;
            }
        }
        else
        {
            KeepingIdle = false;
        }

        _cannotAttack1--;
        _cannotAttack2--;
        _cannotAttack3--;
        _cannotAttack4--;

        if (_attack1Amount >= 2)
        {
            _cannotAttack1 = 1;
        }
        if (_attack2Amount >= 3)
        {
            _cannotAttack2 = 1;
        }

        // check position Player for attack 3
        Vector3 distance = PlayerHealth.Instance.transform.position - transform.position;
        if (distance.magnitude > 6f)
        {
            _cannotAttack3 = 1;
        }

        List<int> randomList = new List<int>();
        if (_cannotAttack1 <= 0)
        {
            randomList.Add(1); // đi bộ + lao đến chém
        }
        if (_cannotAttack2 <= 0)
        {
            randomList.Add(2); // tốc biến đến
        }
        if (_cannotAttack3 <= 0)
        {
            randomList.Add(3); // lộn + 3 chém

        }
        if (_cannotAttack4 <= 0)
        {
            randomList.Add(4); // vòng lửa
        }

        if (_bossHealth.IsLessThan(0.66f))
        {
            if (_cannotSummon <= 0f)
            {
                randomList.Add(5);
            }
        }


        // random attack
        _combo--;
        if (randomList.Count == 0)
        {
            Debug.LogError("Combo cua Boss bi loi");
            return;
        }

        int randomIndex = Random.Range(0, randomList.Count);
        _animator.SetInteger("Attack Type", randomList[randomIndex]);

        if (randomList[randomIndex] == 1)
        {
            _attack1Amount++;
        }
        else
        {
            _attack1Amount = 0;
        }
        if (randomList[randomIndex] == 2)
        {
            _attack2Amount++;
        }
        else
        {
            _attack2Amount = 0;
        }
        if (randomList[randomIndex] == 3)
        {
            _cannotAttack3 = 1;
        }
        if (randomList[randomIndex] == 4)
        {
            _cannotAttack4 = 2;
        }
    }

    //
    public void Event_PlayerSummonClip()
    {
        _source.PlayOneShot(_summonClip, 1f);
    }

    // Attack 5
    public void Event_HandTrailsStop()
    {
        _handTrails1.Stop();
        _handTrails2.Stop();
    }

    public void Event_HandTrailsPlay()
    {
        _handTrails1.Play();
        _handTrails2.Play();
    }

    void Event_Summon()
    {
        Monster = LevelManager.Instance.CallingSpawnEnemy(EnemyType.Wizard);
    }


    // Attack 4
    public void Event_ActiveDarknessCircle()
    {
        _darknessCircle.StartSpawn(_circleTransform.position, 1f);
    }

    public void Event_Attack4Audio()
    {
        _source.PlayOneShot(_attack4Clip);
    }

    // Attack 3
    public void Event_EnableCheckAttack3()
    {
        CheckAttack3.gameObject.SetActive(true);
    }

    public void Event_DisableCheckAttack3()
    {
        CheckAttack3.gameObject.SetActive(false);
    }

    public void Event_setAttack3MovingType(int type)
    {
        Attack3_MovingType = type;
    }

    // Attack 2
    public void Event_Attack2_DefineDirection()
    {
        // random position from NavMesh - near Player
        Vector2 direction2D = Random.insideUnitCircle;
        Vector3 direction = new Vector3(direction2D.x, 0f, direction2D.y);

        Attack2_warning.gameObject.SetActive(true);
        Attack2_warning.Direction = direction.normalized;
    }

    public void Event_FlashToNearPlayer()
    {
        // random position from NavMesh - near Player
        Vector3 playerPos = PlayerHealth.Instance.transform.position;

        Vector3 randomPosition = playerPos + Attack2_warning.Direction * Distance_Attack2; ;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, 10f, 1);
        _agent.transform.position = hit.position;

        // look player
        Vector3 vector = playerPos - transform.position;
        vector.y = 0f;
        if (vector.magnitude > 0.0001f)
        {
            _agent.transform.rotation = Quaternion.LookRotation(vector);
        }
    }

    // Attack 1
    public void Event_StopMoving_Attack1()
    {
        StopMoving_Attack1 = true;
    }

    //
    public void Event_TurnOnAgentRotate()
    {
        _agent.angularSpeed = _originAngularSpeed;
    }

    public void Event_TurnOffAgentRotate()
    {
        _agent.angularSpeed = 0;
    }

    public void Event_setStateSpeed(float speed)
    {
        StateSpeed = speed;
    }

    void Event_PowerUp()
    {
        _explosion1.SetActive(true);
        _explosion2.SetActive(true);
        _source.PlayOneShot(_clip);

        ShakeCamera.Instance.ShouldShake = true;

        LevelManager.Instance.PlayBossFightAudio();
    }

    void Event_PlayDemoClip()
    {
        _source.PlayOneShot(_demoClip);
    }

    void Event_SwordTrailsStop()
    {
        _swordTrails.Stop();
    }

    void Event_SwordTrailsPlay()
    {
        _swordTrails.Play();
    }

    void Event_TurnOnSwordDamage()
    {
        _swordDamage.gameObject.SetActive(true);
        _source.PlayOneShot(_attackClip);
    }

    void Event_TurnOfSwordDamage()
    {
        _swordDamage.gameObject.SetActive(false);
    }
}
