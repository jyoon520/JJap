using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public int attackPower = 10;
    public int defensePower = 15;

    public Image PlayerHpBar;

    public TMP_Text healthText;

    void Start()
    {
        PlayerHPState();
    }

    public void LevelUp()
    {
        float levelUpHealth = Random.Range(1, 3);
        int levelUpAttack = Random.Range(1, 3);
        int levelUpDefencs = Random.Range(1, 3);
        maxHealth += levelUpHealth;
        attackPower += levelUpAttack;
        defensePower += levelUpDefencs;

        currentHealth = maxHealth;

        PlayerHPState();  // UI 업데이트
    }
    public void PlayerHPState()
    {
        if (PlayerHpBar != null)
        {
            PlayerHpBar.fillAmount = currentHealth / maxHealth;
        }
        if(healthText !=null)
        {
        healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }
}

