using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] float xBorder;
    [SerializeField] float yBorder;
    [SerializeField] float defeatY;
    [SerializeField] float sideflyDelay;
    [SerializeField] float sideFlySpeed;
    [SerializeField] float step;
    [SerializeField] float tiltAngle;
    [SerializeField] EnemyWave[] waves;
    float sinusoidalPos = 0;
    public float startY;
    float actualstep;
    GameObject[] enemies = new GameObject[0];

    GameManager gameManager;

    [System.Serializable]
    public struct EnemyWave
    {

       public EnemySpecies[] species;

    }

    [System.Serializable]
     public struct EnemySpecies
    {
       public GameObject prefabOfSpecies;
       public int numberOfSpecies;

    }
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        //float _heightStep = (yBorder * 2) / (wave.Length + 1);
        //for (int i = 0; i < wave.Length; i++)
        //{
        //    var _enemyRow = wave[i];
        //    float _rowHeight = -yBorder + _heightStep * (i + 1);
        //    float _widthStep = (xBorder * 2) / (_enemyRow.numberOfSpecies+1);

        //    for (int j = 0; j < _enemyRow.numberOfSpecies; j++)
        //    {
        //        float _enemyWidth = -xBorder + _widthStep * (j + 1);
        //        Vector3 _enemyPos = new Vector3(_enemyWidth,_rowHeight);
        //        Instantiate(_enemyRow.prefabOfSpecies, transform.position+_enemyPos, transform.rotation, transform);
        //    }
        //}
        startY = transform.position.y;

        
        // step = -(startY - defeatY) / (secsForFirstWave / Time.fixedDeltaTime);
    }

    private void Update()
    {

    }
    void FixedUpdate()
    {
        
        foreach (var enemy in enemies)
        {
            if (enemy) enemy.transform.rotation = Quaternion.AngleAxis(tiltAngle * Mathf.Cos(sinusoidalPos * Mathf.PI), Vector3.down);
        }
        Move();
    }

   
    public void StartButton()
    {
        sinusoidalPos = 0;
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        GameObject[] _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var _enemy in _enemies)
        {
            Destroy(_enemy);
        }

        
        
        
        SpawnWave(0);
        //step = -(startY - defeatY + heightStep) / (secsForWave * 2 / Time.fixedDeltaTime);
    }

    public void SpawnWave(int _waveNumber)
    {
        ResetPosition();
        DeleteEnemies();


        //heightStep = (yBorder * 2) / (waves[_waveNumber].species.Length + 1);
        float _heightStep = 1.8f;
        
        for (int _rowNumber = 0; _rowNumber < waves[_waveNumber].species.Length; _rowNumber++)
        { 
            var _enemyRow = waves[_waveNumber].species[_rowNumber];
            float _rowHeight = yBorder - _heightStep * (_rowNumber);
            float _widthStep = (xBorder * 2) / (_enemyRow.numberOfSpecies + 1);
            for (int j = 0; j < _enemyRow.numberOfSpecies; j++)
            {
                float _enemyWidth = -xBorder + _widthStep * (j + 1);
                Vector3 _enemyPos = new Vector3(_enemyWidth, _rowHeight);
                var _enemy = Instantiate(_enemyRow.prefabOfSpecies, transform.position + _enemyPos, transform.rotation, transform) as GameObject;
                //_enemyRow.population.Add(_enemy);
                //_enemy.GetComponent<EnemyScript>().listIndex = j;
                //_enemy.GetComponent<EnemyScript>().rowNumber = _rowNumber;
            }

        }
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameManager.UpdateTimeScale();
    }
    void ResetPosition()
    {
        sinusoidalPos = 0;
       // transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }
    void DeleteEnemies()
    {
        GameObject[] _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var _enemy in _enemies)
        {
            Destroy(_enemy);
        }
    }
    void Move()
    {
        if (GameManager.isGameOver) actualstep = 0;
        else actualstep = step;
        sinusoidalPos += Time.fixedDeltaTime * sideFlySpeed;
        transform.position = new Vector3(sideflyDelay * Mathf.Sin(sinusoidalPos * Mathf.PI), transform.position.y + actualstep, transform.position.z);
    }

    public int GetMaximumWaves()
    {
        return waves.Length;
    }
    public void IterationSpeedUp(int _iteration)
    {
        step *= 1 + (_iteration * 0.3f);
    }
    public void Restart()
    {
        IterationSpeedUp(0);
    }

    private IEnumerator StartShooting(List<GameObject> _population)
    {
        bool _isRowExist = false;
        float _minDelay = _population[0].GetComponent<SimpleEnemyShooting>().minDelay;
        float _maxDelay = _population[0].GetComponent<SimpleEnemyShooting>().maxDelay;
        if (_population.Count != 0) _isRowExist = true;
        yield return new WaitForSeconds(1);
    }
}
