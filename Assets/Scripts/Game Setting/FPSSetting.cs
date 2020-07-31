using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSSetting : MonoBehaviour
{
    [SerializeField] int _fps = 60;
    [SerializeField] bool _forceFPS;
    [SerializeField] bool _checkFps = true;
    [SerializeField] TextMeshProUGUI _textMeshPro;
    private float _frequency = 1.0f;
    float _count;

    // Start is called before the first frame update
    void Start()
    {

        if (_forceFPS)
        {
            Application.targetFrameRate = _fps;
        }

        if (_checkFps)
        {
            StartCoroutine(CheckFPS());
        }
        else
        {
            _textMeshPro.text = "";
        }
    }


    private IEnumerator CheckFPS()
    {
        for (; ; )
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(_frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            float currentFPS = frameCount / timeSpan;

            // Display it
            if (_textMeshPro)
            {
                _textMeshPro.text = "FPS: " + currentFPS;
            }
        }
    }
}
