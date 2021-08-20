using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //에너미 상태
    public enum EnemyState
    {
        Idle,
        Move,
        Damaged,
        Die,
        Freeze
    }

    [Header("Attributes")]
    public float speed;
    public int maxHp;
    public int gold;
    public float removalTime;

    [Header("UI")]
    public Slider hpSlider;
    public Text damageText;

    public GameObject mainObject;

    private EnemyState _enemyState;
    public EnemyState _EnemyState
    {
        get { return _enemyState; }
        set { _enemyState = value; }
    }

    private Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    int _hp;
    int wavePointIndex = 0;
    bool isSlowdown;
    Animator anim;

    WaitForSeconds dieWait;

    void Awake()
    {
        dieWait = new WaitForSeconds(removalTime);
    }

    void OnEnable()
    {
        //활성화 마다 초기화를 해준다.
        Init();
        CancelInvoke("Duration");
    }

    void Update()
    {
        //상태에 따른 호출
        switch (_EnemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Freeze:
                Freeze();
                break;
        }

        //데미지 텍스트 효과
        if (damageText.enabled) if(damageText.fontSize < 32)    damageText.fontSize++;

        //현재 체력
        hpSlider.value = (float)_hp / (float)maxHp;
    }

    public void Init()
    {
        _hp = maxHp;
        wavePointIndex = 0;
        target = MovePoints.points[0];
        transform.forward = target.position - transform.position;
        anim = transform.GetComponentInChildren<Animator>();
        _EnemyState = EnemyState.Idle;
        damageText.enabled = false;
        isSlowdown = false;
        mainObject.GetComponent<Renderer>().material = EnemyManager.em.defaultMaterial;
    }

    //Idle 상태
    void Idle()
    {
        _EnemyState = EnemyState.Move;
        anim.SetTrigger("IdleToMove");
    }

    //Move 상태
    void Move()
    {
        float currentSpeed = (!isSlowdown ? speed : (float)(speed * 0.65));
        Vector3 dir = (target.position - transform.position).normalized;
        
        transform.Translate(dir * currentSpeed * Time.deltaTime, Space.World);

        Quaternion lookRotaion = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotaion, Time.deltaTime * (speed / 2));

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
            GetNextMovePoint();
    }

    //Die 상태
    void Die()
    {
        if (this.gameObject.activeInHierarchy)
            StartCoroutine(DieProcess());
    }

    //Hit 상태
    public void HitEnemy(int damage)
    {
        if (_EnemyState == EnemyState.Die) return;

        _hp -= damage;

        if (this.gameObject.activeInHierarchy)
            StartCoroutine(DamageProcess(damage));

        if (_hp <= 0)
        {
            _EnemyState = EnemyState.Die;
            Die();
        }
    }

    //Freeze 상태
    public void Freeze()
    {
        anim.SetTrigger("MoveToIdle");
        mainObject.GetComponent<Renderer>().material = EnemyManager.em.freezeMaterial;
        StartCoroutine(FreezeProcess());
    }

    void GetNextMovePoint()
    {
        if (wavePointIndex >= MovePoints.points.Length - 1)
        {
            if (_EnemyState == EnemyState.Die) return;
            LifeManager.lm.LifeDisable();
            EnemyManager.em.EnemyDisable(gameObject);
            return;
        }
        wavePointIndex++;
        target = MovePoints.points[wavePointIndex];
    }

    public void Burn()
    {
        try
        {
            InvokeRepeating("Duration", 1.5f, 1.75F);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        
    }

    void Duration()
    {
        HitEnemy(7);
        mainObject.GetComponent<Renderer>().material = EnemyManager.em.flameMaterial;
        if (this.gameObject.activeInHierarchy)
            StartCoroutine(SetDefaultMaterial());
    }

    public void Slowdown()
    {
        isSlowdown = true;
        StartCoroutine(SetSpeed());
    }

    IEnumerator DieProcess()
    {
        
        anim.SetTrigger("MoveToDie");
        PlayerManager.pm.GoldAcheive(gold);
        yield return dieWait;
        EnemyManager.em.EnemyDisable(gameObject);
    }

    readonly WaitForSeconds damageWait = new WaitForSeconds(0.71f);
    IEnumerator DamageProcess(int damage)
    {
        damageText.text = damage.ToString();
        damageText.fontSize = 8;
        damageText.enabled = true;
        yield return damageWait;
        damageText.enabled = false;
    }

    readonly WaitForSeconds freezeeWait = new WaitForSeconds(0.21f);
    IEnumerator FreezeProcess()
    {
        yield return new WaitForSeconds(0.21f);
        mainObject.GetComponent<Renderer>().material = EnemyManager.em.defaultMaterial;
        _EnemyState = EnemyState.Idle;
    }

    readonly WaitForSeconds materialWait = new WaitForSeconds(0.11f);
    IEnumerator SetDefaultMaterial()
    {
        yield return materialWait;
        mainObject.GetComponent<Renderer>().material = EnemyManager.em.defaultMaterial;
    }

    readonly WaitForSeconds speedWait = new WaitForSeconds(1.8f);
    IEnumerator SetSpeed()
    {
        yield return speedWait;
        isSlowdown = false;
    }
}
