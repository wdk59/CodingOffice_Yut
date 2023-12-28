using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInfo: MonoBehaviour
{
    public bool isPlayerOn = false;
    [SerializeField]
    private int myIdx;
    public int nextIdx;         // 방에서는 왼쪽 방향 패널
    [SerializeField]
    private int subIdx = -1;    // 방에서는 목표지점쪽 패널

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
            if (subIdx != -1)    // 모퉁이
            {
                if (this.isPlayerOn == true)    // 모퉁이가 시작
                {
                    this.isPlayerOn = false;
                    goalIdx = nextPanel(subIdx, --yutNum);
                }
                else  // 모퉁이가 시작은 아님
                {
                    goalIdx = nextPanel(nextIdx, --yutNum);
                }
            }
            else if (subIdx == -1)  // 모퉁이 아님
            {
                Debug.Log("nowIdx: " + nowIdx + "nextIdx: " + nextIdx + "yutNum: " + yutNum);
                goalIdx = nextPanel(nextIdx, --yutNum);
            }
        }

        this.isPlayerOn = false;
        return goalIdx;
    }
}
