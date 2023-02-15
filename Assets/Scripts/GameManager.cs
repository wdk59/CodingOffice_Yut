using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerMove player;      // 플레이어의 차례가 될 때마다 각 차례의 플레이어로 오브젝트 변경

    public GameObject clickedObject = null;    // 클릭 이벤트 발생 시 선택되는 게임 오브젝트 저장

    int[] yutSeat = new int[29];    // 자리별로 위치한 플레이어 구분. 인덱스 순서는 윷 자리에 대한 게임 오브젝트 배치 순서와 동일
    bool[] participant = new bool[] { false, false, false, false };     // 참여자 (최대 4명)
    public int participantTop = -1;     // participant 배열의 마지막 인덱스

    public int playerTurn = 0;          // 어느 플레이어 차례인지 저장해두는 변수
    public bool turnChanged = false;    // 차례가 지금 막 바뀌었는지 알려주는 변수

    short[] yuts = new short[] { 0, 0, 0, 0 };  // 0: 대기, 1: 둥근 면, 2: 평평한 면, 3: 낙
    short[] yutCount = new short[] { 0, 0, 0 };    // 0번 인덱스: 둥근 면의 개수, 1번 인덱스: 평평한 면의 개수, 2번 인덱스: 낙인 윷의 개수

    short result = 0;   // 0: 대기, 1: 도, 2: 개, 3: 걸, 4: 윷, 5: 모, 6: 빽도, 7: 낙

    // Start is called before the first frame update
    void Start()
    {
        /* initializing: 변수 초기화 */
        clickedObject = null;
        participantTop = -1;
        playerTurn = 0;
        turnChanged = false;
        result = 0;
        for (int i = 0; i < 4; i++)
        {
            participant[i] = false;
            yuts[i] = 0;
        }
        for (int i = 0; i < 3; i++)
        {
            yutCount[i] = 0;
        }

        /* 게임 준비 */
        participant[++participantTop] = true;  // user
        participant[++participantTop] = true;  // computer
        GameObject.Find("UI").transform.Find("Yut_Result_Panel").gameObject.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            if (participant[i] == true) {
                GameObject.Find("UI").transform.GetChild(i).gameObject.SetActive(true);
            }
            else {
                GameObject.Find("UI").transform.Find("Player_Panel_Group").transform.GetChild(i).gameObject.SetActive(false);
                GameObject.Find("Player_Group").transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        // 플레이어1에게  차례를 넘겨줌
        playerTurn = 1;
        turnChanged = true;
        GameObject.Find("UI").transform.Find("Player_Panel_Group").transform.GetChild(playerTurn - 1).gameObject.GetComponent<SpriteRenderer>().material.color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 클릭 시 실행
        if (Input.GetMouseButtonDown(0))
        {
            // 오브젝트를 취득해서 변수에 넣음
            clickedObject = GetClickedObject();
            // 말 선택 단계이고, 아직 말을 선택 안 했는지 확인
            // 선택된 오브젝트의 부모 오브젝트의 이름과 현재 차례의 플레이어의 말 그룹 오브젝트의 이름이 같은지 확인
            if (player.selectingPawn && !player.pawnSelected && clickedObject.transform.parent.name == GameObject.Find("Player_Group").transform.GetChild(playerTurn - 1).name)
            {
                player.pawnSelected = true;
            }
            //Debug.Log(clickedObject);
        }

        // 차례 넘어갔을 때 실행
        if (turnChanged)
        {
            Debug.Log("gmUpdate: 차례 바꼈다!");
            turnChanged = false;
            player = GameObject.Find("Player_Group").transform.GetChild(playerTurn - 1).GetComponent<PlayerMove>();
            player.myTurn = true;
            Debug.Log("gmUpdate 차례: " + GameObject.Find("Player_Group").transform.GetChild(playerTurn - 1));

            // 윷을 던져서 결과를 플레이어 클래스의 배열에 저장해준다.
            ThrowYuts();
        }
    }

    // 클릭된 오브젝트 받아오기: 마우스 클릭 시 실행
    GameObject GetClickedObject()
    {
        GameObject target = null;

        // 마우스 위치 취득
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 마우스 위치로 레이 던짐
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

        // 던진 레이가 히트되었다면 실행
        if (hit.collider != null)
        {
            // 오브젝트를 취득해서 변수에 넣음
            target = hit.collider.transform.gameObject;
            Debug.Log(target);
        }

        return target;
    }

    // throw yuts: 윷 값을 랜덤으로 받고 결과 나타내기
    public void ThrowYuts()
    {
        GameObject.Find("UI").transform.Find("Yut_Result_Panel").gameObject.SetActive(true);

        for (int i = 0; i < 4; i++)
        {
            // 윷 값 정하기
            yuts[i] = (short)Random.Range(1, 51);   // 낙이 나올지 정하기 (윷 당 확률: 2%). 낙: 3
            if (yuts[i] != 3)
            {
                yuts[i] = (short)Random.Range(1, 3);   // 1~2 중 랜덤으로 고르기
            }
            // 윷 값에 따라 개수 세기
            switch (yuts[i])
            {
                case 1:             // 둥근 면
                    yutCount[0]++;
                    break;
                case 2:             // 평평한 면
                    yutCount[1]++;
                    break;
                case 3:             // 낙
                    yutCount[2]++;
                    break;
                default:
                    break;
            }
        }

        // 결과 판정
        result = (short)Random.Range(1, 21);   // 뒷도가 나올지 정하기 (확률: 5%). 빽도: 6
        if (result != 6) // 뒷도가 아닌 경우, 윷의 값에 따라서 결과가 정해짐
        {
            if (yutCount[2] > 0)    // 낙: 윷 값에 낙이 하나라도 있는 경우
            {
                result = 7;
            } else if (yutCount[0] == 0)    // 윷: 둥근 면 0, 평평한 면 4
            {
                result = 4;
            } else if (yutCount[0] == 1)    // 도: 둥근 면 1, 평평한 면 3
            {
                result = 1;
            } else if (yutCount[0] == 2)    // 개: 둥근 면 2, 평평한 면 2
            {
                result = 2;
            } else if (yutCount[0] == 3)    // 걸: 둥근 면 3, 평평한 면 1
            {
                result = 3;
            } else if (yutCount[0] == 4)    // 모: 둥근 면 4, 평평한 면 0
            {
                result = 5;
            }
        }

        // 플레이어에게 결과 전달
        player.waitYut[++player.waitYutTop] = result;
        player.remainingYut += player.waitYut[player.waitYutTop];
        Debug.Log("gm_ThrowYuts: player.waitYut List");
        for (int i = 0; i <= player.waitYutTop; i++)
            Debug.Log(player.waitYut[i]);

        // 던지는 모습 표현 및 시간 지연 (2초)
        StartCoroutine(ThrowingYutTimer());

        /* 윷 결과 보여주기 및 변수 초기화는 코루틴 종료 후 ShowingYutResult 함수에서 수행 */
    }

    /* 코루틴 함수 (Coroutine) */

    // 윷 던지는 시간 타이머
    IEnumerator ThrowingYutTimer()
    {
        yield return new WaitForSeconds(2.0f);  // 유니티 시간 기준 초단위. 유니티 시간을 배속하면 빨라지거나 느려질 수 있음.
        Debug.Log("2초!");
        ShowingYutResult();
    }
    void ShowingYutResult() // 윷 값 보여주기
    {
        // 타이머 코루틴 멈추기
        StopCoroutine(ThrowingYutTimer());

        // 윷 결과 보여주기 (3초)
        GameObject.Find("Yut_Result_Panel").transform.GetChild(0).transform.GetChild(result - 1).gameObject.SetActive(true);
        StartCoroutine(ShowingYutTimer());
    }

    // 윷 값 보여주는 시간 타이머
    IEnumerator ShowingYutTimer()
    {
        yield return new WaitForSeconds(3.0f);  // 유니티 시간 기준 초단위. 유니티 시간을 배속하면 빨라지거나 느려질 수 있음.
        Debug.Log("3초!");
        HidingYutResult();
    }
    void HidingYutResult()  // 윷 값 숨기기
    {
        // 타이머 코루틴 멈추기
        StopCoroutine(ShowingYutTimer());

        // 게임 오브젝트 상태 및 사용된 변수 초기화
        GameObject.Find("UI").transform.Find("Yut_Result_Panel").transform.Find("Canvas").transform.GetChild(result - 1).gameObject.SetActive(false);
        GameObject.Find("UI").transform.Find("Yut_Result_Panel").gameObject.SetActive(false);
        yuts = new short[] { 0, 0, 0, 0 };
        yutCount = new short[] { 0, 0, 0 };
        result = 0;

        // 결과가 윷이나 모일 때는 윷을 다시 던지고, 아니면 윷 던지기 끝내기
        if (player.waitYut[player.waitYutTop] == 4 || player.waitYut[player.waitYutTop] == 5)
            ThrowYuts();
        else
        {
            clickedObject = null;
            player.selectingPawn = true;
        }
    }
}
