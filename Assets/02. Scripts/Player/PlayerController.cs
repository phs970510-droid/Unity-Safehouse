using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("참조")]
    //값 주입받기
    public PlayerDataSO playerData;
    //이미지만 마우스 따라 회전하게 하기
    public Transform visual;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        RotateToMouse();
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    { 
        rb.MovePosition(rb.position+moveInput.normalized*playerData.moveSpeed*Time.fixedDeltaTime);
    }

    private void RotateToMouse()
    {
        if (visual == null)
            return;

        //플레이어 기준 위치
        Vector2 playerPos =transform.position;
        //월드에서 마우스 좌표
        Vector2 mousePos=Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //상대 방향 계산
        float dy=mousePos.y-playerPos.y;
        float dx=mousePos.x-playerPos.x;
        
        //각도 계산
        float rotateDeg=Mathf.Atan2(dy,dx)*Mathf.Rad2Deg;

        //회전 적용
        visual.rotation=Quaternion.Euler(0.0f,0.0f,rotateDeg);
    }
}
