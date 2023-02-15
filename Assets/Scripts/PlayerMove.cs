using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;

    public bool myTurn = false;         // �� �������� �Ǻ��ϴ� ����
    public bool selectingPawn = false;  // �� ������ �� ������ ���� �����ϴ� �ܰ����� �Ǻ��ϴ� ����
    public bool pawnSelected = false;          // ������ ���� �����ߴ��� �Ǻ��ϴ� ����

    int[] movingVector = new int[] { 0, 0 };    // ���� ������ ����� ũ�⸦ ����

    public int[] waitYut = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // 1: ��, 2: ��, 3: ��, 4: ��, 5: ��, 6: ����, 7: ��
    public int waitYutTop = -1;             // waitYut�� ������ �ε����� ����Ŵ (-1�̸� �ƹ��͵� ����)
    public int remainingYut = 0;            // waitYut�� �ƹ��͵� ������ Ȯ��. waitYut�� ���� �ٲ� ������ ������Ʈ.

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        // �� ���ʿ� ���� �� ������, ��⿭(waitYut)�� ���� �̵� Ƚ���� ���� ������ ������ �� ���� (GM�� HidingYutResult �Լ����� selectingPawn�� true�� �ٲ�)
        if (selectingPawn && !pawnSelected && remainingYut > 0) // ���� ������ �� ���� �� ��
        {
            Debug.Log("pUpdate: ������!");
            // Ÿ�̸� �ɰ� �ð� ���� ������ ���� �� �ϸ� �������� �����̸� �ɵ�
        }
        else if (selectingPawn && pawnSelected && remainingYut > 0) // ������ �� ���� ��: ���� ������ �����ϰ� �������� ��
        {
            // ������ ������ ����: �̵� ������ ������ ǥ�����ֱ�, �����ϸ� waitYut���� �ش� ������ �����ְ� �ڸ� ����, waitYutTop ������Ʈ, remainingYut ������Ʈ

            // ���õ� ���� ���������� �̵�
            gameManager.clickedObject.transform.position = new Vector2(4f, -4f);

            // ���� �� ���� �ʱ�ȭ
            Initialize();
            // ���� �ѱ��
            GameObject.Find("UI").transform.Find("Player_Panel_Group").transform.GetChild(gameManager.playerTurn - 1).gameObject.GetComponent<SpriteRenderer>().material.color = Color.white;
            if (gameManager.playerTurn < gameManager.participantTop + 1)
                gameManager.playerTurn++;
            else
                gameManager.playerTurn = 1;
            GameObject.Find("UI").transform.Find("Player_Panel_Group").transform.GetChild(gameManager.playerTurn - 1).gameObject.GetComponent<SpriteRenderer>().material.color = Color.gray;
            gameManager.turnChanged = true;
            Debug.Log("���� ����: " + gameManager.playerTurn);
        }
    }

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
