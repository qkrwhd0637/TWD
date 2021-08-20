using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    #region 싱글톤화
    public static PlayerManager pm;
    private void Awake()
    {
        //싱글톤화
        if (pm == null) pm = this;
    }
    #endregion

    public int money;

    [HideInInspector]
    public string id;
    [HideInInspector]
    public string password;

    [Header("UI (Text)")]
    public Text roundText;
    public Text waveText;
    public Text goldText;
    public Text goldNoticeText;

    Vector3 dir;
    readonly WaitForSeconds wait = new WaitForSeconds(0.7f);

    void Start()
    {
        SetUserInfo();
        
        //현재 게임 현황 초기화
        waveText.text = EnemyManager.em.waveIndex.ToString();
        goldText.text = money.ToString();
        goldNoticeText.enabled = false;
    }

    void Update()
    {
        //현재 게임 상태 설정
        roundText.text = StageManager.sm.StageIndex.ToString();
        waveText.text = EnemyManager.em.waveIndex.ToString();
        goldText.text = money.ToString();
        if (goldNoticeText.enabled)
        {
            goldNoticeText.transform.Translate(dir * Time.deltaTime);
        }
    }

    //유저 정보 설정
    void SetUserInfo()
    {
#if DEBUG
        PlayerManager.pm.id = Main.main == null ? "qwer" : Main.main.userInfo.Id;
        PlayerManager.pm.password = Main.main == null ? "qwer" : Main.main.userInfo.Password;
        //PlayerManager.pm.id = "qwer";
        //PlayerManager.pm.password = "qwer";
#else
        PlayerManager.pm.id = Main.main.userInfo.Id;
        PlayerManager.pm.password = Main.main.userInfo.Password;
#endif
    }

    void ResetAnim()
    {
        goldNoticeText.transform.position = Vector3.zero;
        dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f) - 1).normalized;
    }

    //골드 획득
    public void GoldAcheive(int gold)
    {
        money += gold;
    }

    IEnumerator SetSpeed()
    {
        yield return wait;
        goldNoticeText.enabled = false;
        ResetAnim();
    }

    //설정 버튼 클릭 시
    public void SettingButton()
    {
        SettingManager.sm.OpenSetting();
    }
}