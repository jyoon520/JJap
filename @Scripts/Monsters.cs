using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Monsters : MonoBehaviour
{
    public float maxEnemyHealth = 100.0f;
    public float currentEnemyHealth;
    public int attackPower = 10;
    public int defensePower = 5;
    public int level;

    public Image EnemyHpBar;

    public TMP_Text healthText;
    public TMP_Text levelText;

    void Start()
    {
        currentEnemyHealth = maxEnemyHealth;
        level = Random.Range(3, 6);
        levelText.text = $"Lv {level}";
        EnemyHPState();
    }

    public void EnemyHPState()
    {
        if (EnemyHpBar != null)
        {
            EnemyHpBar.fillAmount = currentEnemyHealth / maxEnemyHealth;
        }
        healthText.text = $"{currentEnemyHealth} / {maxEnemyHealth}";

    }
}

// 사용 못함