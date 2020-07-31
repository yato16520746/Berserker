using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngelRequest : MonoBehaviour
{
    [SerializeField] Animator _angelAnimator;
    [SerializeField] LayerMask _playerHealthMask;
    [SerializeField] GameObject _canvas;
    [SerializeField] Collider _triggerCollider;
    bool _acceptRequest = false;

    [SerializeField] Text _text;

    int _currentRound;
    public int CurrentRound
    {
        get { return _currentRound; }
        set
        {
            _currentRound = value;
            _text.text = "Start Round " + _currentRound;  
        }
    }

    private void Start()
    {
        _canvas.SetActive(false);
        _currentRound = 1;
    }

    private void OnEnable()
    {
        _triggerCollider.enabled = true;
        _acceptRequest = false;
    }

    private void Update()
    {
        // check key
        if (_canvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _angelAnimator.SetTrigger("Disappear");
                _canvas.SetActive(false);
                _triggerCollider.enabled = false;
                _acceptRequest = true;
                PlayerHealth.Instance.FullHP();

                // activate enemy spawner
                LevelManager.Instance.ActivateEnemySpawner(CurrentRound);
                CurrentRound++;
            }

            if (Input.GetKey(KeyCode.L) && Input.GetKeyDown(KeyCode.Keypad1))
            {
                CurrentRound = 1;
            }
            if (Input.GetKey(KeyCode.L) && Input.GetKeyDown(KeyCode.Keypad2))
            {
                CurrentRound = 2;
            }
            if (Input.GetKey(KeyCode.L) && Input.GetKeyDown(KeyCode.Keypad3))
            {
                CurrentRound = 3;
            }
        }
    }

    void Event_Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_acceptRequest)
            return;

        if (((1 << other.gameObject.layer) & _playerHealthMask) != 0)
        {
            _canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_acceptRequest)
            return;

        if (((1 << other.gameObject.layer) & _playerHealthMask) != 0)
        {
            _canvas.SetActive(false);
        }
    }
}
