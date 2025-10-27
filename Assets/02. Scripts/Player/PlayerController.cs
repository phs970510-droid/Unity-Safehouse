using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("������ ����")]
    public PlayerDataSO playerData; // �̵� �ӵ� �� �⺻ ������

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Ű �Է� ó��
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // �̵� ó�� (��ֶ������ �밢�� �ӵ� ����)
        Vector2 newPos = rb.position + moveInput.normalized * playerData.moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}
