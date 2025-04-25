using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    public Button StartButton;
    void Start()
    {
        StartButton.onClick.AddListener(OnGameStartButtonClick);
    }

    void OnGameStartButtonClick()
    {
        SceneManager.LoadScene("Game");
    }
}
