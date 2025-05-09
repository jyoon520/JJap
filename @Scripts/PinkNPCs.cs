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
    public Button AttackButton3; // 힐 버튼

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
        // 초기 전투 상태 설정
        state = BattleState3.PLAYERTURN;

        // Hp 초기화
        EnemyCurrHp = initHp;
        playerState = FindAnyObjectByType<PlayerState>();
        playerState.currentHealth = playerState.maxHealth;

        EncounterText.text = $"Pink가 배틀을 신청해왔다!";

        StartCoroutine(EnemyEntry(2.5f)); // 2.5초뒤에 몬스터 생성

        // 몬스터 생성

        string monsterName;

        monsterName = Croky.name;  // 프리팹 이름

        // 초기 텍스트 설정
        EnemynameText.text = $"{monsterName}";
        PlayernameText.text = "Me";



        RunButton.onClick.AddListener(OnRunButtonClick);
        BattleButton.onClick.AddListener(OnBattleButtonClick);

        FightText.text = "무엇을 할까?";
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
        if (state != BattleState3.PLAYERTURN) return;  // 플레이어 턴이 아닐 때 공격하지 않음

        Vector3 TackleCurrentPos = Player.transform.position;
        StartCoroutine(TackleMove(TackleCurrentPos, TackleReady));
        // 플레이어 공격
        float TackleDamage = 30.0f;
        float TacklePower = TackleDamage + (playerState.attackPower / 2); // 대충 세운 계산 식
        EnemyCurrHp -= TacklePower;
        EnemyHPState();
        playerState.PlayerHPState();
        if (EnemyCurrHp <= 0)
        {
            state = BattleState3.WON;
            FightText.text = $"승리하셨습니다!";
            GetPlayerEXP(200 + (Enemylevel * 3));
            SceneManager.LoadScene("Game");
            
            // 새로운 몬스터 소환
            if (!hasEnemySpawned)
            {
                StartCoroutine(SpawnNewEnemy());
                hasEnemySpawned = true; // 한 번만 소환 하도록 설정
            }
            else
            {
                FightText.text = "전투가 끝났습니다.";
                SceneManager.LoadScene("Game");
            }
        }
        else
        {
            FightText.text = $"상대에게 데미지를 입혔습니다!";
            state = BattleState3.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    void OnRightFightButtonClick()
    {
        if (state != BattleState3.PLAYERTURN) return;  // 플레이어 턴이 아닐 때 공격하지 않음

        Instantiate(Rock, SpawnPos2, Quaternion.identity); // 돌 소환

        // 플레이어 공격
        float rockDamage = 40.0f;
        float rockPower = rockDamage + (playerState.attackPower / 2); // 대충 세운 계산식
        EnemyCurrHp -= rockPower;
        EnemyHPState();
        playerState.PlayerHPState();
        
        if (EnemyCurrHp <= 0)
        {
            state = BattleState3.WON;
            FightText.text = $"승리하셨습니다!";
            GetPlayerEXP(200 + (Enemylevel * 3)); // 경험치 얻음

            // 새로운 몬스터 소환
            if (!hasEnemySpawned)
            {
                StartCoroutine(SpawnNewEnemy());
                hasEnemySpawned = true; // 한 번만 소환 하도록 설정
            }
            else 
            {
                FightText.text = "전투가 끝났습니다.";
                SceneManager.LoadScene("Game");
            }
        }
        else
        {
            FightText.text = $"상대에게 데미지를 입혔습니다!";
            state = BattleState3.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    void OnRightHealButtonClick()
    {
        if (state != BattleState3.PLAYERTURN) return;  // 플레이어 턴이 아닐 때 공격하지 않음

        playerState.currentHealth += 20;
        EnemyHPState();
        playerState.PlayerHPState();
        if (EnemyCurrHp >= 0)
        {
            FightText.text = $"체력을 회복하셨습니다!";
            state = BattleState3.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerTurn()
    {
        FightText.text = "플레이어의 턴입니다.";
        yield return new WaitForSeconds(1f);
    }

    IEnumerator EnemyTurn()
    {
        FightText.text = "적의 턴입니다.";
        yield return new WaitForSeconds(1f); // 적의 공격 간 대기 시간

        float EnemyDamage = 50.0f;
        float EnemyPower = EnemyDamage + Enemylevel - (playerState.defensePower / 2);
        playerState.currentHealth -= EnemyPower;  // 실제 플레이어 체력을 감소시킴
        playerState.PlayerHPState();  // UI 업데이트

        if (playerState.currentHealth <= 0)
        {
            state = BattleState3.LOST;
            FightText.text = $"슬프다";
            SceneManager.LoadScene("Game");
        }
        else
        {
            FightText.text = $"상대가 당신에게 데미지를 입혔습니다!";
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

    IEnumerator TackleMove(Vector3 TackleCurrentPos, Vector3 TackleReady) // 태클 뒤 앞 이동
    {
        // 뒤로 이동
        while (Vector3.Distance(Player.transform.position, TackleReady) > 0.1f) // 두 거리에 차이가 있을때
        {
            Player.transform.position = Vector3.MoveTowards(Player.transform.position, TackleReady, 5f * Time.deltaTime);
            yield return null;
        }

        // 뒤로 이동 완료 후 원래 위치로 돌아오기
        while (Vector3.Distance(Player.transform.position, TackleCurrentPos) > 0.1f) // 두 거리에 차이가 있을때
        {
            Player.transform.position = Vector3.MoveTowards(Player.transform.position, TackleCurrentPos, 20f * Time.deltaTime);
            yield return null;
        }

        // 정확히 원래 위치로 돌아왔는지 확인
        Player.transform.position = TackleCurrentPos;
    }

    IEnumerator EnemyEntry(float delay)
    {
        yield return new WaitForSeconds(delay); // 딜레이 시간
        currentEnemy = Instantiate(Croky, SpawnPos, Quaternion.identity); // 몬스터 생성
    }

    IEnumerator SpawnNewEnemy()
    {
        // 기존 몬스터 제거
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);
        }

        yield return new WaitForSeconds(1f); // 1초 대기 후

        // 새로운 몬스터 생성
        EnemyCurrHp = initHp; // 적의 HP 초기화
        Enemylevel += 5; // 적 레벨 증가

        currentEnemy = Instantiate(SunBird, SpawnPos, Quaternion.identity);

        // 전투 UI 업데이트
        EnemynameText.text = SunBird.name;
        EnemyHPState();

        FightText.text = "Pink는 SunBird를 내보냈다";
        state = BattleState3.PLAYERTURN;
    }
}


