using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    bool canMove = false;   // ���ʰ� �Ǹ� ���� ���� �Ŀ� �����̵��� ����
    int[] movingVector = new int[] { 0, 0 };    // ���� ������ ����� ũ�⸦ ����
    int[] waitYut = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // 1: ��, 2: ��, 3: ��, 4: ��, 5: ��, 6: ����, 7: ��

    // Start is called before the first frame update
    void Start()
    {
        canMove = false;
        movingVector[0] = 0;
        movingVector[1] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
