using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using MySql.Data;
using System;

public class CanonTower : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
    public float range = 25f;
    public float fireRate = 0.5f;
    public float turnSpeed;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform towerRotate;
    public Transform firePoint;

    private float fireCountdown = 0f;
    WaitForSeconds wait = new WaitForSeconds(0.5f);

    void Start()
    {
        //주기적으로 에너미 탐색
        InvokeRepeating("UpdateTarget", 1f, 3.0f);
    }

    //에너미를 탐색한다.
    void UpdateTarget()
    {
        GameObject[] enemyObjs = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortesDistance = Mathf.Infinity;
        GameObject nearesEnemy = null;

        //선택정렬로 필드에 존재하는 에너미를 전부 검사한다.
        foreach (GameObject enemyObj in enemyObjs)
        {
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy._EnemyState == Enemy.EnemyState.Die) continue;

            //가장 가까운 에너미를 선택한다.
            float distanceToEnemy = Vector3.Distance(transform.position, enemyObj.transform.position);
            if (distanceToEnemy < shortesDistance)
            {
                shortesDistance = distanceToEnemy;
                nearesEnemy = enemyObj;
            }
        }

        //가장 가깝고 공격 범위에 근접한 에너미를 선택한다.
        if (nearesEnemy != null && shortesDistance <= range)
        {
            target = nearesEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target == null || !target.gameObject.activeSelf) return;

        //에너미를 향해 회전한다.
        Vector3 dir = (target.position - towerRotate.position).normalized;
        Quaternion lookRotaion = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(towerRotate.rotation, lookRotaion, Time.deltaTime * turnSpeed).eulerAngles;
        towerRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);

        //발사 주기가 된다면 화살 공격 활성화한다.
        if (fireCountdown <= 0f)
        {
            AddBomb();

            fireCountdown = 2f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void AddBomb()
    {
        Bomb bomb = AttackManager.am.BombActive();
        if (bomb != null)
        {
            bomb.Seek(target, firePoint.position, tag);
            target = null;
        }
    }

    IEnumerator ContinuousFiring()
    {
        yield return wait;
        AddBomb();
    }
}
