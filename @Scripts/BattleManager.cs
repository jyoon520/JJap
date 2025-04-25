using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;

public enum BattleState { PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleManager : MonoBehaviour
{
    public GameObject Roro;
    public GameObject SunBird;
    public GameObject Rock;
    public GameObject Player;
    Vector3 SpawnPos = new Vector3(5, 1.8f, 0);
    Vector3 SpawnPos2 = new Vector3(-5, -1.8f, 0);
    Vector3 TackleReady = new Vector3(-7.14f, -1.73f);

    public GameObject EncounterUI;
    public Text EncounterText;
    public TMP_Text PlayernameText;
    public TMP_Text EnemynameText;
    public Button RunButton;
    public Button BattleButton;

    public GameObject FightUI;
    public Text FightText;
    public Button FightRunButton;
    public Button AttackButton;
    public Button AttackButton2;
    public Button AttackButton3; // �� ��ư

    public BattleState state;

    private readonly float initHp = 100.0f;
    private float EnemyCurrHp;

    public Image EnemyHpBar;
    public TMP_Text EnemyhealthText;
    public Text EnemylevelText;

    public PlayerEXP playerEXP;
    public PlayerState playerState;
    public Skills skill;

    int Enemylevel;
    void Start()
    {
        // �ʱ� ���� ���� ����
        state = BattleState.PLAYERTURN;

        // ���� ���� 3���� 7���� ���� 
        Enemylevel = Random.Range(3, 7);

        // Hp �ʱ�ȭ
        EnemyCurrHp = initHp;
        playerState = FindAnyObjectByType<PlayerState>();
        playerState.currentHealth = playerState.maxHealth;

        // ���� ����
        int random = Random.Range(0, 10);
        GameObject SpawnedMonster;
        string monsterName;

        if (random < 5) // 50�ۼ�Ʈ Ȯ����
        { 
            SpawnedMonster = Instantiate(SunBird, SpawnPos, Quaternion.identity);
            monsterName = SunBird.name;  // Bird�� ������ �̸�
    
        }
        else
        {
            SpawnedMonster = Instantiate(Roro, SpawnPos, Quaternion.identity);
            monsterName = Roro.name;  // Roro�� ������ �̸�

        }

        // �ʱ� �ؽ�Ʈ ����
        EncounterText.text = $"�߻��� {monsterName}��/�� ������!";
        EnemynameText.text = $"{monsterName}";
        PlayernameText.text = "Me";


        RunButton.onClick.AddListener(OnRunButtonClick);
        BattleButton.onClick.AddListener(OnBattleButtonClick);

        FightText.text = "������ �ұ�?";
        FightRunButton.onClick.AddListener(OnFightRunButtonClick);

        AttackButton.onClick.AddListener(OnLeftFightButtonClick);
        AttackButton2.onClick.AddListener(OnRightFightButtonClick);
        AttackButton3.onClick.AddListener(OnRightHealButtonClick);

        EnemyHPState(); 
    }
    private void Update()
    {
        playerState.PlayerHPState();
    }

    void OnRunButtonClick()
    {
        SceneManager.LoadScene("Game");
    }

    void OnBattleButtonClick()
    {
        EncounterUI.SetActive(true);
        FightUI.SetActive(true);
        StartCoroutine(PlayerTurn());
    }

    void OnFightRunButtonClick()
    {
        SceneManager.LoadScene("Game");
    }

