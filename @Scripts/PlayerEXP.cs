using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEXP : MonoBehaviour
{ 
    public int playerLevel = 1;
    public int playerExp = 0;
    public int MaxExp = 100;

    public Text levelText;
    public TMP_Text ExpText;
    
    public Image ExpBar;

    PlayerState playerState;
    
    void Start()
    {
        playerState = FindAnyObjectByType<PlayerState>();
        //�ʱ�ȭ�Ҷ�
        //PlayerPrefs.DeleteAll();
        LoadProgress();
        UpdateExpUI();
    }

    // ���� Exp
    public void AddExp(int expAmount)
    {
        playerExp += expAmount;
        CheckLevelUp();
        UpdateExpUI();
    }

    private void CheckLevelUp()
    {
        // ������ ����
        while (playerExp >= MaxExp)
        {
            playerExp -= MaxExp;
            playerLevel++;
            playerState.LevelUp(); // ������ �� ���� ����
            MaxExp = Mathf.RoundToInt(MaxExp * 1.2f);  // MaxExp ���� ����
        }
        SaveProgress();  // ������ �� ����ġ�� ���� ����
    }

    void UpdateExpUI()
    {
        // ����ġ �ٿ� ���� �ؽ�Ʈ ������Ʈ
        if (ExpBar != null)
        {
            ExpBar.fillAmount = (float)playerExp / MaxExp;  // �� ���̴°� �ƴ϶� ä����.
            ExpText.text = $"{playerExp}/{MaxExp}";
        }

        if (levelText != null)
        {
            levelText.text = "Lv " + playerLevel;
        }
    }

    // ������ �� ���� ����
    private void SaveProgress()
    {
        PlayerPrefs.SetInt("PlayerExp", playerExp);
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        PlayerPrefs.SetInt("MaxExp", MaxExp);
        PlayerPrefs.SetFloat("CurrentHealth", playerState.currentHealth);
        PlayerPrefs.SetFloat("MaxHealth", playerState.maxHealth);
        PlayerPrefs.SetInt("AttackPower", playerState.attackPower);
        PlayerPrefs.SetInt("DefensePower", playerState.defensePower);
    }

    // ���� �� ���� �ε�
    private void LoadProgress()
    {
        playerExp = PlayerPrefs.GetInt("PlayerExp", 0);
        playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        MaxExp = PlayerPrefs.GetInt("MaxExp", 100);
        playerState.currentHealth = PlayerPrefs.GetFloat("CurrentHealth", playerState.currentHealth);
        playerState.maxHealth = PlayerPrefs.GetFloat("MaxHealth", playerState.maxHealth);
        playerState.attackPower = PlayerPrefs.GetInt("AttackPower", playerState.attackPower);
        playerState.defensePower = PlayerPrefs.GetInt("DefensePower", playerState.defensePower);
    }
}
