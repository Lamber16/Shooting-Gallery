using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("Screens")]
    [SerializeField]
    GameObject Title;
    [SerializeField]
    GameObject Pregame;
    [SerializeField]
    GameObject Gameplay;
    [SerializeField]
    GameObject Endgame;


    [Header("Pre-game UI")]
    [SerializeField]
    TextMeshProUGUI CountdownText;

    [Header("Gameplay UI")]
    [SerializeField]
    TextMeshProUGUI GameScoreText;
    [SerializeField]
    TextMeshProUGUI TimeRemainingText;
    [SerializeField]
    TextMeshProUGUI Crosshair;

    [Header("Endgame UI")]
    [SerializeField]
    TextMeshProUGUI FinalScoreText;
    [SerializeField]
    TextMeshProUGUI ShotsFiredText;
    [SerializeField]
    TextMeshProUGUI TargetsHitText;
    [SerializeField]
    TextMeshProUGUI AccuracyText;
    [SerializeField]
    [Tooltip("A list of elements to be displayed one at a time, in order.")]
    List<GameObject> EndgameElements;

    private void Update()
    {
        UpdatePregameUI();
        UpdateGameUI();
    }

    public void SetScreen(GameState state)
    {
        Title.SetActive(false);
        Pregame.SetActive(false);
        Gameplay.SetActive(false);
        Endgame.SetActive(false);
        switch (state)
        {
            case GameState.Title:
                Title.SetActive(true);
                break;
            case GameState.Pregame:
                Pregame.SetActive(true);
                break;
            case GameState.Gameplay:
                Gameplay.SetActive(true);
                break;
            case GameState.Endgame:
                SetEndgameUI();
                Endgame.SetActive(true);
                break;
        }
    }

    public void ToggleCrosshair(bool isActive)
    {
        Crosshair.gameObject.SetActive(isActive);
    }

    #region UpdateUI

    void UpdatePregameUI()
    {
        CountdownText.text = GameManager.Instance.GetPregameCountdown().ToString();
    }

    void UpdateGameUI()
    {
        GameScoreText.text = GameManager.Instance.GetScore().ToString("#0000");
        TimeRemainingText.text = GameManager.Instance.GetTimeRemaining().ToString("#.0");
    }

    #endregion

    #region Endgame

    private void SetEndgameUI()
    {
        FinalScoreText.text = GameManager.Instance.GetScore().ToString("#0000");
        ShotsFiredText.text = GameManager.Instance.GetShotsFired().ToString();
        TargetsHitText.text = GameManager.Instance.GetTargetsHit().ToString();

        float accuracy = 0;
        if(GameManager.Instance.GetShotsFired() != 0)  //Avoid division by 0, default to an accuracy of 0 if no shots were fired
        {
            accuracy = (float)GameManager.Instance.GetTargetsHit() / (float)GameManager.Instance.GetShotsFired();
        }

        accuracy *= 100.0f;
        AccuracyText.text = Mathf.RoundToInt(accuracy).ToString() + "%";
    }

    public void ShowNextEndgameElement()
    {
        if(EndgameElements.Count > 0)
        {
            EndgameElements.ElementAt(0).SetActive(true);
            EndgameElements.RemoveAt(0);
        }
        if(EndgameElements.Count == 0)
        {
            GameManager.Instance.UnlockRestart();
        }
    }

    #endregion
}
