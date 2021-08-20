using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{    
    #region 싱글톤화
    public static LifeManager lm;
    void Awake()
    {
        if (lm == null) lm = this;
    }
    #endregion

    [Range(1, 20)]
    public int lifeMax;

    public GameObject lifePanel;
    public GameObject lifeFactory;
    public Stack<GameObject> totalLife;
    
    void Start()
    {
        //오브젝트 폴링
        InitObjectPooling();
    }

    void InitObjectPooling()
    {
        //일정 거리(Img) 만큼 라이프를 생성한다.
        totalLife = new Stack<GameObject>();
        float x = 0f;
        for (int i = 0; i < lifeMax; i++)
        {
            GameObject life = Instantiate(lifeFactory, lifePanel.transform);
            life.transform.position += new Vector3(x + 100, 0);
            life.SetActive(true);
            totalLife.Push(life);
            x -= 30f;
        }
    }

    //플레이어 라이프 잃음
    public void LifeDisable()
    {
        Destroy(totalLife.Pop());

        //플레이어 배패
        if (totalLife.Count <= 0)
        {
            SoundManager.sm.MenuAudio();
            SettingManager.sm.Pause();
            NoticeManager.nm.Lose.SetActive(true);
        }
    }
}
