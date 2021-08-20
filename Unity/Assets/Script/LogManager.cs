using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//개발 과정 중 테스트를 위해서 만든 스크립트
public class LogManager : MonoBehaviour
{
    #region 싱글톤화
    public static LogManager lm;
    private void Awake()
    {
        //싱글톤화
        if (lm == null) lm = this;
    }
    #endregion

    public string GetDateTime()
    {
        System.DateTime NowDate = System.DateTime.Now;
        return NowDate.ToString("yyyy-MM-dd HH:mm:ss") + ":" + NowDate.Millisecond.ToString("000");
    }

    /// 로그내용
    public void Log(String msg)
    {
        string FilePath = Directory.GetCurrentDirectory() + @"\Logs\" + DateTime.Today.ToString("yyyyMMdd") + ".log";
        string DirPath = Directory.GetCurrentDirectory() + @"\Logs";
        string temp;

        DirectoryInfo di = new DirectoryInfo(DirPath);
        FileInfo fi = new FileInfo(FilePath);

        try
        {
            if (di.Exists != true) Directory.CreateDirectory(DirPath);
            if (fi.Exists != true)
            {
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    temp = string.Format("[{0}] {1}", GetDateTime(), msg);
                    sw.WriteLine(temp);
                    sw.Close();
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(FilePath))
                {
                    temp = string.Format("[{0}] {1}", GetDateTime(), msg);

                    sw.WriteLine(temp);

                    sw.Close();
                }
            }
        }
        catch (Exception e)
        {

        }
    }
}
