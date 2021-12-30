using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public delegate void GameStatusEvent();
public delegate void BattleBeginEvent(float time);
public delegate void SetClockEvent(float time);
public delegate void StartSliding(int begin, int end);

[System.Serializable]
public class Round
{
    [Range(0, 30)]
    public int miniMonster;
    [Range(0, 30)]
    public int flyMonster;
    [Range(0, 30)]
    public int boss;
    [Range(1.0f, 300.0f)]
    public float time;
    [Range(1.0f, 300.0f)]
    public float spawnTime;
    [Range(0.1f, 30.0f)]
    public float spawnRadius;

    public Transform[] spawnPoints;

    private int numOfSpawnPos;
    public float startedTime = .0f;
    public int spawnedMiniMonster;
    public int spawnedFlyMonster;
    public int spawnedBoss;

    

    public void Start()
    {
        numOfSpawnPos = spawnPoints.Length;
        startedTime = .0f;
    }

    public void Update()
    {

    }

}


public class GameManager_Ka : MonoBehaviour
{
    [Header("Monster")]
    [SerializeField]
    //[Range(0.01f, 30.0f)]
    //float radius = 10f;

    //[SerializeField]
    //[Range(1, 30)] 
    //int minienemyMaximun = 10;
    //[SerializeField]
    //[Range(1, 30)]
    //int flyingenemyMaximun = 10;

    public Transform miniEnemy;
    public Transform flyingEnemy;
    public int curMiniEnemy = 0;
    public int curFlyingEnemy = 0;

    [Header("Game Control")]
    [Range(1, 100)]
    public int health = 20; 
    public static int curHealth = 20;

    [Range(1f, 3000f)]
    public float gameTime = 20.0f;

    public static float curTime = .0f;

    [Header("Baby")]
    public AudioClip cry1, cry2;

    public Round[] rounds;
    AudioSource audioManager;

    public enum GameStatus { startMenu, cutscene1, planeAni, cutscene2, babyAni, cutscene3, battle, end };
    public static GameStatus gameStatus = GameStatus.startMenu;


    public static event GameStatusEvent StartCutscene1;
    public static event GameStatusEvent PlaneAnimation;
    public static event GameStatusEvent BabyAnimation;
    public static event GameStatusEvent GameEnd;
    public static event GameStatusEvent GameOver;
    public static event BattleBeginEvent BattleBegin;
    public static event StartSliding StartCutscene;
    public static event SetClockEvent SetMoonClock;

    public GameObject babyUI;
    public GameObject hand;
    public GameObject GameEndUI;
    public GameObject GameOverUI;
    public GameObject lights;
    public GameObject instrutionUI;

    //public static UnityEvent StartCutscene1;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GetComponent<AudioSource>();

        StartMenu.StartButtonClipped    += OnStartGameButtonDown;
        ImageSlider.EndCutscene1        += FinishCutscene1;
        Plane.FinishPlaneAnimation += FinishPlaneAnimation;
        // shicai add this line
        BigPlane.FinishPlaneAnimation      += FinishPlaneAnimation;
        Baby.FinishBabyAnimation        += FinishBabyAnimation;
        GameEnd                         += GameWin;
        StartCutscene1                  += DisableLineRenderer;
        GameOver                        += DisplayGameOverUI;

        curHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStatus == GameStatus.cutscene1)
        {
            instrutionUI.SetActive(false);
        }

        if(gameStatus == GameStatus.battle)
        {
            if (curHealth <= 0)
            {
                gameStatus++;
                GameOver.Invoke();
            }
            Battle();
        }
    }

    void Battle()
    {
        curTime += Time.deltaTime;
        if (curTime > gameTime)
        {
            gameStatus++;
            GameEnd.Invoke();
        }
        else
        {
            SetBabyUI();
            SpawnEnemy();
        }
    }

    //************************************ Enemy Spawn **************************************************************//

    public static int enemyKilled; 

    int round = 0;
    void SpawnEnemy()
    {
        int numOfMiniMonster    = rounds[round].miniMonster;
        int numOfFlyMonster     = rounds[round].flyMonster;
        int numOfBoss           = rounds[round].boss;
        float spawnTime         = rounds[round].spawnTime;
        int numOfSpawnPos       = rounds[round].spawnPoints.Length;
        float spawnRadius       = rounds[round].spawnRadius;
        Transform[] spawnPoints = rounds[round].spawnPoints;
        if(rounds[round].spawnedMiniMonster < rounds[round].miniMonster)
            SpawnMini(numOfMiniMonster, spawnTime, numOfSpawnPos, spawnRadius, spawnPoints, miniEnemy, round);
        if (rounds[round].spawnedFlyMonster < rounds[round].flyMonster)
            SpawnFly(numOfFlyMonster, spawnTime, numOfSpawnPos, spawnRadius, spawnPoints, miniEnemy, round);

        rounds[round].startedTime += Time.deltaTime;

        SetMoonClock.Invoke((float)round / (float)rounds.Length);

        Debug.Log("enemyKilled: " + enemyKilled + "  EnemytoKill: " + (numOfMiniMonster + numOfFlyMonster + numOfBoss));

        if ((rounds[round].startedTime > rounds[round].time) || (enemyKilled >= numOfMiniMonster + numOfFlyMonster + numOfBoss))
        {
            Debug.Log("Round++" + enemyKilled);
            if (round < rounds.Length - 1)
            {
                round++;
                enemyKilled = 0;
            }
            else
            {
                gameStatus++;
                GameEnd.Invoke();
            }
        }
    }

    float miniTimer = .0f;
    private void SpawnMini(int numOfMiniMonster, float spawnTime, int numOfSpawnPos, float spawnRadius, Transform[] spawnPoints, Transform miniEnemy, int round)
    {
        miniTimer += Time.deltaTime;
        if (miniTimer > (spawnTime / numOfMiniMonster))
        {
            int spawnPosIndex = (int)(Random.Range(.0f, numOfSpawnPos));
            Vector3 enemyPosi = Quaternion.Euler(0f, Random.Range(0, 360), 0f) * new Vector3(spawnRadius, 0f, 0f) + spawnPoints[spawnPosIndex].position;
            Instantiate(miniEnemy, enemyPosi, Quaternion.identity);
            rounds[round].spawnedMiniMonster++;
            miniTimer = .0f;
        }
    }

    float flyTimer = .0f;
    private void SpawnFly(int numOfFlyMonster, float spawnTime, int numOfSpawnPos, float spawnRadius, Transform[] spawnPoints, Transform miniEnemy, int round)
    {
        flyTimer += Time.deltaTime;
        if (flyTimer > (spawnTime / numOfFlyMonster))
        {
            int spawnPosIndex = (int)(Random.Range(.0f, numOfSpawnPos));
            Vector3 enemyPosi = Quaternion.Euler(0f, Random.Range(0, 360), 0f) * new Vector3(spawnRadius, 1f, 0f) + spawnPoints[spawnPosIndex].position;
            Instantiate(flyingEnemy, enemyPosi, Quaternion.identity);
            rounds[round].spawnedFlyMonster++;
            flyTimer = .0f;
        }
    }

    //*****************************************************************************************************************//

    static void OnStartGameButtonDown()
    {
        if(gameStatus == GameStatus.startMenu)
        {
            gameStatus++;
            Debug.Log("OnStartGameButtonDown");
            StartCutscene1.Invoke();
        }
    }

    void FinishCutscene1()
    {
        if (gameStatus == GameStatus.cutscene1)
        {
            gameStatus++;
            PlaneAnimation.Invoke();
        }

        if (gameStatus == GameStatus.cutscene2)
        {
            gameStatus++;
            BabyAnimation.Invoke();
        }

        if (gameStatus == GameStatus.cutscene3)
        {
            gameStatus++;
            lights.SetActive(true);
            BattleBegin.Invoke(gameTime);
        }
    }

    static void FinishBabyAnimation()
    {
        if (gameStatus == GameStatus.babyAni)
        {
            gameStatus++;
            StartCutscene.Invoke(0, 5);
        }
    }

    static void FinishPlaneAnimation()
    {
        if (gameStatus == GameStatus.planeAni)
        {
            gameStatus++;
            StartCutscene.Invoke(0, 3);
        }
    }

    void GameWin()
    {
        SetMoonClock.Invoke(1.0f);
        GameEndUI.SetActive(true);
    }

    void DisplayGameOverUI()
    {
        GameOverUI.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Monster")
        {
            //collision.gameObject.SetActive(false);
            curHealth--;
            int a = Random.Range(1, 2);
            if (a == 1)
            {
                audioManager.PlayOneShot(cry1);
            }
            else
            {
                audioManager.PlayOneShot(cry2);
            }

        }
    }

    void SetBabyUI()
    {
        babyUI.SetActive(true);
        babyUI.GetComponent<UnityEngine.UI.Slider>().value = (float)curHealth / (float)health;
    }

    private void DisableLineRenderer()
    {
        hand.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRRayInteractor>().enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

}
