using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyForUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Enemy enemy;

    private void Start()
    {
        if (enemy == null)
            enemy = GetComponentInParent<Enemy>();
    }

    private void Update()
    {
        if (enemy != null && enemy.enemyData != null && hpSlider != null)
        {
            float ratio = Mathf.Clamp01(enemy.CurrentHP / enemy.enemyData.maxHP);
            hpSlider.value = ratio;
        }
    }
}
