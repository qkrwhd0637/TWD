using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ChoiceRound : MonoBehaviour
{
    #region 싱글톤화
    public static ChoiceRound cr;
    private void Awake()
    {
        if (cr == null) cr = this;
    }
    #endregion

    [Header("UI Group")]
    public GameObject selectRoundGroup;
    public ToggleGroup toggleGroup;

    [SerializeField]
    public int selectedRound;

    //디펜스 씬 버튼
    public void TowerSceneButton()
    {
        SoundManager.sm.ClickAudio();
        Toggle SelectedToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        string name = SelectedToggle.transform.parent.name;

        //라운드를 선택한다.
        int index = name == "Round_1" ? 0 :
                    name == "Round_2" ? 1 :
                    name == "Round_3" ? 2 :
                    name == "Round_4" ? 3 :
                    name == "Round_5" ? 4 :
                    name == "Round_6" ? 5 :
                    name == "Round_7" ? 6 :
                    name == "PlayerRound" ? Main.main.userInfo.LastPlayRound - 1 : 0;

        selectedRound = index;

        Main.main.LoginPanel.SetActive(false);
        Main.main.CreatePanel.SetActive(false);
        selectRoundGroup.SetActive(false);

        //로딩 씬으로
        Main.main.nextScene = "TowerDef";
        SceneManager.LoadSceneAsync("LodingScene");
    }

    //로그아웃 버튼
    public void LogoutButton()
    {
        SoundManager.sm.ClickAudio();
        Main.main.Logout();
        selectRoundGroup.SetActive(false);
    }
}
