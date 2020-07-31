using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup : MonoBehaviour
{
    [SerializeField] PlayerBullet _bullet;
    [SerializeField] GameObject _shootFX;

    private void OnEnable()
    {
        _bullet.SetDirection(transform.forward);
        _bullet.WhenEnable();

        _shootFX.SetActive(true);
    }
}
