using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Price
{
    public int archerTowerLv_1;
    public int archerTowerLv_2;
    public int archerTowerLv_3;
    public int archerTower_A;
    public int archerTower_B;
    public int canonTowerLv_1;
    public int canonTowerLv_2;
    public int canonTowerLv_3;
    public int canonTower_A;
    public int canonTower_B;
    public int mageTowerLv_1;
    public int mageTowerLv_2;
    public int mageTowerLv_3;
    public int mageTower_A;
    public int mageTower_B;
}

public class DealManager : MonoBehaviour
{
    #region 싱글톤화
    public static DealManager dm;
    private void Awake()
    {
        //싱글톤화
        if (dm == null) dm = this;
    }
    #endregion

    [Header("Notice Text")]
    public GameObject noticeText;

    [Header("Tower Price")]
    public Price totalTowerPrice;

    Dictionary<string, int> dictionary = new Dictionary<string, int>();
    readonly WaitForSeconds wait = new WaitForSeconds(1.5f);

    void Start()
    {
        //설정창을 종료 상태 설정
        noticeText.SetActive(false);

        //타워 정보를 딕셔너리에 담아두어 관리 할 예정
        dictionary.Add("ArcherTowerLv_1", totalTowerPrice.archerTowerLv_1);
        dictionary.Add("ArcherTowerLv_2", totalTowerPrice.archerTowerLv_2);
        dictionary.Add("ArcherTowerLv_3", totalTowerPrice.archerTowerLv_3);
        dictionary.Add("ArcherTower_A", totalTowerPrice.archerTower_A);
        dictionary.Add("ArcherTower_B", totalTowerPrice.archerTower_B);
        dictionary.Add("CanonTowerLv_1", totalTowerPrice.canonTowerLv_1);
        dictionary.Add("CanonTowerLv_2", totalTowerPrice.canonTowerLv_2);
        dictionary.Add("CanonTowerLv_3", totalTowerPrice.canonTowerLv_3);
        dictionary.Add("CanonTower_A", totalTowerPrice.canonTower_A);
        dictionary.Add("CanonTower_B", totalTowerPrice.canonTower_B);
        dictionary.Add("MageTowerLv_1", totalTowerPrice.mageTowerLv_1);
        dictionary.Add("MageTowerLv_2", totalTowerPrice.mageTowerLv_2);
        dictionary.Add("MageTowerLv_3", totalTowerPrice.mageTowerLv_3);
        dictionary.Add("MageTower_A", totalTowerPrice.mageTower_A);
        dictionary.Add("MageTower_B", totalTowerPrice.mageTower_B);
    }
   
    //타워 구매 시도
    public bool TryToBuy(string name, Vector3 _position)
    {
        int price = PriceSel(name);
        if (price <= 0) return false;

        bool isPossible;
        if (PlayerManager.pm.money >= price)
        {
            PlayerManager.pm.money -= price;
            StartCoroutine(UseMoneyNotice("-" + price.ToString() + "$", _position));
            isPossible = true;
        }
        else
        {
            StartCoroutine(UseMoneyNotice("금액이 부족합니다.", _position));
            isPossible = false;
        }
        return isPossible;
    }

    //해당 타워를 판매 시도
    public void Refund(string name, Vector3 _position)
    {
        int price = PriceSel(name);
        int refundPrice = (int)(price * 0.5);
        PlayerManager.pm.money += refundPrice;
        StartCoroutine(UseMoneyNotice("+" + refundPrice.ToString() + "$", _position));
    }

    //딕셔너리에서 해당 타워의 정보를 불러온다.
    public int PriceSel(string name)
    {
        int price = (dictionary.ContainsKey(name).Equals(true)) == true ? dictionary[name] : 0;
        return price;
    }

    public void EastEgg(Vector3 _position)
    {
        PlayerManager.pm.money += 100;
        StartCoroutine(UseMoneyNotice("+100$", _position));
    }

    //해당 타워에 가격 정보를 일정 시간동안 텍스트로 띄어준다.
    IEnumerator UseMoneyNotice(string text, Vector3 _position)
    {
        noticeText.SetActive(true);
        noticeText.GetComponent<Text>().text = text;
        noticeText.transform.position = Camera.main.WorldToScreenPoint(_position + new Vector3(2, 10, 0));
        yield return wait;
        noticeText.SetActive(false);
    }
}
