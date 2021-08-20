using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoticeManager : MonoBehaviour
{
    #region 싱글톤화
    public static NoticeManager nm;
    private void Awake()
    {
        if (nm == null) nm = this;
    }
    #endregion

    [Header("UI Object")]
    [SerializeField]
    private GameObject win;
    public GameObject Win
    {
        get { return win; }
        set { win = value; }
    }

    [SerializeField]
    private GameObject lose;
    public GameObject Lose
    {
        get { return lose; }
        set { lose = value; }
    }

    void Start()
    {
        //UI 오브젝트 초기화
        win.SetActive(false);
        lose.SetActive(false);
    }

    //동의 버튼 클릭
    public void AgreeButton()
    {
        if (win.activeSelf)
        {
            SettingManager.sm.MainSceneButton();
        }
        else if (lose.activeSelf)
        {
            Main.main.nextScene = "TowerDef";
            SceneManager.LoadSceneAsync("LodingScene");
        }
    }

    //취소 버튼 클릭
    public void CancelButton()
    {
        SettingManager.sm.Resume();

        if (win.activeSelf)
        {
            Main.main.nextScene = "TowerDef";
            SceneManager.LoadSceneAsync("LodingScene");
        }
        else if (lose.activeSelf)
        {
            SettingManager.sm.MainSceneButton();
        }
    }
}
