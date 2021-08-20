using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;
using System.Data;
using DBRun;
using System;
using System.Threading.Tasks;

public class Main : MonoBehaviour
{
    #region 싱글톤화
    public static Main main;
    private void Awake()
    {
        //싱글톤화
        if (main == null) main = this;

        LoginPanel.SetActive(true);
        CreatePanel.SetActive(false);
        loginProc = new LoginProc();
        userInfo = new UserInfoModel();
    }
    #endregion

    public GameObject LoginGroup;

    [Header("LoginPanel")]
    public InputField IDField;
    public InputField PasswordField;
    public GameObject LoginPanel;

    [Header("CreatePanel")]
    public InputField NewIDField;
    public InputField NewPasswordField;
    public GameObject CreatePanel;

    [NonSerialized]
    public string nextScene;

    public GameObject notice;
    public Text noticeText;
    LoginProc loginProc;

    //DB Info
    protected static string _connStr = "Server=localhost;Port=3306;Database=twdef;Uid=root;Password=0637;CharSet=utf8;Allow Zero Datetime = True;";

    public class UserInfoModel : CollectionBase
    {
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private int topPlayRound;
        public int TopPlayRound
        {
            get { return topPlayRound; }
            set { topPlayRound = value; }
        }

        private int lastPlayRound;
        public int LastPlayRound
        {
            get { return lastPlayRound; }
            set { lastPlayRound = value; }
        }

        private DateTime accessDate;
        public DateTime AccessDate
        {
            get { return accessDate; }
            set { accessDate = value; }
        }
    }

    public UserInfoModel userInfo;

    void Start()
    {
        Time.timeScale = 1;
    }

    //계정 로그인하기 버튼
    public void LoginButton()
    {
        SoundManager.sm.ClickAudio();
        if (string.IsNullOrEmpty(IDField.text) || string.IsNullOrEmpty(PasswordField.text))
        {
            //로그인 실패
            notice.SetActive(true);
            noticeText.text = "로그인에 실패했습니다." + "\n" + "공백이 들어갔는지 확인하십시오";
            return;
        }
        Login(IDField.text, PasswordField.text);

    }

    //계정 생성하기 버튼
    public void CreateButton()
    {
        SoundManager.sm.ClickAudio();
        LoginPanel.SetActive(false);
        CreatePanel.SetActive(true);
        IDField.text = null;
        PasswordField.text = null;
    }

    //계정 추가하기 버튼
    public void AddButtonAsync()
    {
        SoundManager.sm.ClickAudio();
        if (string.IsNullOrEmpty(NewIDField.text) || string.IsNullOrEmpty(NewPasswordField.text))
        {
            //로그인 실패
            notice.SetActive(true);
            noticeText.text = "계정 생성에 실패하였습니다." + "\n" + "공백이 들어갔는지 확인하십시오";
            return;
        }


    #region .exe Code(Local DB Use) / 윈도우 실행 파일로 전환 시, 주석 해제
        Task<int> task = Task.Factory.StartNew<int>(() => loginProc.UserIns(NewIDField.text, NewPasswordField.text));
        int errorCheck = task.Result;
        if (errorCheck >= 1)
        {
            //로그인 실패
            notice.SetActive(true);
            noticeText.text = "계정 생성에 실패하였습니다.";
            return;
        }
    #endregion

    #region  .APK Code / 안드로이드 실행 파일로 전환 시, 주석 해제
        //#if DEBUG
        //        Task<int> task = Task.Factory.StartNew<int>(() => loginProc.UserIns(NewIDField.text, NewPasswordField.text));
        //        int errorCheck = task.Result;
        //        if (errorCheck >= 1)
        //        {
        //            //로그인 실패
        //            notice.SetActive(true);
        //            noticeText.text = "계정 생성에 실패하였습니다.";
        //            return;
        //        }
        //#endif
    #endregion

        LoginPanel.SetActive(true);
        CreatePanel.SetActive(false);
        NewIDField.text = null;
        NewPasswordField.text = null;

        notice.SetActive(true);
        noticeText.text = "계정 생성에 성공하였습니다." + "\n" + "생성한 계정으로 로그인 하십시오";
    }

