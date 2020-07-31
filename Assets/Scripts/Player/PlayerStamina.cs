using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] Animator _animator;

    [SerializeField] float _maxStamina = 100f;
    [SerializeField] float _recoverySpeed = 30f;
    [SerializeField] float _delayTime = 0.4f;
    float _stamina;
    public float Stamina
    {
        set
        {
            _stamina = value;
            if (_stamina < 5f)
            {
                if (!_redAlert)
                {
                    _redAlert = true;
                    _animator.SetBool("Red", true);
                }
            }
            else if (_stamina >= _maxStamina)
            {
                if (_redAlert)
                {
                    _redAlert = false;
                    _animator.SetBool("Red", false);
                }

            }
        }
        get
        {
            return _stamina;
        }
    }
    bool _redAlert;

    [SerializeField] float _shootStamina = 10f;
    [SerializeField] float _diveStamina = 20f;

    float _cannotRecovery;

    [Space]
    [SerializeField] Player_Delegate _delegate;

    private void Start()
    {
        Stamina = _maxStamina;

        _slider.maxValue = _maxStamina;
        _slider.value = _stamina;
    }

    public bool CanShoot
    {
        get
        {
            if (_redAlert)
            {
                return false;
            }

            if (Stamina > _shootStamina)
            {
                return true;
            }

            return false;
        }
    }

    public void Shoot()
    {
        Stamina -= _shootStamina;
    }

    public bool CanDive
    {
        get
        {
            if (_redAlert)
            {
                return false;
            }

            if (Stamina > _shootStamina)
            {
                return true;
            }

            return false;
        }
    }

    public void Dive()
    {
        Stamina -= _diveStamina;
    }



    private void Update()
    {
        if (_delegate.State == PlayerState.DiveForward || _delegate.State == PlayerState.Idle_Aiming ||
         _delegate.State == PlayerState.Walk_Aiming)
        {
            _cannotRecovery = _delayTime;
        }

        _cannotRecovery -= Time.deltaTime;
        if (_cannotRecovery < 0)
        {
            Stamina += _recoverySpeed * Time.deltaTime;
            Stamina = Mathf.Clamp(_stamina, 0f, _maxStamina);
        }

        //_slider.value = _stamina;
        _slider.value = Mathf.Lerp(_slider.value, Stamina, 7f * Time.deltaTime);
    }
}
