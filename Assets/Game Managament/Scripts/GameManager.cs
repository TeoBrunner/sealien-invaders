using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    static int playerLives = 3;
    [SerializeField] GameObject livePicsHolder;
    [SerializeField] GameObject[] livePics;
    [SerializeField] AudioClip playerExplosion;
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;
    [SerializeField] GameObject restartText;
    [SerializeField] GameObject pauseText;
    public static float timeScaleOnStart = 1.2f;
    public static float slowdownForEnemy = 0.98f;
    public static float timeScaleActual;
    //public static float slowdownModifier;
    [SerializeField] int currentWaveNumber;
    [SerializeField] static int enemiesCount;
    [SerializeField] static int lasersCount;
    [SerializeField] int maximumWaves;
    [SerializeField] static int iterationNumber = 0;
    [SerializeField] static float iterationSpeedUp = 0.2f;
    [SerializeField] GameObject playerBoom;
    private static int scoresActual;
     private static int scoresTop;
    public static bool isWaveActive = false;
    public static bool isGameOver = false;
    public static bool isPaused = false;
    public static bool canInput = false;
    GameObject player;
    Canons canons;
    Engine engine;
    StarsParallax starsParallax;
    WaveManager waveManager;
    AudioSource audioSource;
    [SerializeField] AudioClip FTLsound;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        Instance.LoadTop();
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = timeScaleOnStart;
        waveManager = GetComponent<WaveManager>();
        maximumWaves = waveManager.GetMaximumWaves();
        player = GameObject.FindGameObjectWithTag("Player");
        engine = player.GetComponent<Engine>();
        canons = player.GetComponent<Canons>();
        starsParallax = GameObject.Find("BackGround").GetComponent<StarsParallax>();
        StartGame();


        if (scoresTop > 0) highScoreText.text = ("!! " + scoresTop);
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause")) Pause();
        if (Input.GetKeyDown(KeyCode.R) && isGameOver && canInput) TrueRestart();
        if (GetEnemiesCount() == 0 && GetlasersCount() == 0 && isWaveActive && !isGameOver) StartCoroutine(JumpToNextWave());
        //Debug.Log(Time.timeScale);
        //UpdateTimeScale();
    }
    private void LoadTop()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            scoresTop = data.scoresTop;
        }
    }

    private void SaveTop() 
    {
        SaveData data = new SaveData();
        data.scoresTop = scoresTop;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    void Pause()
    {
        float _timeScale = timeScaleActual;
        if (isPaused)
        {
            
            Time.timeScale = _timeScale;
            if (Time.timeScale < 0.3f) Time.timeScale = 0.4f;
            pauseText.SetActive(false);
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0.0001f;
            pauseText.SetActive(true);
            isPaused = true;
        }
    }
    void StartGame()
    {
        currentWaveNumber = 0;
        StartCoroutine(StartPlayerMove());
        //StartCoroutine(JumpToNextWave());
        //waveManager.SpawnWave(0);
        enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (scoresTop > 0) highScoreText.text = ("!! " + scoresTop);
    }
    public void GameOver()
    {
        audioSource.PlayOneShot(playerExplosion);
        restartText.SetActive(true);
        Debug.Log("Game is Over");
        Instantiate(playerBoom, player.transform.position, Quaternion.identity);
        player.SetActive(false);
        isGameOver = true;
    }
    public static void ClearLasers()
    {
        GameObject[] _lasers;
        _lasers = GameObject.FindGameObjectsWithTag("PlayerLaser");
        foreach (var laser in _lasers)
        {
            Destroy(laser);
        }
        _lasers = GameObject.FindGameObjectsWithTag("EnemyLaser");
        foreach (var laser in _lasers)
        {
            Destroy(laser);
        }
    }
    public static void ClearEnemies()
    {
        GameObject[] _enemies;
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in _enemies)
        {
            Destroy(enemy);        
        }
    }
    public void TrueRestart()
    {
        GameObject.Find("Game Manager").GetComponent<GameManager>().scoreText.text = ("# 0");
        canInput = false;
        //Restart();
        //waveManager.Restart();
        //starsParallax.Restart();
        //canons.Restart();
        //engine.Restart();
        if (scoresTop > 0) highScoreText.text = ("!! " + scoresTop);
        UpdateScore(-scoresActual);
        ClearLasers();
        //ClearEnemies();
        currentWaveNumber = -1;
        iterationNumber = 0;
        StartCoroutine(JumpToNextWave());
        Debug.Log("Restarted");
        livePics[0].SetActive(true);
        livePics[1].SetActive(true);
        livePics[2].SetActive(true);
        playerLives = 3;
        canons.isReloaded = true;
        foreach (var heart in livePics)
        {
            StartCoroutine(HeartAnim(heart, true));
        }
    }
    public static void UpdateScore(int _addScore)
    {
        
        scoresActual += _addScore;
        if (scoresActual > scoresTop) scoresTop = scoresActual;
        if (scoresActual != 0 && scoresTop != 0) GameObject.Find("Game Manager").GetComponent<GameManager>().scoreText.text = ("# " + scoresActual);
        GameManager.Instance.SaveTop();
    }
    public void Restart()
    {
        Time.timeScale = timeScaleOnStart;
        timeScaleOnStart = 1.2f;
        slowdownForEnemy = 0.98f;
        iterationNumber = 0;
        iterationSpeedUp = 0.2f;
        currentWaveNumber = -1;

    }
    IEnumerator SpawnNextWave()
    {
        isWaveActive = false;
        yield return new WaitForSeconds(0.5f);
        ClearLasers();
        //currentWaveNumber++;
        if (currentWaveNumber == maximumWaves)
        {
            iterationNumber++;
            currentWaveNumber = 0;
            waveManager.IterationSpeedUp(iterationNumber);
        }
        waveManager.SpawnWave(currentWaveNumber);
        //isWaveActive = true;
        //GameManager.UpdateTimeScale();
    }
    IEnumerator RevivePlayer()
    {
        canInput = false;
        yield return new WaitForSeconds(3);
        player.transform.position -= Vector3.up * 3;
        player.SetActive(true);

        engine.SwitchToSTL();
        //поднять игрока
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.005f);
            player.transform.position += Vector3.up * 0.06f;
        }
        canInput = true;
        canons.isReloaded = true;
    }
    IEnumerator StartPlayerMove()
    {
        player.transform.position -= Vector3.up * 3;
        yield return new WaitForSeconds(0.3f);
        engine.SwitchToSTL();
        //поднять игрока
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.005f);
            player.transform.position += Vector3.up * 0.06f;
        }
        //engine.SwitchToSTL();
        //waveManager.SpawnWave(0);

        //заспавнить и опустить новую волну
       // transform.position = new Vector3(transform.position.x, waveManager.startY, transform.position.z);
        transform.position = Vector3.up * 13;
        StartCoroutine(SpawnNextWave());
        //GameManager.UpdateTimeScale();
        while (transform.position.y > 0)
        {
            yield return new WaitForSeconds(0.005f);
            transform.position -= Vector3.up * 0.1f;
        }
        isWaveActive = true;
        canInput = true;
    }

    public int GetCurrentWaveNumber()
    {
        return currentWaveNumber;
    }
    public static int GetlasersCount()
    {
        lasersCount = GameObject.FindGameObjectsWithTag("EnemyLaser").Length + GameObject.FindGameObjectsWithTag("PlayerLaser").Length;
        return lasersCount;
    }
    public static int GetEnemiesCount()
    {
        enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        return enemiesCount;
    }
    public IEnumerator HeartAnim(GameObject heart, bool show)
    {
        for (int i = 0; i < 5; i++)
        {            
            yield return new WaitForSeconds(0.2f);
            if (heart.active) heart.SetActive(false);
            else heart.SetActive(true);
        }
        heart.SetActive(show);
        yield return new WaitForSeconds(1);
    }
    public void DamagePlayer(int _dmg)
    {
        playerLives -= _dmg;
        livePicsHolder.SetActive(true);
        livePics[playerLives].SetActive(false);
        StartCoroutine(HeartAnim(livePics[playerLives], false));
        if (playerLives < 1) 
        { 
            GameOver(); 
        }
        else
        {
            audioSource.PlayOneShot(playerExplosion);
            Debug.Log("Ship Exploded");
            Instantiate(playerBoom, player.transform.position, Quaternion.identity);
            player.SetActive(false);
            StartCoroutine(RevivePlayer());
        }

    }
    public static void UpdateTimeScale()
    {
        //slowdownModifier = Mathf.Pow(slowdownForEnemy, GetEnemiesCount
        float _timeScale = (timeScaleOnStart + iterationNumber*iterationSpeedUp) *Mathf.Pow(slowdownForEnemy, GetEnemiesCount());
        //float _timeScale = timeScaleOnStart * Mathf.Pow(slowdownForEnemy, enemiesCount);
        timeScaleActual = _timeScale;
        Time.timeScale = timeScaleActual;
        Debug.Log(Time.timeScale);
    }

    IEnumerator JumpToNextWave()
    {
        currentWaveNumber++;
        if (currentWaveNumber > 0) UpdateScore(1);
        canInput = false;
        restartText.SetActive(false);
        isWaveActive = false;
        ClearLasers();
        StarsParallax parallax = GameObject.Find("BackGround").GetComponent<StarsParallax>();
        //поднять камеру
        GameObject _camera = GameObject.Find("Main Camera");
        float _cameraY = 0;
        //for (int i = 0; i < 10; i++)
        //{
        //    yield return new WaitForSeconds(0.1f);
        //    _camera.transform.position += (Vector3.up * 0.3f);
        //}

        //выключить игрока
        //GameObject _player = GameObject.FindGameObjectWithTag("Player");
        // _player.SetActive(false);
        engine.SwitchToFTL();

        //прогнать врагов
        if (isGameOver)
        {

            ClearLasers();
            while (transform.position.y < 15)
            {
                yield return new WaitForSeconds(0.025f);
                transform.position += Vector3.up * 0.8f;
            }

        }

        if(FTLsound)audioSource.PlayOneShot(FTLsound);
        //удлиннить звёзды
        GameObject[] _stars = GameObject.FindGameObjectsWithTag("Star");
        float _scaleY = _stars[0].transform.localScale.y;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.05f);
            foreach (var star in _stars)
            {
                star.transform.localScale += (Vector3.up * 0.2f);
                parallax.parallaxSpeed += 0.01f;
            }
        }
        yield return new WaitForSeconds(1f);

        //укоротить звёзды
        _scaleY = _stars[0].transform.localScale.y;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.05f);
            foreach (var star in _stars)
            {
                star.transform.localScale -= (Vector3.up * 0.2f);
                parallax.parallaxSpeed -= 0.01f;
            }
        }
        engine.SwitchToSTL();

        //рестартнуть игрока
        if (isGameOver)
        {

            player.SetActive(true);
            player.transform.position -= Vector3.up * 3;
            yield return new WaitForSeconds(0.3f);
            engine.SwitchToSTL();
            //поднять игрока
            for (int i = 0; i < 50; i++)
            {
                yield return new WaitForSeconds(0.005f);
                player.transform.position += Vector3.up * 0.06f;
            }
            isGameOver = false;
            canons.isReloaded = true;
        }


        //опустить камеру
        //for (int i = 0; i < 10; i++)
        //{
        //    yield return new WaitForSeconds(0.1f);
        //    _camera.transform.position -= (Vector3.up * 0.3f);
        //}

        //заспавнить и опустить волну
        ClearEnemies();
        //transform.position = new Vector3(transform.position.x, waveManager.startY, transform.position.z);
        transform.position = Vector3.up*15;
        StartCoroutine(SpawnNextWave());
        //GameManager.UpdateTimeScale();

        while (transform.position.y > 0)
        {
            yield return new WaitForSeconds(0.005f);
            transform.position -= Vector3.up * 0.1f;
        }
        isWaveActive = true;
        canInput = true;
        
    }


    [System.Serializable]
    class SaveData
    {
        public int scoresTop;
    }

}