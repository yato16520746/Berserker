using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [Space]

    [SerializeField] int _maxHealth = 100;
    [SerializeField] int _health = 100;

    [Space]
    [SerializeField] int _defaultMask = 0;
    [SerializeField] int _enemyHealthMask = 10;
    [SerializeField] string _enemyName;

    [Header("On Animation")]
    [SerializeField] int _deadTypeCount = 3;
    [SerializeField] bool _useDeadTrigger = false;

    public bool IsDead
    {
        get
        {
            if (_health <= 0)
            {
                return true;
            }

            return false;
        }
    }

    private void Start()
    {
        _health = _maxHealth;
    }

    public bool IsLessThan(float percent)
    {
        float realPercent = (float)_health / _maxHealth;
        
        if (realPercent < percent)
        {
            return true;
        }

        return false;
    }

    public void FullHealth()
    {
        _health = _maxHealth;
        gameObject.layer = _enemyHealthMask;
        
    }

    public void AddDamage(int amount)
    {
        if (_health <= 0)
        {
            return;
        }

        // trừ máu quái, nếu máu <= 0 thì destroy
        _health -= amount;

        // chạy animation nhấp nháy
        if (_animator)
        {
            _animator.SetTrigger("Get Hit");
        }

        // hiện trên canvas
        EnemyHealthCanvas.Instance.set_Value(_health, _maxHealth, _enemyName);

        // chạy animation death 
        if (_health <= 0)
        {
            if (_useDeadTrigger)
            {
                _animator.SetTrigger("Dead");
            }
            else
            {
                _animator.SetBool("Dead", true);
            }
   
            _animator.SetInteger("Dead Type", Random.Range(0, _deadTypeCount));

            gameObject.layer = _defaultMask;     
        }
    }
}
