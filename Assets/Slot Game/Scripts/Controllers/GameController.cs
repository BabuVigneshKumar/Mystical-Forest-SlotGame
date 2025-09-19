using DG.Tweening;
using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Info Panel Configuration")]
    private int CurrentIndex = 0;
    private bool CanMove;

    [SerializeField] GameHUD gameHUD;
    private void Start()
    {
        ButtonActions();
    }

    private void ButtonActions()
    {  
       //info
        gameHUD.InfoBtn.onClick.AddListener(delegate { OnClickInfo(true); });
        gameHUD.CloseBtn.onClick.AddListener(delegate { OnClickInfo(false); });
        gameHUD.NextBtn.onClick.AddListener(delegate { OnClickNext(); });
        gameHUD.PreviousBtn.onClick.AddListener(delegate { OnClickPrevious(); });

        //welcome
        gameHUD.WelcomeToggle.onValueChanged.AddListener(delegate { OnWelcomeToggleSelection(gameHUD.WelcomeToggle.isOn); });
    }


    #region Welcome Panel
    public void OnWelcomeToggleSelection(bool isOn)
    {
        gameHUD.WelcomeToggle.isOn = isOn;
        PlayerPrefs.SetInt("Toggle", isOn ? 1 : 0);
    }
    #endregion

    #region Info Panel
    private void OnClickInfo(bool canEnable)
    {
        CurrentIndex = 0;
        foreach (RectTransform panel in gameHUD.InfoPanels)
        {
            panel.anchoredPosition = new Vector2(2500, 0);
        }
        gameHUD.InfoPanels[CurrentIndex].anchoredPosition = Vector2.zero;
        CanMove = canEnable;
        gameHUD.InfoPanel.SetActive(canEnable);
    }
    private void OnClickNext()
    {
        if (gameHUD.InfoPanel.activeSelf)
        {
            if (CanMove)
            {
                if (CurrentIndex >= 3) return;
                CanMove = false;
                gameHUD.InfoPanels[CurrentIndex].DOAnchorPosX(-2500, 1).SetEase(Ease.InOutFlash);
                CurrentIndex += 1;
                gameHUD.InfoPanels[CurrentIndex].DOAnchorPosX(0, 1).SetEase(Ease.InOutFlash).OnComplete(() =>
                {
                    CanMove = true;
                });
            }
        }
    }

    private void OnClickPrevious()
    {
        if (gameHUD.InfoPanel.activeSelf)
        {
            if (CanMove)
            {
                if (CurrentIndex <= 0) return;
                CanMove = false;
                gameHUD.InfoPanels[CurrentIndex].DOAnchorPosX(2500, 1).SetEase(Ease.InOutFlash);
                CurrentIndex -= 1;
                gameHUD.InfoPanels[CurrentIndex].DOAnchorPosX(0, 1).SetEase(Ease.InOutFlash).OnComplete(() =>
                {
                    CanMove = true;
                });
            }
        }
    }
    #endregion

}
