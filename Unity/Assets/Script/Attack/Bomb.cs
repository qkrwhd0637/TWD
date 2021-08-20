using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Effect
{
    public GameObject bombEffect_1;
    public GameObject bombEffect_2;
    public GameObject bombEffect_3;
    public GameObject bombEffect_4;
}

public class Bomb : MonoBehaviour
{
    [Header("Total Bomb Effect")]
    public Effect totalEffect;

    [Header("Attributes")]
    public float firingAngle;
    public float gravity;
    public float elapse_time;

    private Transform target;
    private float flightDuration = 0;
    private float Vx = 0;
    private float Vy = 0;
    

    GameObject effect;

    //목표물을 찾았다면 오브젝트의 속성을 초기 설정한다.
    public void Seek(Transform _target, Vector3 _position, string tag)
    {
        EffectSel(tag);

        this.target = _target;

        //Move projectile to the position of throwing object + add some offset if needed.
        transform.position = _position + new Vector3(0, 0.0f, 0);

        //Calculate distance to target
        float target_Distance = Vector3.Distance(transform.position, target.position);

        //Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        //Extract the X  Y componenent of the velocity
        Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        //Calculate flight time.
        flightDuration = target_Distance / Vx;

        //Rotate projectile to face the target.
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);

        elapse_time = 0;

        //생성 즉시 오디오 재생
        SoundManager.sm.BombAudio();
    }

    //태그를 찾아서 적용 하고자 임펙트를 하는 선택한다.
    void EffectSel(string name)
    {
        switch (name)
        {
            case "CanonTowerLv_1":
                effect = totalEffect.bombEffect_1;
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
            case "CanonTowerLv_2":
                effect = totalEffect.bombEffect_1;
                transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
                break;
            case "CanonTowerLv_3":
                effect = totalEffect.bombEffect_2;
                transform.localScale = new Vector3(2,2,2);
                break;
            case "CanonTower_A":
                effect = totalEffect.bombEffect_3;
                transform.localScale = new Vector3(3f, 3f, 3f);
                break;
            case "CanonTower_B":
                effect = totalEffect.bombEffect_4;
                transform.localScale = new Vector3(2.6f, 2.6f, 2.6f);
                break;
            default:
                effect = totalEffect.bombEffect_1;
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
        }
    }

    void Update()
    {
        //포물선 공식을 이용하여 최초 발견된 목표물 위치를 향해 접근한다.
        transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime * 2, Vx * Time.deltaTime * 2);

        elapse_time += Time.deltaTime * 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("Terrain")) return;

        //미리 선택해둔 선택된 임펙트를 인스터스화 및 최초 위치를 설정한다.
        GameObject obj = Instantiate(effect);
        obj.transform.position = transform.position;

        //해당 오브젝트의 위치를 초기화 및 미사용으로 설정한다.
        firingAngle = 55f;
        gravity = 12f;
        elapse_time = 0f;
        flightDuration = 0;
        Vx = 0;
        Vy = 0;

        transform.position = Vector3.zero;
        AttackManager.am.BombDisable(gameObject);
    }
}
