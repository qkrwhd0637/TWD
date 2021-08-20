using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Tooltip("Object Speed")]
    [Range(0, 100f)]
    public float speed;

    int damage;
    Transform target;

    //목표물을 찾았다면 오브젝트의 속성을 초기 설정한다.
    public void Seek(Transform _target, Vector3 _position, int _damage)
    {
        this.target = _target;
        transform.position = _position;
        this.damage = _damage;

        //생성 즉시 오디오 재생
        SoundManager.sm.ArrowAudio();
    }

    void Update()
    {
        //목표물이 존재하지 않는다면 제거한다.
        if (target == null || !target.gameObject.activeSelf)
        {
            transform.position = Vector3.zero;
            AttackManager.am.ArrowDisable(gameObject);
            return;
        }

        //목표물을 향해 접근한다.
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        //이미지 앞뒤가 반전되어 있음
        transform.up = -dir.normalized;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //에너미 충돌 시 에너미 피격 함수 호출
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null) enemy.HitEnemy(damage);
        }

        //해당 오브젝트의 위치를 초기화 및 미사용으로 설정한다.
        transform.position = Vector3.zero;
        AttackManager.am.ArrowDisable(gameObject);
    }
}
