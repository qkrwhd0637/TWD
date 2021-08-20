using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DBRun;
using System.Threading.Tasks;

public class StageManager : MonoBehaviour
{
    #region 싱글톤화
    public static StageManager sm;
    private void Awake() 
    {
        if (sm == null) sm = this;
    }
    #endregion

    public GameObject NextStageObj;
    public Text startButtonText;

    [System.NonSerialized]
    public int maxStageIndex = 7;

    private int stageIndex = 0;
    public int StageIndex
    {
        get { return stageIndex; }
        set { stageIndex = value; }
    }

    bool isRest;
    int pointCnt = 0;
    PlayProc playProc;

    void Start()
    {
#if DEBUG
        stageIndex = ChoiceRound.cr == null ? 0 : ChoiceRound.cr.selectedRound;
        playProc = new PlayProc();
#else
        stageIndex = ChoiceRound.cr.selectedRound;
#endif
        isRest = true;
        startButtonText.text = "스타트";
        InvokeRepeating("TextProgress", 0.5f, 1f);
    }
    void Update()
    {
        if (isRest) return;

        EnemyManager.em.Wave();
    }

    public void WaveEnd()
    {
        SoundManager.sm.DefaultBGM();
        startButtonText.text = "스타트";
        isRest = true;
    }

    //다음 라운드 버튼 클릭
    public void NextStageButton()
    {
        if (!isRest) return;

        SoundManager.sm.DangerBGM();
        isRest = false;

        #region .exe Code(Local DB Use) / 윈도우 실행 파일로 전환 시, 주석 해제
        Task.Factory.StartNew(() => playProc.PlayUpt(PlayerManager.pm.id, stageIndex));
        #endregion

        stageIndex++;
        EnemyManager.em.InitWave();


    }

    //라운드 시작 버튼에 텍스트 효과 재생
    void TextProgress()
    {
        if (isRest) return;

        if (pointCnt == 0)
            startButtonText.text = "진행중";
        else
            startButtonText.text = startButtonText.text + ".";

        pointCnt = pointCnt == 3 ? 0 : ++pointCnt;
    }
}
