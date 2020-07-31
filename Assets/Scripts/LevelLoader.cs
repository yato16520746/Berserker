using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    static LevelLoader _instance;
    public static LevelLoader Instance { get { return _instance; } }

    [Header("My objects")]
    [SerializeField] Animator _animator;

    [Space]
    [SerializeField] float _transitionTime = 1.05f;
    bool _loadScene = false;
    float _count = 0;
    string _sceneName = "";

    float _maxVolume;

    private void Start()
    {
        _instance = this;
    }

    private void Update()
    {   
        if (_loadScene)
        {
            _count += Time.unscaledDeltaTime;
            if (_count > _transitionTime)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        _animator.SetTrigger("End"); //animator
        _loadScene = true;
        _sceneName = sceneName;
    }

    public void LoadCurrentScene()
    {
        _animator.SetTrigger("End"); // animator
        _loadScene = true;
        _sceneName = SceneManager.GetActiveScene().name;
    }
}
