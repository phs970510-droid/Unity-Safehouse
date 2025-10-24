using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("����")]
    //�� ���Թޱ�
    public PlayerDataSO playerData;
    //�̹����� ���콺 ���� ȸ���ϰ� �ϱ�
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

        //�÷��̾� ���� ��ġ
        Vector2 playerPos =transform.position;
        //���忡�� ���콺 ��ǥ
        Vector2 mousePos=Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //��� ���� ���
        float dy=mousePos.y-playerPos.y;
        float dx=mousePos.x-playerPos.x;
        
        //���� ���
        float rotateDeg=Mathf.Atan2(dy,dx)*Mathf.Rad2Deg;

        //ȸ�� ����
        visual.rotation=Quaternion.Euler(0.0f,0.0f,rotateDeg);
    }
}
