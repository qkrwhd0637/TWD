using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    public Transform Stick;         // 조이스틱.
    
    private Vector3 StickFirstPos;  // 조이스틱의 처음 위치.
    private Vector3 JoyVec;         // 조이스틱의 벡터(방향)
    private float Radius;           // 조이스틱 배경의 반 지름.

    void Start()
    {
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        StickFirstPos = Stick.transform.position;

        // 캔버스 크기에대한 반지름 조절.
        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= Can;
    }

    // 드래그
    public void Drag(BaseEventData _Data)
    {
        PointerEventData Data = _Data as PointerEventData;
        Vector2 Pos = Data.position;
        
        // 조이스틱을 이동시킬 방향을 구함.(오른쪽,왼쪽,위,아래)
        JoyVec = (Pos - (Vector2)StickFirstPos).normalized;

        // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
        float Dis = Vector2.Distance(Pos, StickFirstPos);

        // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는곳으로 이동. 
        if (Dis < Radius)
            Stick.position = StickFirstPos + JoyVec * Dis;
        // 거리가 반지름보다 커지면 조이스틱을 반지름의 크기만큼만 이동.
        else
            Stick.position = StickFirstPos + JoyVec * Radius;

        Vector2 value = Vector2.ClampMagnitude(JoyVec, Radius).normalized;
        Vector3 movePosition;
        switch (SettingManager.sm.CameraIndex)
        {
            case 1 :
                if (Camera.main.transform.position.x < -100 && value.y > 0 || Camera.main.transform.position.x > 100 && value.y < 0) value.y = 0;
                if (Camera.main.transform.position.z < -100 && value.x > 0 || Camera.main.transform.position.z > 100 && value.x < 0) value.x = 0;

                movePosition = new Vector3(value.y * 50f * Time.deltaTime, 0, -value.x * 50f * Time.deltaTime);
                Camera.main.transform.position += movePosition;
                break;
            case 2:
                if (Camera.main.transform.position.x < 0 && value.y > 0 || Camera.main.transform.position.x > 200 && value.y < 0) value.y = 0;
                if (Camera.main.transform.position.z < -200 && value.x < 0 || Camera.main.transform.position.z > 200 && value.x > 0) value.x = 0;

                movePosition = new Vector3(-value.y * 50f * Time.deltaTime, 0, value.x * 50f * Time.deltaTime);
                Camera.main.transform.position += movePosition;
                break;
            case 3:
                if (Camera.main.transform.position.x < -50 && value.y > 0 || Camera.main.transform.position.x > 200 && value.y < 0) value.y = 0;
                if (Camera.main.transform.position.z < -200 && value.x < 0 || Camera.main.transform.position.z > 100 && value.x > 0) value.x = 0;

                movePosition = new Vector3(-value.y * 50f * Time.deltaTime, 0, value.x * 50f * Time.deltaTime);
                Camera.main.transform.position += movePosition;
                break;
        }

        //업데이트 UI 열린 상태라면 위치 조정
        UpgradeManager.um.LocationChange();
    }

    // 드래그 끝.
    public void DragEnd()
    {
        Stick.position = StickFirstPos; // 스틱을 원래의 위치로.
        JoyVec = Vector3.zero;          // 방향을 0으로.
    }
}
