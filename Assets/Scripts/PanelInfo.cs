using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInfo: MonoBehaviour
{
    public bool isPlayerOn = false;
    [SerializeField]
    private int myIdx;
    public int nextIdx;         // �濡���� ���� ���� �г�
    [SerializeField]
    private int subIdx = -1;    // �濡���� ��ǥ������ �г�

    void Start()
    {
        this.isPlayerOn = false;
        this.myIdx = gameObject.transform.GetSiblingIndex();
        this.nextIdx = transform.GetSiblingIndex() + 1;

        if (this.myIdx == 19 || this.myIdx == 28) { this.nextIdx = 0; }

        switch (myIdx)
        {
            case 5:
                this.subIdx = 20;
                break;
            case 10:
                this.subIdx = 25;
                break;
            case 15:
                this.subIdx = 24;
                break;
            case 22:
                this.subIdx = 27;
                break;
        }
    }

    public int nextPanel(int nowIdx, int yutNum)
    {
        int goalIdx = -1;
        Debug.Log("Index Num: " + this.myIdx);

        if (yutNum >= 0)
        {
            if (subIdx != -1)    // ������
            {
                if (this.isPlayerOn == true)    // �����̰� ����
                {
                    this.isPlayerOn = false;
                    goalIdx = nextPanel(subIdx, --yutNum);
                }
                else  // �����̰� ������ �ƴ�
                {
                    goalIdx = nextPanel(nextIdx, --yutNum);
                }
            }
            else if (subIdx == -1)  // ������ �ƴ�
            {
                Debug.Log("nowIdx: " + nowIdx + "nextIdx: " + nextIdx + "yutNum: " + yutNum);
                goalIdx = nextPanel(nextIdx, --yutNum);
            }
        }

        this.isPlayerOn = false;
        return goalIdx;
    }
}