    void OnLeftFightButtonClick()
    {
        if (state != BattleState.PLAYERTURN) return;  // �÷��̾� ���� �ƴ� �� �������� ����

        Vector3 TackleCurrentPos = Player.transform.position;
        StartCoroutine(TackleMove(TackleCurrentPos, TackleReady));
        // �÷��̾� ����
        float TackleDamage = 30.0f;
        float TacklePower = TackleDamage + (playerState.attackPower / 2); // ���� ���� ��� ��
        EnemyCurrHp -= TacklePower;
        EnemyHPState();
        playerState.PlayerHPState();
        if (EnemyCurrHp <= 0)
        {
            state = BattleState.WON;
            FightText.text = $"�¸��ϼ̽��ϴ�!";
            GetPlayerEXP(100 + (Enemylevel*2));
            SceneManager.LoadScene("Game");
        }
        else
        {
            FightText.text = $"��뿡�� �������� �������ϴ�!";
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    void OnRightFightButtonClick()
    {

        if (state != BattleState.PLAYERTURN) return;  // �÷��̾� ���� �ƴ� �� �������� ����

        Instantiate(Rock, SpawnPos2, Quaternion.identity); // �� ��ȯ

        // �÷��̾� ����
        float rockDamage = 40.0f;
        float rockPower = rockDamage + (playerState.attackPower / 2); // ���� ���� ����
        EnemyCurrHp -= rockPower;
        EnemyHPState();
        playerState.PlayerHPState();
        if (EnemyCurrHp <= 0)
        {
            state = BattleState.WON;
            FightText.text = $"�¸��ϼ̽��ϴ�!";
            GetPlayerEXP(100 + (Enemylevel*2)); // ����ġ ����
            SceneManager.LoadScene("Game");
        }
        else
        {
            FightText.text = $"��뿡�� �������� �������ϴ�!";
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    void OnRightHealButtonClick()
    {
        if (state != BattleState.PLAYERTURN) return;  // �÷��̾� ���� �ƴ� �� �������� ����

        playerState.currentHealth += 20;
        EnemyHPState();
        playerState.PlayerHPState();
        if (EnemyCurrHp >= 0)
        {
            FightText.text = $"ü���� ȸ���ϼ̽��ϴ�!";
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerTurn()
    {
        FightText.text = "�÷��̾��� ���Դϴ�.";
        yield return new WaitForSeconds(1f); 
    }

    IEnumerator EnemyTurn()
    {
        FightText.text = "���� ���Դϴ�.";
        yield return new WaitForSeconds(1f); // ���� ���� �� ��� �ð�

        float EnemyDamage = 30.0f;
        float EnemyPower = EnemyDamage + Enemylevel - (playerState.defensePower / 2);
        playerState.currentHealth -= EnemyPower;  // ���� �÷��̾� ü���� ���ҽ�Ŵ
        playerState.PlayerHPState();  // UI ������Ʈ

        if (playerState.currentHealth <= 0)
        {
            state = BattleState.LOST;
            FightText.text = $"������";
            SceneManager.LoadScene("Game");
        }
        else
        {
            FightText.text = $"��밡 ��ſ��� �������� �������ϴ�!";
            state = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        }
    }

    void GetPlayerEXP(int expAmount)
    {
        if (playerEXP != null)
        {
            playerEXP.AddExp(expAmount);  
        }
    }


    public void EnemyHPState() // ��� HP���� UI
    {
        EnemyhealthText.text = $"{EnemyCurrHp} / {initHp}";
        if (EnemyHpBar != null)
        {
            EnemyHpBar.fillAmount = EnemyCurrHp / initHp;
        }
        else if (EnemyCurrHp <= 0)
        {
            EnemyCurrHp = 0; 
        }
        if (EnemylevelText != null)
        {
            EnemylevelText.text = "Lv " + Enemylevel;
        }
    }

    IEnumerator TackleMove(Vector3 TackleCurrentPos, Vector3 TackleReady) // ��Ŭ �� �� �̵�
    { 
        // �ڷ� �̵�
        while (Vector3.Distance(Player.transform.position, TackleReady) > 0.1f) // �� �Ÿ��� ���̰� ������
        {
            Player.transform.position = Vector3.MoveTowards(Player.transform.position, TackleReady, 5f * Time.deltaTime);
            yield return null;
        }

        // �ڷ� �̵� �Ϸ� �� ���� ��ġ�� ���ƿ���
        while (Vector3.Distance(Player.transform.position, TackleCurrentPos) > 0.1f) // �� �Ÿ��� ���̰� ������
        {
            Player.transform.position = Vector3.MoveTowards(Player.transform.position, TackleCurrentPos, 20f * Time.deltaTime);
            yield return null;
        }

        // ��Ȯ�� ���� ��ġ�� ���ƿԴ��� Ȯ��
        Player.transform.position = TackleCurrentPos;
    }
}
