using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameSetting : MonoBehaviour
{
    // singleton
    static GameSetting _instance;
    public static GameSetting Instance { get { return _instance; } }

    static public readonly int SlowestFPS = 40;

    void Start()
    {
        _instance = this;
        //if (_instance == null)
        //{
        //    _instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }
}
