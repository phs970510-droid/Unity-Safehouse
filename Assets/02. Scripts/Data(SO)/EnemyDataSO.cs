using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    [Header("기본 스탯")]
    [Tooltip("적 이름")]
    public string enemyName = "Zombie";

    [Tooltip("최대 체력")]
    public float maxHP = 30f;

    [Tooltip("이동 속도")]
    public float moveSpeed = 2f;

    [Tooltip("공격력")]
    public float damage = 5f;

    [Header("기타")]
    [Tooltip("사망 시 드랍 확률 (0~1)")]
    [Range(0f, 1f)] public float dropChance = 0.3f;
}
