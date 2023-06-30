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
    int[] pawnLoc = new int[4] { -1, -1, -1, -1 };  // ���� ���� ��ġ (-1: ��� ���, 0 ~ 28: �ڸ� ��ȣ)

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
            //Debug.Log("pUpdate: ������!");
            // Ÿ�̸� �ɰ� �ð� ���� ������ ���� �� �ϸ� �������� �����̸� �ɵ�
        }
        else if (selectingPawn && pawnSelected && remainingYut > 0) // ������ �� ���� ��: ���� ������ �����ϰ� �������� ��
        {
            // ������ ������ ����: �̵� ������ ������ ǥ�����ֱ�, �����ϸ� waitYut���� �ش� ������ �����ְ� �ڸ� ����, waitYutTop ������Ʈ, remainingYut ������Ʈ
            Debug.Log(gameManager.clickedObject.transform.GetSiblingIndex());
            // (����) �ϴ� �ϳ��� waitYut�� �ִٰ� �������� �� �����̴������� Ȯ��
            // waitYut�� ������ִ� ����� ��� Ž���Ͽ� ������ ������ �� �ְ� �ϱ�

            // ���õ� ���� ���������� �̵�
            //gameManager.clickedObject.transform.position = new Vector2(4f, -4f);

            // �� ��� �ε��� ����
            if (pawnLoc[gameManager.clickedObject.transform.GetSiblingIndex()] == -1)   // ���� ���� ������� �ʾ��� ��
            {
                pawnLoc[gameManager.clickedObject.transform.GetSiblingIndex()] = 0;
            }
            else if (waitYut[0] == 6)   // ���� ����� ���¿��� ���� ���� (���� �߸��ƴµ�)��
            {
                waitYut[0] = -1;
            }

            // ��
            if (waitYut[0] == 7)    
            {
                Debug.Log("���Դϴ�.");
            }
            else
            {
                pawnLoc[gameManager.clickedObject.transform.GetSiblingIndex()] += waitYut[0];
                gameManager.clickedObject.transform.position = GameObject.Find("UI").transform.Find("YutPanel").transform.GetChild(pawnLoc[gameManager.clickedObject.transform.GetSiblingIndex()]).position;
            }

            // ���� �ѱ��
            nextTurn();
        }
    }

    // ���� �ѱ��
    public void nextTurn()
    {
        Initialize();   // ���� �� ���� �ʱ�ȭ

        GameObject.Find("UI").transform.Find("Player_Panel_Group").transform.GetChild(gameManager.playerTurn - 1).gameObject.GetComponent<SpriteRenderer>().material.color = Color.white;
        if (gameManager.playerTurn < gameManager.participantTop + 1)
            gameManager.playerTurn++;
        else
            gameManager.playerTurn = 1;
        GameObject.Find("UI").transform.Find("Player_Panel_Group").transform.GetChild(gameManager.playerTurn - 1).gameObject.GetComponent<SpriteRenderer>().material.color = Color.gray;
        gameManager.turnChanged = true;
        Debug.Log("���� ����: " + gameManager.playerTurn);
    }

    // ���� �� ���� �ʱ�ȭ
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
