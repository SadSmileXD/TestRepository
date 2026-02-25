using System;
using UnityEngine;
using UnityEngine.InputSystem; // 새로운 인풋 시스템을 사용하기 위해 반드시 추가해야 합니다.

public class ClickPos : MonoBehaviour
{
    [SerializeField]private GameManager gameManager;
    public MoveObject movedata;
    Vector2Int data;
    void Update()
    {
        // 마우스가 연결되어 있는지, 그리고 이번 프레임에 우클릭이 눌렸는지 확인합니다.
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            // 1. 화면 픽셀 좌표 가져오기 (Vector2 형태로 들어옵니다)
            Vector2 mousePos = Mouse.current.position.ReadValue();

            // Vector2를 Vector3로 변환 (ScreenToWorldPoint에 넣기 위함)
            Vector3 screenPos = new Vector3( mousePos.x, mousePos.y, 0f);

            // 2. 월드 좌표로 변환
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
              data= Vector2Int.RoundToInt(worldPos);
           
           

            Debug.Log("새로운 인풋 시스템 - 우클릭 위치: " + data);
            gameManager.targetPos = data;
            gameManager.PathFinding();
            movedata.OnMove();
        }
    }
}