using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : MonoBehaviour
{
    //임펙틀 종류
    enum MageKinds
    {
        None = 0,
        Explosion_1 = 16,
        Explosion_2 = 17,
        Explosion_3,
        Explosion_4,
    }

    [Header("Attack Attribute")]
    [Range(0, 4f)]
    public float destroyTime;
    public int damage;

    float currentTime = 0;
    float TimeLeft = 0.05f;
    float nextTime = 0.0f;
    MageKinds _mageKinds;

    void Start()
    {
        //생성과 즉시 오디오를 재생한다.
        SoundManager.sm.ExplosionAudio();
        //해당 오브젝트의 레이아웃으로 임펙트르 선택한다.
        _mageKinds = (MageKinds)gameObject.layer;
    }

    void Update()
    {
        //일정 시간이 지나면 자연스럽게 파괴한다.
        if (currentTime > destroyTime)
        {
            Destroy(gameObject);
        }
        currentTime += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (nextTime > 1.1f) return;

            //에너미 충돌 시 에너미 피격 함수 호출
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy == null) return;

            enemy.HitEnemy(damage);

            //임펙트 혹은 공격에 따라 에너미 상태를 설정한다.
            if (_mageKinds != MageKinds.Explosion_3) return;
            enemy.Burn();
        }
    }

    //특정 임펙트 혹은 공격에만 해당되는 함수
    private void OnTriggerStay(Collider other)
    {
        if (_mageKinds != MageKinds.Explosion_4) return;
        
        //지정 시간까지 계속 재생한다.
        if (Time.time > nextTime)
        {
            nextTime = Time.time + TimeLeft;
            if (other.tag != "Enemy") return;
            //if (other.CompareTag("Enemy")) return;

            //에너미 충돌 시 에너미 피격 함수 호출
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy == null) return;

            enemy.HitEnemy(10);
        }
    }
}
