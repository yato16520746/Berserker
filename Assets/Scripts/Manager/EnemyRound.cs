using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRound : MonoBehaviour
{
    static EnemyRound _currentRound;
    public static EnemyRound CurrentRound { get { return _currentRound; } }

    [SerializeField] List<EnemyWave> _waves;
    int _totalEnemy;
    [SerializeField] List<int> _activateNextWave;


    private void Start()
    {
        _totalEnemy = 0;
        foreach (EnemyWave wave in _waves)
        {
            _totalEnemy += wave.TotalEnemyAmount;
        }
    }

    public void StartRound()
    {
        _currentRound = this;

        _waves[0].SpawnEnemy();
    }

    public void WhenAnEnemyDisable()
    {
        if (_activateNextWave.Count <= 0)
        {
            return;
        }

        _activateNextWave[0]--;
        if (_activateNextWave[0] <= 0)
        {
            _activateNextWave.RemoveAt(0);
         

            if (_waves.Count > 0)
            {
                _waves.RemoveAt(0);

                if (_waves.Count > 0)
                {
                    _waves[0].SpawnEnemy();
                }
            }
        }

        _totalEnemy--;
        if (_totalEnemy == 0)
        {
            // repare for next Round
            LevelManager.Instance.RepareForNextRound();
        }
    }
    
    //

    //public static int MonsterAmount = 0;

    //[Space]
    //[SerializeField] float _delayTime = 1.25f;
    //Vector3 _position;
    //[SerializeField] float _timeSpawn = 4f;

    //[Space]
    //[SerializeField] GameObject _enemyPref;

    //[Space]
    //[SerializeField] GameObject _spawningFX;

    //private void Awake()
    //{
    //    MonsterAmount = 0;
    //}

    //private void Start()
    //{
    //    StartCoroutine(SpawnEnemy(Random.Range(_timeSpawn - 0.2f, _timeSpawn + 0.2f)));
    //}

    //private void Update()
    //{
    //    _timeSpawn -= 0.2f * Time.deltaTime;
    //    if (_timeSpawn < 1.5f)
    //    {
    //        _timeSpawn = 1.5f;
    //    }
    //}

    //IEnumerator SpawnEnemy(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);

    //    if (MonsterAmount < 3)
    //    {

    //        _position = new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6));

    //        Instantiate(_spawningFX, _position + new Vector3(0f, 0.1f, 0f), _spawningFX.transform.rotation);
    //    }

    //    // spawn enenmy GO
    //    StartCoroutine(SpawnEnemyGO(_delayTime));
    //}

    //IEnumerator SpawnEnemyGO(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);

    //    if (MonsterAmount < 3)
    //    {
    //        Instantiate(_enemyPref, _position, _enemyPref.transform.rotation);
    //    }

    //    // spawn enemy again
    //    StartCoroutine(SpawnEnemy(Random.Range(2f, 5f)));
    //}
}