    public void GoBackButton()
    {
        SoundManager.sm.ClickAudio();
        LoginPanel.SetActive(true);
        CreatePanel.SetActive(false);
        NewIDField.text = null;
        NewPasswordField.text = null;
    }

    void OnLevelWasLoaded(int level)
    {
        LoginGroup.SetActive(true);
        Login(PlayerManager.pm.id, PlayerManager.pm.password);
        notice.SetActive(false);
    }


    public void Login(string id, string password)
    {

    #region .exe Code(Local DB Use) / 윈도우 실행 파일로 전환 시, 주석 해제
        Task<DataTable> task = Task.Factory.StartNew<DataTable>(() => loginProc.UserSel(id, password));
        DataTable dt = task.Result;
        if (dt.Rows.Count <= 0)
        {
            //로그인 실패
            notice.SetActive(true);
            noticeText.text = "로그인에 실패했습니다." + "\n" + "해당 아이디는 존재하지 않습니다.";
            return;
        }

        //로그인 완료
        userInfo.Id = Convert.ToString(dt.Rows[0][0]);
        userInfo.TopPlayRound = dt.Rows[0][1] == null ? 1 :
                                Convert.ToString(dt.Rows[0][1]) == "" ? 1 : Convert.ToInt32(dt.Rows[0][1]);
        userInfo.LastPlayRound = dt.Rows[0][2] == null ? 2 :
                                Convert.ToString(dt.Rows[0][2]) == "" ? 1 : Convert.ToInt32(dt.Rows[0][2]);

        userInfo.AccessDate = Convert.ToDateTime(dt.Rows[0][3]);
    #endregion

    #region .APK Code / 안드로이드 실행 파일로 전환 시, 주석 해제
        //#if DEBUGE
        //        Task<DataTable> task = Task.Factory.StartNew<DataTable>(() => loginProc.UserSel(id, password));
        //        DataTable dt = task.Result;
        //        if (dt.Rows.Count <= 0)
        //        {
        //            //로그인 실패
        //            notice.SetActive(true);
        //            noticeText.text = "로그인에 실패했습니다." + "\n" + "해당 아이디는 존재하지 않습니다.";
        //            Debug.Log("로그인 실패");
        //            return;
        //        }

        //        //로그인 완료
        //        userInfo.Id = Convert.ToString(dt.Rows[0][0]);
        //        userInfo.TopPlayRound = dt.Rows[0][1] == null ? 1 :
        //                                Convert.ToString(dt.Rows[0][1]) == "" ? 1 : Convert.ToInt32(dt.Rows[0][1]);
        //        userInfo.LastPlayRound = dt.Rows[0][2] == null ? 2 :
        //                                Convert.ToString(dt.Rows[0][2]) == "" ? 1 : Convert.ToInt32(dt.Rows[0][2]);

        //        userInfo.AccessDate = Convert.ToDateTime(dt.Rows[0][3]);
        //#else
        //        userInfo.Password = password;
        //        userInfo.Id = "qwer";
        //        userInfo.Password = "qwer";
        //        userInfo.TopPlayRound = 7;
        //        userInfo.LastPlayRound = 3;
        //        Debug.Log("Login" + userInfo.Id);
        //#endif
        #endregion

        LoginGroup.SetActive(false);
        ChoiceRound.cr.selectRoundGroup.SetActive(true);
        IDField.text = null;
        PasswordField.text = null;

        GameObject.Find("Round_1").GetComponent<RoundButton>().ButtonEnable();
        GameObject.Find("Round_2").GetComponent<RoundButton>().ButtonEnable();
        GameObject.Find("Round_3").GetComponent<RoundButton>().ButtonEnable();
        GameObject.Find("Round_4").GetComponent<RoundButton>().ButtonEnable();
        GameObject.Find("Round_5").GetComponent<RoundButton>().ButtonEnable();
        GameObject.Find("Round_6").GetComponent<RoundButton>().ButtonEnable();
        GameObject.Find("Round_7").GetComponent<RoundButton>().ButtonEnable();
        GameObject.Find("PlayerRound").GetComponent<RoundButton>().ButtonEnable();
    }

    public void Logout()
    {
        userInfo.Clear();
        Main.main.LoginGroup.SetActive(true);
        Main.main.LoginPanel.SetActive(true);
        Main.main.CreatePanel.SetActive(false);
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
