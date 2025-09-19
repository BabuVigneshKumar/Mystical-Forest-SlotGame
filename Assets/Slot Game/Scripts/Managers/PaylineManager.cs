using System.Collections.Generic;
using UnityEngine;

public class PaylineManager : MonoBehaviour
{
    [Header("Payline Config")]
    public PayLineDatabase paylineDatabase; 
    private Dictionary<int, PaylineData> paylineLookup; 

    private void Awake()
    {
        paylineLookup = new Dictionary<int, PaylineData>();
        foreach (var payline in paylineDatabase.paylines)
        {
            if (payline != null)
                paylineLookup[payline.lineNumber] = payline;
        }
    }
    
    public PaylineData GetPayline(int lineNumber)
    {
        paylineLookup.TryGetValue(lineNumber, out var payline);
        return payline;
    }

    public List<PaylineData> GetAllPaylines()
    {
        return paylineDatabase.paylines;
    }
    public SlotData GetSymbolOnPayline(SlotManagers[] allReels, int paylineNumber, int reelIndex)
    {
        if (!paylineLookup.ContainsKey(paylineNumber) || reelIndex < 0 || reelIndex >= allReels.Length)
            return null;

        var payline = GetPayline(paylineNumber);
        if (reelIndex >= payline.positions.Length)
            return null;

        int rowIndex = payline.positions[reelIndex];
        return allReels[reelIndex].Slots[rowIndex];
    }

    public bool CheckPaylineWin(SlotManagers[] allReels, int paylineNumber, out SlotRank winningRank, out int matchCount, out List<SlotData> winningSymbols)
    {
        winningSymbols = new List<SlotData>();
        matchCount = 0;
        winningRank = SlotRank.TEN;

        var payline = GetPayline(paylineNumber);
        if (payline == null || allReels.Length == 0)
            return false;

        var firstSlot = GetSymbolOnPayline(allReels, paylineNumber, 0);
        if (firstSlot?.SymbolData == null)
            return false;

        var firstRank = firstSlot.SymbolData.symbolType;

        if (firstRank == SlotRank.SCATTER || firstRank == SlotRank.BONUS)
            return false;

        winningRank = firstRank;
        winningSymbols.Add(firstSlot);
        matchCount = 1;

        for (int reel = 1; reel < allReels.Length; reel++)
        {
            var currentSlot = GetSymbolOnPayline(allReels, paylineNumber, reel);
            if (currentSlot?.SymbolData == null)
                break;

            var currentRank = currentSlot.SymbolData.symbolType;

            if (currentRank == firstRank || currentRank == SlotRank.WILD)
            {
                matchCount++;
                winningSymbols.Add(currentSlot);
            }
            else
            {
                break; 
            }
        }
        return matchCount >= 3;
    }
}