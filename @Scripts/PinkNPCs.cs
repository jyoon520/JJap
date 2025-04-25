using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum BattleState4 { PLAYERTURN, ENEMYTURN, WON, LOST }

public class PinkNPCs : MonoBehaviour
{
    public GameObject Croky;
    public GameObject SunBird;
    public GameObject Rock;
    public GameObject Player;
    public GameObject Pink;
    Vector3 SpawnPos = new Vector3(5, 1.11f, 0);
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

    public BattleState3 state;

    private readonly float initHp = 100.0f;
    private float EnemyCurrHp;

    public Image EnemyHpBar;
    public TMP_Text EnemyhealthText;
    public Text EnemylevelText;

    public PlayerEXP playerEXP;
    public PlayerState playerState;


    public int Enemylevel = 20;

    private bool hasEnemySpawned = false;
    private GameObject currentEnemy;
    void Start()
    {
        // �ʱ� ���� ���� ����
        state = BattleState3.PLAYERTURN;

        // Hp �ʱ�ȭ
        EnemyCurrHp = initHp;
        playerState = FindAnyObjectByType<PlayerState>();
        playerState.currentHealth = playerState.maxHealth;

        EncounterText.text = $"Pink�� ��Ʋ�� ��û�ؿԴ�!";

        StartCoroutine(EnemyEntry(2.5f)); // 2.5�ʵڿ� ���� ����

        // ���� ����

        string monsterName;

        monsterName = Croky.name;  // ������ �̸�

        // �ʱ� �ؽ�Ʈ ����
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
        if (state != BattleState3.PLAYERTURN) return;  // �÷��̾� ���� �ƴ� �� �������� ����

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
            state = BattleState3.WON;
            FightText.text = $"�¸��ϼ̽��ϴ�!";
            GetPlayerEXP(200 + (Enemylevel * 3));
            SceneManager.LoadScene("Game");
            
            // ���ο� ���� ��ȯ
            if (!hasEnemySpawned)
            {
                StartCoroutine(SpawnNewEnemy());
                hasEnemySpawned = true; // �� ���� ��ȯ �ϵ��� ����
            }
            else
            {
                FightText.text = "������ �������ϴ�.";
                SceneManager.LoadScene("Game");
            }
        }
        else
        {
            FightText.text = $"��뿡�� �������� �������ϴ�!";
            state = BattleState3.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    void OnRightFightButtonClick()
    {
        if (state != BattleState3.PLAYERTURN) return;  // �÷��̾� ���� �ƴ� �� �������� ����

        Instantiate(Rock, SpawnPos2, Quaternion.identity); // �� ��ȯ

        // �÷��̾� ����
        float rockDamage = 40.0f;
        float rockPower = rockDamage + (playerState.attackPower / 2); // ���� ���� ����
        EnemyCurrHp -= rockPower;
        EnemyHPState();
        playerState.PlayerHPState();
        
        if (EnemyCurrHp <= 0)
        {
            state = BattleState3.WON;
            FightText.text = $"�¸��ϼ̽��ϴ�!";
            GetPlayerEXP(200 + (Enemylevel * 3)); // ����ġ ����

            // ���ο� ���� ��ȯ
            if (!hasEnemySpawned)
            {
                StartCoroutine(SpawnNewEnemy());
                hasEnemySpawned = true; // �� ���� ��ȯ �ϵ��� ����
            }
            else 
            {
                FightText.text = "������ �������ϴ�.";
                SceneManager.LoadScene("Game");
            }
        }
        else
        {
            FightText.text = $"��뿡�� �������� �������ϴ�!";
            state = BattleState3.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    void OnRightHealButtonClick()
    {
        if (state != BattleState3.PLAYERTURN) return;  // �÷��̾� ���� �ƴ� �� �������� ����

        playerState.currentHealth += 20;
        EnemyHPState();
        playerState.PlayerHPState();
        if (EnemyCurrHp >= 0)
        {
            FightText.text = $"ü���� ȸ���ϼ̽��ϴ�!";
            state = BattleState3.ENEMYTURN;
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

        float EnemyDamage = 50.0f;
        float EnemyPower = EnemyDamage + Enemylevel - (playerState.defensePower / 2);
        playerState.currentHealth -= EnemyPower;  // ���� �÷��̾� ü���� ���ҽ�Ŵ
        playerState.PlayerHPState();  // UI ������Ʈ

        if (playerState.currentHealth <= 0)
        {
            state = BattleState3.LOST;
            FightText.text = $"������";
            SceneManager.LoadScene("Game");
        }
        else
        {
            FightText.text = $"��밡 ��ſ��� �������� �������ϴ�!";
            state = BattleState3.PLAYERTURN;
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


    public void EnemyHPState()
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

    IEnumerator EnemyEntry(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð�
        currentEnemy = Instantiate(Croky, SpawnPos, Quaternion.identity); // ���� ����
    }

    IEnumerator SpawnNewEnemy()
    {
        // ���� ���� ����
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);
        }

        yield return new WaitForSeconds(1f); // 1�� ��� ��

        // ���ο� ���� ����
        EnemyCurrHp = initHp; // ���� HP �ʱ�ȭ
        Enemylevel += 5; // �� ���� ����

        currentEnemy = Instantiate(SunBird, SpawnPos, Quaternion.identity);

        // ���� UI ������Ʈ
        EnemynameText.text = SunBird.name;
        EnemyHPState();

        FightText.text = "Pink�� SunBird�� �����´�";
        state = BattleState3.PLAYERTURN;
    }
}


