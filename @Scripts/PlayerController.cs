using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Animator _animator;
    bool isMove = false;
    public float moveSpeed = 7f;

    public GameObject Exclamationmark;
    public GameObject LudoDialog;
    public GameObject PinkNPCDialog;

    public static Vector3 savedPosition;
    Vector3 exclamationmarkPos = new Vector3(3.22f, 12.27f, 0);
    Vector3 exclamationmarkPos2 = new Vector3(7.5f, 24.66f, 0);
    Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        if (savedPosition != Vector3.zero) 
        {
            transform.position = savedPosition; // 플레이어 위치 저장
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        isMove = false;
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            _animator.SetBool("isMove", true);
            _animator.SetFloat("AxisX", h);
            _animator.SetFloat("AxisY", v);
            Vector3 move = new Vector3(h, v, 0);
            StartCoroutine(Moving(move));
        }
        else
        {
            _animator.SetBool("isMove", false);
        }
    }

    IEnumerator Moving(Vector3 move)
    {
        transform.Translate(move.normalized * moveSpeed * Time.deltaTime);
        isMove = true;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag("Flower") && isMove)
        {
            // 10% 확률로 몬스터 출현
            if (Random.Range(1, 101) <= 10)
            {
                savedPosition = transform.position; // 플레이어 위치 저장
                SceneManager.LoadScene("Battle");
            }
        }
        if (collision.CompareTag("Ludo") && isMove)
        {
            // Ludo와 충돌 했을때
            isMove = false;
            savedPosition = transform.position; // 플레이어 위치 저장
            Instantiate(Exclamationmark, exclamationmarkPos, Quaternion.identity); // 느낌표 생성
            StartCoroutine(LoadSceneAfterDelay(2f)); // 2초 뒤에 배틀씬으로 넘어가기
        }
        if (collision.CompareTag("Pinkplayer") && isMove)
        {
            // Pink와 충돌 했을때
            isMove = false;
            savedPosition = transform.position; // 플레이어 위치 저장
            Instantiate(Exclamationmark, exclamationmarkPos2, Quaternion.identity); // 느낌표 생성
            StartCoroutine(LoadSceneAfterDelay2(2f)); // 2초 뒤에 배틀씬으로 넘어가기
        }
        if (collision.CompareTag("NextMap"))
        {
            SceneManager.LoadScene("Tunnel"); // 다음 맵으로 이동(예정)
        }
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        moveSpeed = 0; // 2초 동안 가만히 있기
        LudoDialog.SetActive(true);
        yield return new WaitForSeconds(delay); // 대기 시간
        moveSpeed = 7;
        LudoDialog.SetActive(false);
        SceneManager.LoadScene("LudoBattle");  // 씬 전환
    }
    private IEnumerator LoadSceneAfterDelay2(float delay)
    {
        moveSpeed = 0; // 2초 동안 가만히 있기
        PinkNPCDialog.SetActive(true);
        yield return new WaitForSeconds(delay); // 대기 시간
        moveSpeed = 7;
        PinkNPCDialog.SetActive(false);
        SceneManager.LoadScene("PinkNPCBattle");  // 씬 전환
    }
}
