using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    [Header("�⺻ ����")]
    [Tooltip("�� �̸�")]
    public string enemyName = "Zombie";

    [Tooltip("�ִ� ü��")]
    public float maxHP = 30f;

    [Tooltip("�̵� �ӵ�")]
    public float moveSpeed = 2f;

    [Tooltip("���ݷ�")]
    public float damage = 5f;

    [Header("��Ÿ")]
    [Tooltip("��� �� ��� Ȯ�� (0~1)")]
    [Range(0f, 1f)] public float dropChance = 0.3f;
}
