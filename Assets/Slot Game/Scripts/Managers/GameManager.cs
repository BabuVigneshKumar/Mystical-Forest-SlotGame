using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [Header("Win Detection")]
    private List<SlotData> winningSlots = new List<SlotData>();
    [SerializeField] float WinAmt = 0;
    public Action<float> WinAmountAction;

    [Header("Betting Configuration")]
    [SerializeField] private float[] betLevels;
    private int currentBetIndex = 0;
    [SerializeField] float CurrentBetAmt = 0;
    private event Action<float> BetAmountAction;

    [Header("Balance Configuration")]
    [SerializeField] float PlayerBalance = 0;
    [SerializeField] private float initialBalance = 2000f;
    public Action<float> PlayerBalanceAction;

    [Header("Paytable Configuration")]
    [SerializeField] public List<PayTable> payTableValues;
    [SerializeField] private List<PayTable> configurablePayTable;

    [Header("Script references ")]
    [SerializeField] GameHUD gameHUD;
    [SerializeField] ReelsManager reelsManager;
    [SerializeField] GameController controller;
    [SerializeField] PaylineManager paylineManager;
    private void Awake()
    {
        controller.OnWelcomeToggleSelection(false);
        DOTween.SetTweensCapacity(1000, 200);
    }
    private void Start()
    {
        gameHUD.SpinBtn.onClick.AddListener(OnClickSpinBtn);

        gameHUD.PlusBtn.onClick.AddListener(delegate { IncreaseBet(); });
        gameHUD.MinusBtn.onClick.AddListener(delegate { DecreaseBet(); });
        gameHUD.stopBtn.onClick.AddListener(delegate { reelsManager.StopAllReels(); });
        SetPlayerBalance();
        payTableValues = configurablePayTable;
    }
    private void OnEnable()
    {
        BetAmountAction += UpdateBetAmount;
        PlayerBalanceAction += UpdatePlayerAmount;
        WinAmountAction += UpdateWinAmount;
    }
    private void OnDisable()
    {
        BetAmountAction -= UpdateBetAmount;
        PlayerBalanceAction -= UpdatePlayerAmount;
        WinAmountAction -= UpdateWinAmount;
    }

    #region Betting System 

    public void IncreaseBet()
    {
        if (currentBetIndex < betLevels.Length - 1)
        {
            currentBetIndex++;
            CurrentBetAmt = betLevels[currentBetIndex];
            BetAmountAction?.Invoke(CurrentBetAmt);
        }
        CheckPlusMinus();
    }
    public void DecreaseBet()
    {
        if (currentBetIndex > 0)
        {
            currentBetIndex--;
            CurrentBetAmt = betLevels[currentBetIndex];
            BetAmountAction?.Invoke(CurrentBetAmt);
        }
        CheckPlusMinus();
    }
    public void CheckPlusMinus()
    {
        float val = CurrentBetAmt;
        gameHUD.PlusBtn.interactable = (val < 25f);
        gameHUD.MinusBtn.interactable = (val > 0.10f);
    }

    public void UpdateBetAmount(float amount)
    {
        Debug.Log("Bet Amount ******* " + amount);
        gameHUD.SetBetAmount(amount);
    }
    public void UpdateWinAmount(float amount)
    {
        Debug.Log("Win Amount ******* " + amount);
        gameHUD.SetWinAmount(amount);
    }


    public void UpdatePlayerAmount(float amount)
    {
        Debug.Log("Player Balance ******* " + amount);
        gameHUD.SetPlayerBalance(amount);
    }
    private void SetPlayerBalance()
    {
        PlayerBalance = initialBalance;
        PlayerBalanceAction?.Invoke(PlayerBalance);

        CurrentBetAmt = 0.10f;
        BetAmountAction?.Invoke(CurrentBetAmt);

        WinAmt = 0;
        WinAmountAction?.Invoke(WinAmt);
    }

    #endregion

    #region Spin Logics

    private void OnClickSpinBtn()
    {
        if (reelsManager != null)
        {
            reelsManager.StartSpin();
            AudioController.Instance.PlayReelSpin();
            UpdateBalanceOnBet();
        }
    }
    public void UpdateBalanceOnBet()
    {
        PlayerBalance -= CurrentBetAmt;
        PlayerBalanceAction?.Invoke(PlayerBalance);
    }

    #endregion

    #region Win Detection and Calculation
    public void CheckForWins(SlotManagers[] allReels)
    {
        float totalWin = 0f;
        winningSlots.Clear();
        List<PaylineWin> paylineWins = new List<PaylineWin>();

        totalWin += CheckAllPaylines(allReels, paylineWins);

        totalWin += CheckScatterWins(allReels);

        if (totalWin > 0)
        {
            WinAmt += totalWin;
            WinAmountAction?.Invoke(WinAmt);
            PlayerBalance += totalWin;
            PlayerBalanceAction?.Invoke(PlayerBalance);

            AnimateWinningSymbols();
            Debug.Log($"Total Win: ${totalWin:F2}");
            UpdateBetAmount(totalWin);
            foreach (var win in paylineWins)
            {
                Debug.Log($"Payline {win.paylineNumber}: {win.symbolRank} x{win.matchCount} = ${win.winAmount:F2}");
            }
            gameHUD.ShowResult();
        }
        else
        {
            Debug.Log("No wins this spin");
            AudioController.Instance.PlayError();

        }
        StartCoroutine(ClearUI());
    }

    private IEnumerator ClearUI()
    {
        yield return new WaitForSeconds(2f);

        ResetBetAndWinAmounts();
    }

    private float CheckAllPaylines(SlotManagers[] allReels, List<PaylineWin> paylineWins)
    {
        float totalPaylineWin = 0f;
        var allPaylines = paylineManager.GetAllPaylines();

        foreach (var payline in allPaylines)
        {
            if (paylineManager.CheckPaylineWin(allReels, payline.lineNumber,
                out SlotRank winningRank, out int matchCount, out List<SlotData> winningSymbols))
            {
                float winAmount = CalculateWinAmount(winningRank, matchCount);
                if (winAmount > 0)
                {
                    totalPaylineWin += winAmount;
                    winningSlots.AddRange(winningSymbols);
                    paylineWins.Add(new PaylineWin
                    {
                        paylineNumber = payline.lineNumber,
                        symbolRank = winningRank,
                        matchCount = matchCount,
                        winAmount = winAmount,
                        winningSlots = winningSymbols
                    });
                }
            }
        }
        return totalPaylineWin;
    }
    private float CalculateWinAmount(SlotRank rank, int matchCount)
    {
        PayTable payTable = payTableValues.Find(x => x.symbolRank == rank);
        if (payTable != null)
        {
            switch (matchCount)
            {
                case 3: return CurrentBetAmt * payTable.threeRows;
                case 4: return CurrentBetAmt * payTable.fourRows;
                case 5: return CurrentBetAmt * payTable.fiveRows;
                default: return 0f;
            }
        }
        return 0f;
    }
    private void AnimateWinningSymbols()
    {
        foreach (var slot in winningSlots)
        {
            slot.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f)
                .SetEase(Ease.OutBack)
                .SetLoops(2, LoopType.Yoyo);
        }
        AudioController.Instance.PlayWinning();
    }

    private SlotRank GetSymbolRank(SlotData slot)
    {
        if (slot != null && slot.SymbolData != null)
        {
            return slot.SymbolData.symbolType;
        }
        return SlotRank.TEN;
    }

    private float CheckScatterWins(SlotManagers[] allReels)
    {
        int scatterCount = 0;
        List<SlotData> scatterSlots = new List<SlotData>();

        foreach (var reel in allReels)
        {
            foreach (var slot in reel.Slots)
            {
                if (GetSymbolRank(slot) == SlotRank.SCATTER)
                {
                    scatterCount++;
                    scatterSlots.Add(slot);
                }
            }
        }

        if (scatterCount >= 3)
        {
            float scatterWin = CurrentBetAmt * scatterCount * 10f;
            winningSlots.AddRange(scatterSlots);

            Debug.Log($"Scatter win: {scatterCount} scatters = ${scatterWin:F2}");
            return scatterWin;
        }

        return 0f;
    }
    private void ResetBetAndWinAmounts()
    {
        WinAmt = 0f;
        WinAmountAction?.Invoke(WinAmt);
        if (betLevels != null && betLevels.Length > 0)
        {
            currentBetIndex = 0;
            CurrentBetAmt = betLevels[currentBetIndex];
            BetAmountAction?.Invoke(CurrentBetAmt);
        }
        CheckPlusMinus();
        gameHUD.HideResult();
    }
    #endregion

}
[Serializable]
public class SlotManagers
{
    public List<SlotData> Slots = new List<SlotData>();
}
[System.Serializable]
public class PaylineWin
{
    public int paylineNumber;
    public SlotRank symbolRank;
    public int matchCount;
    public float winAmount;
    public List<SlotData> winningSlots;
}