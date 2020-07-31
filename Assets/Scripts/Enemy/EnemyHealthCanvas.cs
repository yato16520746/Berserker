using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthCanvas : MonoBehaviour
{
    // singleton
    static EnemyHealthCanvas _instance;
    static public EnemyHealthCanvas Instance
    {
        get
        {
            return _instance;
        }
    }

    // slider
    [SerializeField] Slider _slider;
    [SerializeField] Text _nameText;

    // animator
    [Space]
    [SerializeField] Animator _animator;


    void Start()
    {
        if (_instance)
        {
            Destroy(_instance.gameObject);
        }

        _instance = this;
    }

    void Update()
    {
    }

    // lúc enemy mất máu thì set các thông số để hiển thị lên
    public void set_Value(float currentHealth, float maxHealth, string enemyName)
    {
        _slider.maxValue = maxHealth;
        _slider.value = currentHealth;
        _nameText.text = enemyName;

        _animator.SetTrigger("Show");
    }
}
