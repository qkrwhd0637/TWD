using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class TotalTowerCons
{
    public GameObject archerLv1;
    public GameObject archerLv2;
    public GameObject archerLv3;
    public GameObject archerA;
    public GameObject archerB;
    public GameObject canonLv1;
    public GameObject canonLv2;
    public GameObject canonLv3;
    public GameObject canonA;
    public GameObject canonB;
    public GameObject mageLv1;
    public GameObject mageLv2;
    public GameObject mageLv3;
    public GameObject mageA;
    public GameObject mageB;
}

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager um;
    void Awake()
    {
        if (um == null) um = this;
    }

    [Header("UI Group")]
    public GameObject buildingUIGroup;
    public GameObject defaultUIGroup;
    public GameObject maximumUIGroup;
    public GameObject refundGroup;

    [Header("building Point Obj")]
    public GameObject buildingPoint;
    public TotalTowerCons towerCons;

    GameObject target;
    GameObject tempTower;

    void Start()
    {
        //UI GROUP 초기 설정
        buildingUIGroup.SetActive(false);
        defaultUIGroup.SetActive(false);
        maximumUIGroup.SetActive(false);
        refundGroup.SetActive(false);
    }

    void Update()
    {
        //마우스 왼쪽 클릭 시
        if (Input.GetMouseButtonDown(0))
        {
            //설정창 혹은 알림창이 뜬 상태에서는 활성화 하지 않는다.
            if (SettingManager.sm.setting.activeSelf == true) return;
            if (NoticeManager.nm.Win.activeSelf == true || NoticeManager.nm.Lose.activeSelf == true) return;
            if (buildingUIGroup.activeSelf || defaultUIGroup.activeSelf || maximumUIGroup.activeSelf || refundGroup.activeSelf) return;

            //해당 충돌한 오브젝트를 가지고 온다.
            target = GetClickedObject();
            if (target is null) return;
            //오브젝트 선택
            UpgradeUIActive(target);
        }
    }

    private void UpgradeUIActive(GameObject _target)
    {
        switch (_target.tag)
        {
            case "BuildingPoint":
                buildingUIGroup.SetActive(true);
                buildingUIGroup.transform.position = Camera.main.WorldToScreenPoint(_target.transform.position);
                defaultUIGroup.SetActive(false);
                maximumUIGroup.SetActive(false);
                refundGroup.SetActive(false);
                break;
            case "ArcherTowerLv_1":
            case "ArcherTowerLv_2":
            case "CanonTowerLv_1":
            case "CanonTowerLv_2":
            case "MageTowerLv_1":
            case "MageTowerLv_2":
                defaultUIGroup.SetActive(true);
                defaultUIGroup.transform.position = Camera.main.WorldToScreenPoint(_target.transform.position);
                buildingUIGroup.SetActive(false);
                maximumUIGroup.SetActive(false);
                refundGroup.SetActive(false);
                break;
            case "ArcherTowerLv_3":
            case "CanonTowerLv_3":
            case "MageTowerLv_3":
                maximumUIGroup.SetActive(true);
                maximumUIGroup.transform.position = Camera.main.WorldToScreenPoint(_target.transform.position);
                buildingUIGroup.SetActive(false);
                defaultUIGroup.SetActive(false);
                refundGroup.SetActive(false);
                break;
            case "ArcherTower_A":
            case "ArcherTower_B":
            case "CanonTower_A":
            case "CanonTower_B":
            case "MageTower_A":
            case "MageTower_B":
                refundGroup.SetActive(true);
                refundGroup.transform.position = Camera.main.WorldToScreenPoint(_target.transform.position);
                buildingUIGroup.SetActive(false);
                defaultUIGroup.SetActive(false);
                maximumUIGroup.SetActive(false);
                break;
            case "EastEgg":
                DealManager.dm.EastEgg(target.transform.position);
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    buildingUIGroup.SetActive(false);
                    defaultUIGroup.SetActive(false);
                    maximumUIGroup.SetActive(false);
                    refundGroup.SetActive(false);
                }
                return;
            default:
                //if (!EventSystem.current.IsPointerOverGameObject())
                //{
                //    buildingUIGroup.SetActive(false);
                //    defaultUIGroup.SetActive(false);
                //    maximumUIGroup.SetActive(false);
                //    refundGroup.SetActive(false);
                //}
                return;
        }
        tempTower = _target;
    }

    //오브젝트 가져오기
    private GameObject GetClickedObject()
    {
        RaycastHit hit;
        GameObject target = null;
        Camera.main.ScreenPointToRay(Input.mousePosition);
        var temp = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 포인트 근처 좌표를 만든다. 
        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))   //마우스 근처에 오브젝트가 있는지 확인
        {
            //있으면 오브젝트를 저장한다.
            target = hit.collider.gameObject;
        }
        return target;
    }

    //궁수타워 생성 버튼
    public void ArcherButton()
    {
        //기존에 선택된 오브젝트가 없다면 아래 코드 무시
        if (tempTower is null)
            return;

        //딜 매니저에서 해당 타워 구매 가능한지? Bool 값을 반환한다.
        if (!DealManager.dm.TryToBuy("ArcherTowerLv_1", tempTower.transform.position)) return;

        //구매 타워를 짓는다.
        addTowerCons(towerCons.archerLv1);
        
        //기존 타워는 제거한다.
        DestroyTower();
    }

    //캐논타워 생성 버튼
    public void CanonButton()
    {
        if (tempTower is null)
            return;

        if (!DealManager.dm.TryToBuy("CanonTowerLv_1", tempTower.transform.position)) return;

        addTowerCons(towerCons.canonLv1);
        DestroyTower();
    }

    //마법타워 생성 버튼
    public void MageButton()
    {
        if (tempTower is null)
            return;

        if (!DealManager.dm.TryToBuy("MageTowerLv_1", tempTower.transform.position)) return;

        addTowerCons(towerCons.mageLv1);
        DestroyTower();
    }

    //타워 종류에 맞는 타워를 업그레이드 한다. (LV_1 이상 LV_2 이하 타워만 해당하는 버튼)
    public void TowerUpgradeButton()
    {
        if (tempTower is null)
            return;

        switch (tempTower.tag)
        {
            case "ArcherTowerLv_1":
                if (!DealManager.dm.TryToBuy("ArcherTowerLv_2", tempTower.transform.position)) return;
                addTowerCons(towerCons.archerLv2);
                break;
            case "ArcherTowerLv_2":
                if (!DealManager.dm.TryToBuy("ArcherTowerLv_3", tempTower.transform.position)) return;
                addTowerCons(towerCons.archerLv3);
                break;
            case "CanonTowerLv_1":
                if (!DealManager.dm.TryToBuy("CanonTowerLv_2", tempTower.transform.position)) return;
                addTowerCons(towerCons.canonLv2);
                break;
            case "CanonTowerLv_2":
                if (!DealManager.dm.TryToBuy("CanonTowerLv_3", tempTower.transform.position)) return;
                addTowerCons(towerCons.canonLv3);
                break;
            case "MageTowerLv_1":
                if (!DealManager.dm.TryToBuy("MageTowerLv_2", tempTower.transform.position)) return;
                addTowerCons(towerCons.mageLv2);
                break;
            case "MageTowerLv_2":
                if (!DealManager.dm.TryToBuy("MageTowerLv_3", tempTower.transform.position)) return;
                addTowerCons(towerCons.mageLv3);
                break;
            default:
                return;
        }
        DestroyTower();
    }

    //타워 종류에 따른 궁극 타워 A (LV_3 이상 타워만 해당하는 타워)
    public void TowerAUpgradeButton()
    {
        if (tempTower is null)
            return;

        switch (tempTower.tag)
        {
            case "ArcherTowerLv_3":
                if (!DealManager.dm.TryToBuy("ArcherTower_A", tempTower.transform.position)) return;
                addTowerCons(towerCons.archerA);
                break;
            case "CanonTowerLv_3":
                if (!DealManager.dm.TryToBuy("CanonTower_A", tempTower.transform.position)) return;
                addTowerCons(towerCons.canonA);
                break;
            case "MageTowerLv_3":
                if (!DealManager.dm.TryToBuy("MageTower_A", tempTower.transform.position)) return;
                addTowerCons(towerCons.mageA);
                break;
            default:
                return;
        }
        DestroyTower();
    }

    //타워 종류에 따른 궁극 타워 B (LV_3 이상 타워만 해당하는 타워)
    public void TowerBUpgradeButton()
    {
        if (tempTower is null)
            return;

        switch (tempTower.tag)
        {
            case "ArcherTowerLv_3":
                if (!DealManager.dm.TryToBuy("ArcherTower_B", tempTower.transform.position)) return;
                addTowerCons(towerCons.archerB);
                break;
            case "CanonTowerLv_3":
                if (!DealManager.dm.TryToBuy("CanonTower_B", tempTower.transform.position)) return;
                addTowerCons(towerCons.canonB);
                break;
            case "MageTowerLv_3":
                if (!DealManager.dm.TryToBuy("MageTower_B", tempTower.transform.position)) return;
                addTowerCons(towerCons.mageB);
                break;
            default:
                return;
        }
        DestroyTower();
    }

    //판매 버튼 클릭
    public void RefundButton()
    {
        if (tempTower is null)
            return;

        DealManager.dm.Refund(tempTower.tag, tempTower.transform.position);

        GameObject obj = Instantiate(buildingPoint, GameObject.Find("Tower").transform);
        obj.transform.position = tempTower.transform.position;

        DestroyTower();
    }

    //업그레이드 타워 생성하기 전에 빌드 타워부터 생성한다.
    public void addTowerCons(GameObject towerCons)
    {
        GameObject obj = Instantiate(towerCons, GameObject.Find("Tower").transform);
        TowerBuild towerBuild = obj.GetComponent<TowerBuild>();
        towerBuild.transform.position = tempTower.transform.position;
    }

    private void DestroyTower()
    {
        Destroy(tempTower);
        buildingUIGroup.SetActive(false);
        defaultUIGroup.SetActive(false);
        maximumUIGroup.SetActive(false);
        refundGroup.SetActive(false);
    }

    public void CloseButton()
    {
        buildingUIGroup.SetActive(false);
        defaultUIGroup.SetActive(false);
        maximumUIGroup.SetActive(false);
        refundGroup.SetActive(false);
    }

    public void LocationChange()
    {
        if (tempTower == null) return;
        if (buildingUIGroup.activeSelf)
            buildingUIGroup.transform.position = Camera.main.WorldToScreenPoint(tempTower.transform.position);
        if (defaultUIGroup.activeSelf)
            defaultUIGroup.transform.position = Camera.main.WorldToScreenPoint(tempTower.transform.position);
        if (maximumUIGroup.activeSelf)
            maximumUIGroup.transform.position = Camera.main.WorldToScreenPoint(tempTower.transform.position);
        if (refundGroup.activeSelf)
            refundGroup.transform.position = Camera.main.WorldToScreenPoint(tempTower.transform.position);
    }
}