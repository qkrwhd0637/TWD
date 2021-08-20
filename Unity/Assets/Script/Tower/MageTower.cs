using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTower : MonoBehaviour
{    
    private Transform target;

    [Header("Attributes")]
    public float range;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public float turnSpeed = 10f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform firePoint;
    public GameObject effect;
    bool isAttack;

    void Start()
    {
        isAttack = false;
        //주기적으로 에너미 탐색
        InvokeRepeating("UpdateTarget", 0.5f, 0.5f);
    }

    //에너미를 탐색한다.
    void UpdateTarget()
    {
        GameObject[] enemyObjs = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortesDistance = Mathf.Infinity;

        //선택정렬로 필드에 존재하는 에너미를 전부 검사한다.
        foreach (GameObject enemyObj in enemyObjs)
        {
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy._EnemyState == Enemy.EnemyState.Die) continue;

            //가장 가까운 에너미를 선택한다.
            float distanceToEnemy = Vector3.Distance(transform.position, enemyObj.transform.position);
            if (distanceToEnemy < shortesDistance)
            {
                isAttack = true;
                return;
            }
        }
    }

    void Update()
    {
        if (!isAttack) return;
        //발사 주기가 된다면 화살 공격 활성화한다.
        if (fireCountdown <= 0f)
        {
            //해당 타워에 레벨에 따른 공격을 활성화 한다.
            Mage mage;
            switch (gameObject.tag)
            {
                case "MageTowerLv_1":
                    mage = AttackManager.am.BlueLaserActive();
                    mage.init(firePoint.position, 1);
                    break;
                case "MageTowerLv_2":
                    mage = AttackManager.am.RedLaserActive();
                    mage.init(firePoint.position, 2);
                    break;
                case "MageTowerLv_3":
                    mage = AttackManager.am.BlackLaserActive();
                    mage.init(firePoint.position, 3);
                    break;
                case "MageTower_A":
                    mage = AttackManager.am.IceCloudActive();
                    mage.init(firePoint.position, 4);
                    break;
                case "MageTower_B":
                    mage = AttackManager.am.poisonExplodeActive();
                    mage.init(firePoint.position, 5);
                    break;
                default:
                    return;
            }
            SoundManager.sm.MagicWhooshAudio();
            fireCountdown = 1f / fireRate;
            isAttack = false;
        }
        fireCountdown -= Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
