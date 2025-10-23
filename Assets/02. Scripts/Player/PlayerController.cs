using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("참조")]
    public PlayerDataSO playerData; //값 주입받기

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
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    { 
        rb.MovePosition(rb.position+moveInput.normalized*playerData.moveSpeed*Time.fixedDeltaTime);
    }
}
