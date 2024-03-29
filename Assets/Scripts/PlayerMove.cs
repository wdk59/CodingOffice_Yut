using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;

    public bool myTurn = false;         // 내 차례인지 판별하는 변수
    public bool selectingPawn = false;  // 내 차례일 때 움직일 말을 선택하는 단계인지 판별하는 변수
    public bool pawnSelected = false;          // 움직일 말을 선택했는지 판별하는 변수

    int[] movingVector = new int[] { 0, 0 };    // 말이 움직일 방향과 크기를 가짐
    int[] pawnLoc = new int[4] { -1, -1, -1, -1 };  // 말의 현재 위치 (-1: 출발 대기, 0 ~ 28: 자리 번호)

    public int[] waitYut = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // 1: 도, 2: 개, 3: 걸, 4: 윷, 5: 모, 6: 빽도, 7: 낙
    public int waitYutTop = -1;             // waitYut의 마지막 인덱스를 가리킴 (-1이면 아무것도 없음)
    public int remainingYut = 0;            // waitYut에 아무것도 없는지 확인. waitYut의 값이 바뀔 때마다 업데이트.

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    
    // Update is called once per frame
    void Update()
    {
        // 내 차례에 윷을 다 던졌고, 대기열(waitYut)에 남은 이동 횟수가 없을 때까지 움직일 수 있음 (GM의 HidingYutResult 함수에서 selectingPawn이 true로 바뀜)
        if (selectingPawn && !pawnSelected && remainingYut > 0) // 아직 움직일 말 선택 안 함
        {
            //Debug.Log("pUpdate: 움직여!");
            // 타이머 걸고 시간 끝날 때까지 선택 안 하면 랜덤으로 움직이면 될듯
        }
        else if (selectingPawn && pawnSelected && remainingYut > 0) // 움직일 말 선택 함: 이제 목적지 선택하고 움직여야 함
        {
            // 움직일 목적지 선택: 이동 가능한 목적지 표시해주기, 선택하면 waitYut에서 해당 목적지 지워주고 자리 당기기, waitYutTop 업데이트, remainingYut 업데이트
            Debug.Log("갈 수 있는 곳: " + gameManager.clickedObject.transform.GetSiblingIndex());
            // (보류) 일단 하나만 waitYut에 있다고 가정했을 때 움직이는지부터 확인
            // waitYut에 저장돼있는 기록을 모두 탐색하여 목적지 선택할 수 있게 하기

            /* 선택된 말을 목적지까지 이동 */
            //gameManager.clickedObject.transform.position = new Vector2(4f, -4f);
            
            // 말 출발 인덱스 조정
            if (pawnLoc[gameManager.clickedObject.transform.GetSiblingIndex()] == -1)   // 말이 아직 출발하지 않았을 때
            {
                pawnLoc[gameManager.clickedObject.transform.GetSiblingIndex()] = 0;
            }
            else if (waitYut[0] == 6)   // 말이 출발한 상태에서 빽도 선택 (뭔가 잘못됐는뎅)
            {
                waitYut[0] = -1;
            }

            // 낙
            if (waitYut[0] == 7)    
            {
                Debug.Log("낙입니다.");
            }
            else
            {
                pawnLoc[gameManager.clickedObject.transform.GetSiblingIndex()] = GameObject.Find("UI").transform.Find("YutPanel").transform.GetChild(pawnLoc[gameManager.clickedObject.transform.GetSiblingIndex()]).transform.GetComponent<PanelInfo>().nextPanel(pawnLoc[gameManager.clickedObject.transform.GetSiblingIndex()], waitYut[0]);
                gameManager.clickedObject.transform.position = GameObject.Find("UI").transform.Find("YutPanel").transform.GetChild(pawnLoc[gameManager.clickedObject.transform.GetSiblingIndex() - 1]).position;
            }

            // 순서 넘기기
            nextTurn();
        }
    }

    // 순서 넘기기
    public void nextTurn()
    {
        Initialize();   // 현재 내 상태 초기화

        GameObject.Find("UI").transform.Find("Player_Panel_Group").transform.GetChild(gameManager.playerTurn - 1).gameObject.GetComponent<SpriteRenderer>().material.color = Color.white;
        if (gameManager.playerTurn < gameManager.participantTop + 1)
            gameManager.playerTurn++;
        else
            gameManager.playerTurn = 1;
        GameObject.Find("UI").transform.Find("Player_Panel_Group").transform.GetChild(gameManager.playerTurn - 1).gameObject.GetComponent<SpriteRenderer>().material.color = Color.gray;
        gameManager.turnChanged = true;
        Debug.Log("다음 순서: " + gameManager.playerTurn);
    }

    // 현재 내 상태 초기화
    void Initialize()
    {
        myTurn = false;
        selectingPawn = false;
        pawnSelected = false;
        movingVector[0] = 0;
        movingVector[1] = 0;
        for (int i = 0; i < 10; i++)
            waitYut[i] = 0;
        waitYutTop = -1;
        remainingYut = 0;
    }
}
