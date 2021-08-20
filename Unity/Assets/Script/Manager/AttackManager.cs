using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    #region 싱글톤화
    public static AttackManager am;
    private void Awake()
    {
        if (am == null) am = this;
    }
    #endregion

    public int poolSize;

    [Header("Attack Kinds")]
    public GameObject arrowFactory;
    public Queue<GameObject> arrowObjectPool;
    public GameObject bombFactory;
    public Queue<GameObject> bombObjectPool;
    public GameObject blueLaserFactory;
    public Queue<GameObject> blueLaserObjectPool;
    public GameObject redLaserFactory;
    public Queue<GameObject> redLaserObjectPool;
    public GameObject blackLaserFactory;
    public Queue<GameObject> blackLaserObjectPool;
    public GameObject iceCloudFactory;
    public Queue<GameObject> iceCloudObjectPool;
    public GameObject poisonExplodeFactory;
    public Queue<GameObject> poisonExplodeObjectPool;
    
    void Start()
    {
        //오브젝트 폴링
        InitObjectPooling();
    }

    //초기 오브젝트를 생성 및 큐에 담아둔다.
    void InitObjectPooling()
    {
        #region Arrow
        arrowObjectPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(arrowFactory, transform);
            obj.SetActive(false);
            arrowObjectPool.Enqueue(obj);
        }
        #endregion

        #region Bomb
        bombObjectPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bombFactory, transform);
            obj.SetActive(false);
            bombObjectPool.Enqueue(obj);
        }
        #endregion

        #region Mage BlueLaser
        blueLaserObjectPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(blueLaserFactory, transform);
            obj.SetActive(false);
            blueLaserObjectPool.Enqueue(obj);
        }
        #endregion

        #region Mage RedLaser
        redLaserObjectPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(redLaserFactory, transform);
            obj.SetActive(false);
            redLaserObjectPool.Enqueue(obj);
        }
        #endregion

        #region Mage BlackLaser
        blackLaserObjectPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(blackLaserFactory, transform);
            obj.SetActive(false);
            blackLaserObjectPool.Enqueue(obj);
        }
        #endregion

        #region Mage IceCloud
        iceCloudObjectPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(iceCloudFactory, transform);
            obj.SetActive(false);
            iceCloudObjectPool.Enqueue(obj);
        }
        #endregion

        #region Mage PoisonExplode
        poisonExplodeObjectPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(poisonExplodeFactory, transform);
            obj.SetActive(false);
            poisonExplodeObjectPool.Enqueue(obj);
        }
        #endregion
    }

    //화살 공격 활성화
    public Arrow ArrowActive()
    {
        Arrow arrow;
        if (arrowObjectPool.Count > 0)
        {
            GameObject obj = arrowObjectPool.Dequeue();
            arrow = obj.GetComponent<Arrow>();
            obj.SetActive(true);
            obj.transform.position = transform.position;
        }
        else
        {
            GameObject obj = Instantiate(arrowFactory, transform);
            arrow = obj.GetComponent<Arrow>();
            obj.SetActive(true);
            arrowObjectPool.Enqueue(obj);
        }
        return arrow;
    }

    //대포 공격 활성화
    public Bomb BombActive()
    {
        Bomb bomb;
        if (bombObjectPool.Count > 0)
        {
            GameObject obj = bombObjectPool.Dequeue();
            if (obj.activeSelf)
                return null;
            bomb = obj.GetComponent<Bomb>();
            obj.SetActive(true);
            obj.transform.position = transform.position;
        }
        else
        {
            GameObject obj = Instantiate(bombFactory, transform);
            bomb = obj.GetComponent<Bomb>();
            obj.SetActive(true);
            bombObjectPool.Enqueue(obj);
        }
        return bomb;
    }

    //마법 LV_1 공격 활성화
    public Mage BlueLaserActive()
    {
        Mage mage;
        if (blueLaserObjectPool.Count > 0)
        {
            GameObject obj = blueLaserObjectPool.Dequeue();
            mage = obj.GetComponent<Mage>();
            obj.SetActive(true);
            obj.transform.position = transform.position;
        }
        else
        {
            GameObject obj = Instantiate(blueLaserFactory, transform);
            mage = obj.GetComponent<Mage>();
            obj.SetActive(true);
            blueLaserObjectPool.Enqueue(obj);
        }
        return mage;
    }

    //마법 LV_2 공격 활성화
    public Mage RedLaserActive()
    {
        Mage mage;
        if (redLaserObjectPool.Count > 0)
        {
            GameObject obj = redLaserObjectPool.Dequeue();
            mage = obj.GetComponent<Mage>();
            obj.SetActive(true);
            obj.transform.position = transform.position;
        }
        else
        {
            GameObject obj = Instantiate(redLaserFactory, transform);
            mage = obj.GetComponent<Mage>();
            obj.SetActive(true);
            redLaserObjectPool.Enqueue(obj);
        }
        return mage;
    }

    //마법 LV_3 공격 활성화
    public Mage BlackLaserActive()
    {
        Mage mage;
        if (blackLaserObjectPool.Count > 0)
        {
            GameObject obj = blackLaserObjectPool.Dequeue();
            mage = obj.GetComponent<Mage>();
            obj.SetActive(true);
            obj.transform.position = transform.position;
        }
        else
        {
            GameObject obj = Instantiate(blackLaserFactory, transform);
            mage = obj.GetComponent<Mage>();
            obj.SetActive(true);
            blackLaserObjectPool.Enqueue(obj);
        }
        return mage;
    }

    //마법 A 공격 활성화
    public Mage IceCloudActive()
    {
        Mage mage;
        if (iceCloudObjectPool.Count > 0)
        {
            GameObject obj = iceCloudObjectPool.Dequeue();
            mage = obj.GetComponent<Mage>();
            obj.SetActive(true);
            obj.transform.position = transform.position;
        }
        else
        {
            GameObject obj = Instantiate(iceCloudFactory, transform);
            mage = obj.GetComponent<Mage>();
            obj.SetActive(true);
            iceCloudObjectPool.Enqueue(obj);
        }
        return mage;
    }

    //마법 B 공격 활성화
    public Mage poisonExplodeActive()
    {
        Mage mage;
        if (poisonExplodeObjectPool.Count > 0)
        {
            GameObject obj = poisonExplodeObjectPool.Dequeue();
            mage = obj.GetComponent<Mage>();
            obj.SetActive(true);
            obj.transform.position = transform.position;
        }
        else
        {
            GameObject obj = Instantiate(poisonExplodeFactory, transform);
            mage = obj.GetComponent<Mage>();
            obj.SetActive(true);
            poisonExplodeObjectPool.Enqueue(obj);
        }
        return mage;
    }

    //화살 공격 비활성화
    public void ArrowDisable(GameObject obj)
    {
        arrowObjectPool.Enqueue(obj);
        obj.SetActive(false);
    }

    //대포 공격 비활성화
    public void BombDisable(GameObject obj)
    {
        bombObjectPool.Enqueue(obj);
        obj.SetActive(false);
    }

    //마법 공격 LV_1 비활성화
    public void BlueLaserDisable(GameObject obj)
    {
        blueLaserObjectPool.Enqueue(obj);
        obj.SetActive(false);
    }

    //마법 공격 LV_2 비활성화
    public void RedLaserDisable(GameObject obj)
    {
        redLaserObjectPool.Enqueue(obj);
        obj.SetActive(false);
    }

    //마법 공격 LV_3 비활성화
    public void BlackLaserDisable(GameObject obj)
    {
        blackLaserObjectPool.Enqueue(obj);
        obj.SetActive(false);
    }

    //마법 공격 A 비활성화
    public void IceCloudDisable(GameObject obj)
    {
        iceCloudObjectPool.Enqueue(obj);
        obj.SetActive(false);
    }

    //마법 공격 B 비활성화
    public void PoisonExplodeDisable(GameObject obj)
    {
        poisonExplodeObjectPool.Enqueue(obj);
        obj.SetActive(false);
    }
}
