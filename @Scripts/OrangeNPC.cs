using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OrangeNPC : MonoBehaviour
{
    public GameObject player;
    public GameObject OrangeNPC2;

    public Button HealButton;
    public Button NothingButton2;

    public Button talk2Button;
    public Text HealText;
    public Text Dialogtext2;

    public GameObject talkUI2;
    public GameObject OrangeNPCDialogUI;

    private PlayerController playercontroller;
    private PlayerState playerstate;

    void Start()
    {
        playercontroller = FindAnyObjectByType<PlayerController>();
        playerstate = FindAnyObjectByType<PlayerState>();
        talk2Button.onClick.AddListener(Ontalk2ButtonClick);
        NothingButton2.onClick.AddListener(OnNothingButton2Click);
        HealButton.onClick.AddListener(OnHealButtonClick);

        LoadProgress();
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, OrangeNPC2.transform.position) < 1f && !OrangeNPCDialogUI.activeSelf)
        {
            talkUI2.SetActive(true);
        }
        else
        {
            talkUI2.SetActive(false);
        }
    }

    void Ontalk2ButtonClick()
    {
        Dialogtext2.text = "치료가 필요하세요?";
        playercontroller.moveSpeed = 0;
        talkUI2.SetActive(false);
        OrangeNPCDialogUI.SetActive(true);
    }

    void OnNothingButton2Click()
    {
        playercontroller.moveSpeed = 7;
        OrangeNPCDialogUI.SetActive(false);
    }

    void OnHealButtonClick()
    {
        playerstate.currentHealth = playerstate.maxHealth;
        playerstate.PlayerHPState();
        StartCoroutine(GetgoingAfterdelay(2f));
        SaveProgress();
    }

    private IEnumerator GetgoingAfterdelay(float delay)
    {
        Dialogtext2.text = "치료가 완료되었어요!";
        yield return new WaitForSeconds(delay); // 대기 시간
        OrangeNPCDialogUI.SetActive(false);
        playercontroller.moveSpeed = 7;
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetFloat("CurrentHealth", playerstate.currentHealth);
        PlayerPrefs.SetFloat("MaxHealth", playerstate.maxHealth);

    }

    private void LoadProgress()
    {
        playerstate.currentHealth = PlayerPrefs.GetFloat("CurrentHealth", playerstate.currentHealth);
        playerstate.maxHealth = PlayerPrefs.GetFloat("MaxHealth", playerstate.maxHealth);
    }
}
