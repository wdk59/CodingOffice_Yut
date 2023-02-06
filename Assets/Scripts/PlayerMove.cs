using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    bool canMove = false;   // 차례가 되면 윷을 던진 후에 움직이도록 제어
    int[] movingVector = new int[] { 0, 0 };    // 말이 움직일 방향과 크기를 가짐
    int[] waitYut = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // 1: 도, 2: 개, 3: 걸, 4: 윷, 5: 모, 6: 빽도, 7: 낙

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
