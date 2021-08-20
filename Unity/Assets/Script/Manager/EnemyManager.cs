using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    #region 싱글톤화
    public static EnemyManager em;
    private void Awake()
    {
        //싱글톤화
        if (em == null) em = this;
    }
    #endregion

    [Serializable]
    public struct EnemyFactory
    {
        public GameObject bat;
        public GameObject dragon;
        public GameObject slime;
        public GameObject turtle;
        public GameObject orc;
        public GameObject skeleton;
        public GameObject golem;
        public GameObject boss;
    }

    [Header("Wave Info")]
    public Transform spawnPoint;
    public float timeBetweenWaves = 3f;

    [SerializeField]
    public int waveIndex = 0;

    [Header("Enemy Kinds")]
    public EnemyFactory enemyFactory;

    [Header("State Material")]
    public Material defaultMaterial;
    public Material freezeMaterial;
    public Material flameMaterial;

    Queue<GameObject> batObjectPool;
    Queue<GameObject> dragonObjectPool;
    Queue<GameObject> slimeObjectPool;
    Queue<GameObject> turtleObjectPool;
    Queue<GameObject> skeletonObjectPool;
    Queue<GameObject> orcObjectPool;
    Queue<GameObject> golemObjectPool;

    float countdown = 1f;
    int poolSize = 12;
    int maxWaveIndex = 8;

    readonly WaitForSeconds waveWait = new WaitForSeconds(1f);
    readonly WaitForSeconds delayWait = new WaitForSeconds(0.6f);
    private void Start()
    {
        //오브젝트 폴링
        InitObjectPooling();
    }

    //웨이브 정보를 초기화 한다.
    public void InitWave()
    {
        countdown = 1f;
        waveIndex = 0;
    }

    //라운드가 진행중 호출되며 웨이브에 따른 함수
    public void Wave()
    {
        //전체 라운드 클리어 한다면 플레이어 승리
        if (StageManager.sm.StageIndex > StageManager.sm.maxStageIndex)
        {
            SoundManager.sm.MenuAudio();
            SettingManager.sm.Pause();
            NoticeManager.nm.Win.SetActive(true);
        }

        //웨이브 종료 상태인지 체크
        if (waveIndex >= maxWaveIndex)
        {
            int iCount = gameObject.transform.childCount;
            Transform[] trChild = new Transform[iCount];

            for (int i = 0; i < iCount; i++)
            {
                trChild[i] = gameObject.transform.GetChild(i);
            }
            int cnt = trChild.Where(x => x.transform.gameObject.activeSelf).Count();

            //웨이브 종료
            if (cnt <= 0)    StageManager.sm.WaveEnd();
        }
        else
        {
            //에너미 계속 활성화
            if (countdown <= 0f)
            {
                StartCoroutine(SpawnWave());
                countdown = timeBetweenWaves;
            }
            countdown -= Time.deltaTime;
        }
    }

    delegate void setObj(Queue<GameObject> pool, GameObject obj);

    //에너미 생성 및 큐에 담아두기
    void InitObjectPooling()
    {
        setObj add = (Queue<GameObject> pool, GameObject obj) =>
        {
            obj.transform.position = spawnPoint.position;
            obj.SetActive(false);
            pool.Enqueue(obj);
        };

        batObjectPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bat = Instantiate(enemyFactory.bat, transform);
            add(batObjectPool, bat);
        }

        dragonObjectPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject dragon = Instantiate(enemyFactory.dragon, transform);
            add(dragonObjectPool, dragon);
        }

        slimeObjectPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject slime = Instantiate(enemyFactory.slime, transform);
            add(slimeObjectPool, slime);
        }

        turtleObjectPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject turtle = Instantiate(enemyFactory.turtle, transform);
            add(turtleObjectPool, turtle);
        }

        skeletonObjectPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject skeleton = Instantiate(enemyFactory.skeleton, transform);
            add(skeletonObjectPool, skeleton);
        }

        orcObjectPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject turtle = Instantiate(enemyFactory.orc, transform);
            add(orcObjectPool, turtle);
        }

        golemObjectPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject golem = Instantiate(enemyFactory.golem, transform);
            add(golemObjectPool, golem);
        }
    }

    //Round_1 박쥐 활성화
    public void BatActive()
    {
        if (batObjectPool.Count > 0)
        {
            GameObject enemy = batObjectPool.Dequeue();
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
        }
        else
        {
            GameObject enemy = Instantiate(enemyFactory.bat, transform);
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
            batObjectPool.Equals(enemy);
        }
    }

    //Round_2 용 활성화
    public void DragonActive()
    {
        if (dragonObjectPool.Count > 0)
        {
            GameObject enemy = dragonObjectPool.Dequeue();
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
        }
        else
        {
            GameObject enemy = Instantiate(enemyFactory.dragon, transform);
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
            dragonObjectPool.Equals(enemy);
        }
    }

    //Round_3 슬라임 활성화
    public void SlimeActive()
    {
        if (slimeObjectPool.Count > 0)
        {
            GameObject enemy = slimeObjectPool.Dequeue();
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
        }
        else
        {
            GameObject enemy = Instantiate(enemyFactory.slime, transform);
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
            slimeObjectPool.Equals(enemy);
        }
    }

    //Round_4 거북이 활성화
    public void TurtleActive()
    {
        if (turtleObjectPool.Count > 0)
        {
            GameObject enemy = turtleObjectPool.Dequeue();
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
        }
        else
        {
            GameObject enemy = Instantiate(enemyFactory.turtle, transform);
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
            turtleObjectPool.Equals(enemy);
        }
    }

    //Round_5 해골 활성화
    public void SkeletonActive()
    {
        if (skeletonObjectPool.Count > 0)
        {
            GameObject enemy = skeletonObjectPool.Dequeue();
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
        }
        else
        {
            GameObject enemy = Instantiate(enemyFactory.skeleton, transform);
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
            skeletonObjectPool.Equals(enemy);
        }
    }

    //Round_6 오크 활성화
    public void OrcActive()
    {
        if (orcObjectPool.Count > 0)
        {
            GameObject enemy = orcObjectPool.Dequeue();
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
        }
        else
        {
            GameObject enemy = Instantiate(enemyFactory.orc, transform);
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
            orcObjectPool.Equals(enemy);
        }
    }

    //Round_7 골렘 활성화
    public void GolemActive()
    {
        if (golemObjectPool.Count > 0)
        {
            GameObject enemy = golemObjectPool.Dequeue();
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
        }
        else
        {
            GameObject enemy = Instantiate(enemyFactory.golem, transform);
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
            golemObjectPool.Equals(enemy);
        }
    }

    //에너미 비활성화
    public void EnemyDisable(GameObject obj)
    {
        //라운드 별로 에너미가 다르기에 스테이지에 따른 에너미 비활성화
        switch (StageManager.sm.StageIndex)
        {
            case 1:
                batObjectPool.Enqueue(obj);
                obj.SetActive(false);
                break;
            case 2:
                dragonObjectPool.Enqueue(obj);
                obj.SetActive(false);
                break;
            case 3:
                slimeObjectPool.Enqueue(obj);
                obj.SetActive(false);
                break;
            case 4:
                turtleObjectPool.Enqueue(obj);
                obj.SetActive(false);
                break;
            case 5:
                skeletonObjectPool.Enqueue(obj);
                obj.SetActive(false);
                break;
            case 6:
                orcObjectPool.Enqueue(obj);
                obj.SetActive(false);
                break;
            case 7:
                golemObjectPool.Enqueue(obj);
                obj.SetActive(false);
                break;
        }
    }
    
    //웨이브가 종료 전까지 일정 시간동안 에너미 활성화를 한다.
    IEnumerator SpawnWave()
    {
        yield return delayWait;
        waveIndex++;

        switch (StageManager.sm.StageIndex)
        {
            case 1:
                for (int i = 0; i < waveIndex; i++)
                {
                    BatActive();
                    yield return waveWait;
                }
                break;
            case 2:
                for (int i = 0; i < waveIndex; i++)
                {
                    DragonActive();
                    yield return waveWait;
                }
                break;
            case 3:
                for (int i = 0; i < waveIndex; i++)
                {
                   SlimeActive();
                    yield return waveWait;
                }
                break;
            case 4:
                for (int i = 0; i < waveIndex; i++)
                {
                   TurtleActive();
                    yield return waveWait;
                }
                break;
            case 5:
                for (int i = 0; i < waveIndex; i++)
                {
                    SkeletonActive();
                    yield return waveWait;
                }
                break;
            case 6:
                for (int i = 0; i < waveIndex; i++)
                {
                    OrcActive();
                    yield return waveWait;
                }
                break;
            case 7:
                for (int i = 0; i < waveIndex; i++)
                {
                   GolemActive();
                    yield return waveWait;
                }
                break;
        }
    }
}
