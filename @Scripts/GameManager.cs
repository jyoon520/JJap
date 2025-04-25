using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject Luna;

    public Button talkButton;
    public Button NothingButton;
    public Button BattleButton;

    public Text Dialogtext;

    public GameObject LunaDialogUI;
    public GameObject talkUI;

    private PlayerController playercontroller;

    void Start()
    {
        playercontroller = FindAnyObjectByType<PlayerController>();
        talkButton.onClick.AddListener(OntalkButtonClick);
        NothingButton.onClick.AddListener(OnNothingButtonClick);
        BattleButton.onClick.AddListener(OnBattleButtonClick);

        Dialogtext.text = "��Ʋ �ҷ�?";
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, Luna.transform.position) < 1f && !LunaDialogUI.activeSelf) // �÷��̾�� �糪�� �Ÿ� ���̰� 1 �̸� �϶� talk ��ư Ȱ��ȭ
        {
            talkUI.SetActive(true);
        }
        else
        {
            talkUI.SetActive(false);
        }
    }

    void OntalkButtonClick()
    {
        playercontroller.moveSpeed = 0; // ��ȭ �� ������ �ֱ�
        talkUI.SetActive(false);
        LunaDialogUI.SetActive(true);
    }

    void OnNothingButtonClick()
    {
        playercontroller.moveSpeed = 7;
        LunaDialogUI.SetActive(false);
    }

    void OnBattleButtonClick()
    {
        playercontroller.moveSpeed = 7;
        player.transform.position = Luna.transform.position + new Vector3(0,1,0);
        SceneManager.LoadScene("LunaBattle");
    }
}
