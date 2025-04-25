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
        //초기화할때
        //PlayerPrefs.DeleteAll();
        LoadProgress();
        UpdateExpUI();
    }

    // 얻은 Exp
    public void AddExp(int expAmount)
    {
        playerExp += expAmount;
        CheckLevelUp();
        UpdateExpUI();
    }

    private void CheckLevelUp()
    {
        // 레벨업 조건
        while (playerExp >= MaxExp)
        {
            playerExp -= MaxExp;
            playerLevel++;
            playerState.LevelUp(); // 레벨업 후 스탯 증가
            MaxExp = Mathf.RoundToInt(MaxExp * 1.2f);  // MaxExp 증가 비율
        }
        SaveProgress();  // 레벨업 후 경험치와 레벨 저장
    }

    void UpdateExpUI()
    {
        // 경험치 바와 레벨 텍스트 업데이트
        if (ExpBar != null)
        {
            ExpBar.fillAmount = (float)playerExp / MaxExp;  // 바 깎이는게 아니라 채워짐.
            ExpText.text = $"{playerExp}/{MaxExp}";
        }

        if (levelText != null)
        {
            levelText.text = "Lv " + playerLevel;
        }
    }

    // 레벨업 후 상태 저장
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

    // 저장 된 스탯 로드
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
