using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{   
    [Range(0, 2f)]
    public float destroyTime;

    float currentTime = 0;
    public int damage;

    int mageLevel;

    void Update()
    {
        //일정 시간이 지나면 자연스럽게 미사용 상태로 전환한다.
        if (currentTime > destroyTime)
        {
            //해당 오브젝트의 레이아웃으로 현재 마법 공격을 선택한다.

            currentTime = 0;
            switch (mageLevel)
            {
                case 1:
                    AttackManager.am.BlueLaserDisable(gameObject);
                    break;
                case 2:
                    AttackManager.am.RedLaserDisable(gameObject);
                    break;
                case 3:
                    AttackManager.am.BlackLaserDisable(gameObject);
                    break;
                case 4:
                    AttackManager.am.IceCloudDisable(gameObject);
                    break;
                case 5:
                    AttackManager.am.PoisonExplodeDisable(gameObject);
                    break;
                default:
                    return;
            }
            #region
            #endregion
        }
        currentTime += Time.deltaTime;
    }

    //초기값 셋팅
    public void init(Vector3 _position, int _mageLevel)
    {
        transform.position = _position;
        this.mageLevel = _mageLevel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //에너미 충돌 시 에너미 피격 함수 호출
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy == null) return;

            enemy.HitEnemy(damage);

            switch (mageLevel)
            {
                case 4:
                    if (currentTime > 0.8f) return;
                    enemy._EnemyState = Enemy.EnemyState.Freeze;
                    break;
                case 5:
                    if (currentTime > 0.75f) return;
                    enemy.Slowdown();
                    break;
            }
        }
    }
}
