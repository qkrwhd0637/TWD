using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    #region 싱글톤화
    public static SettingManager sm;
    private void Awake()
    {
        //싱글톤화
        if (sm == null) sm = this;
    }
    #endregion
    
    public GameObject setting;
    //public GameObject Setting
    //{
    //    get { return setting; }
    //    set { setting = value; SoundManager.sm.MenuAudio();  if (setting.activeSelf) Pause(); else Resume();}
    //}

    [Header("Setting")]
    public Dropdown dropdown;
    public GameObject defaultSpeed;
    public GameObject halfSpeed;
    public GameObject doubleSpeed;

    [Header("Total Camera")]
    public Camera firstCamera;
    public Camera secondCamera;
    public Camera thirdCamera;

    [System.NonSerialized]
    public int CameraIndex;

    void Start()
    {   
        //설정을 초기화 한다.
        setting.SetActive(false);
        firstCamera.enabled = true;
        secondCamera.enabled = false;
        thirdCamera.enabled = false;
        defaultSpeed.SetActive(true);
        halfSpeed.SetActive(false);
        doubleSpeed.SetActive(false);

        CameraIndex = 1;
    }

    //설정 종료 버튼 클릭
    public void CloseButton()
    {
        setting.SetActive(false);
        SoundManager.sm.MenuAudio();
        Resume();

        defaultSpeed.SetActive(true);
        halfSpeed.SetActive(false);
        doubleSpeed.SetActive(false);
    }

    //취소 버튼 클릭
    public void OpenSetting()
    {
        setting.SetActive(true);
        SoundManager.sm.MenuAudio();
        Pause();
    }

    //게임 멈추기
    public void Pause()
    {
        Time.timeScale = 0;
    }

    //게임 재생
    public void Resume()
    {
        Time.timeScale = 1;
    }

    //홈 화면 버튼 클릭
    public void MainSceneButton()
    {
        setting.SetActive(false);
        Main.main.nextScene = "Main";
        SceneManager.LoadSceneAsync("LodingScene");
    }

    //카메라 시점 1
    public void FirstViewButton()
    {
        CameraIndex = 1;
        firstCamera.enabled = true;
        secondCamera.enabled = false;
        thirdCamera.enabled = false;
    }

    //카메라 시점 2
    public void SecondViewButton()
    {
        CameraIndex = 2;
        firstCamera.enabled = false;
        secondCamera.enabled = true;
        thirdCamera.enabled = false;
    }

    //카메라 시점 3
    public void ThirdViewButton()
    {
        CameraIndex = 3;
        firstCamera.enabled = false;
        secondCamera.enabled = false;
        thirdCamera.enabled = true;
    }

    public void SelectButton()
    {        
        switch (dropdown.value)
        {
            case 0 :
                FirstViewButton();
                break;
            case 1:
                SecondViewButton();
                break;
            case 2:
                ThirdViewButton();
                break;
        }
        //업데이트 UI 열린 상태라면 위치 조정
        UpgradeManager.um.LocationChange();
    }

    public void GameSpeed()
    {
        switch (Time.timeScale)
        {
            case 1f:
                defaultSpeed.SetActive(false);
                halfSpeed.SetActive(true);
                doubleSpeed.SetActive(false);
                Time.timeScale = 1.5f;
                break;
            case 1.5f:
                defaultSpeed.SetActive(false);
                halfSpeed.SetActive(false);
                doubleSpeed.SetActive(true);
                Time.timeScale = 2f;
                break;
            case 2f:
                defaultSpeed.SetActive(true);
                halfSpeed.SetActive(false);
                doubleSpeed.SetActive(false);
                Time.timeScale = 1f;
                break;
        }
    }
}
