using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    static ShakeCamera _instance;
    public static ShakeCamera Instance { get { return _instance; } }

    public float power = 0.2f;
    public float duration = 0.2f;
    public float slowDownAmount = 1;
    private bool shouldShake;
    public bool ShouldShake
    {
        get { return shouldShake; }
        set { shouldShake = value; }
    }
    private float initialDuration;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
        initialDuration = duration;

        _instance = this;
    }

    private void Update()
    {
        Shake();
    }

    void Shake()
    {
        if (shouldShake)
        {
            if (duration > 0f)
            {
                transform.localPosition = startPosition + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
            }
            else
            {
                shouldShake = false;
                duration = initialDuration;
                transform.localPosition = startPosition;

            }
        } // if we shoud shake camera

    } // shake
}
