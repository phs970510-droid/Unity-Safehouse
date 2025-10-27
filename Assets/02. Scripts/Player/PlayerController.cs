using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("데이터 참조")]
    public PlayerDataSO playerData; // 이동 속도 등 기본 데이터

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 키 입력 처리
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // 이동 처리 (노멀라이즈로 대각선 속도 보정)
        Vector2 newPos = rb.position + moveInput.normalized * playerData.moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}
