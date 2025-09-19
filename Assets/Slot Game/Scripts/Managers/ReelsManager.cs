using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelsManager : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private SymbolDatabase symbolDatabase;

    [Header("Animation Settings")]
    [SerializeField] private float spinDuration = 3f;
    [SerializeField] private float reelStopDelay = 0.3f;
    [SerializeField] private float symbolSpacing = 150f;
    [SerializeField] private int spinCycles = 20;

    [Header("Easing")]
    [SerializeField] private Ease slowEase = Ease.OutQuad;
    [SerializeField] private Ease stopEase = Ease.OutBack;

    [Header("Reels")]
    public SlotManagers[] SlotManagers;

    private bool isSpinning;
    private int reelsSpinning;
    private readonly List<Sequence> activeSequences = new List<Sequence>();

    private Dictionary<SlotData, Vector2> SlotPositions = new();

    private List<SymbolData> highSymbols = new();
    private List<SymbolData> lowSymbols = new();
    private List<SymbolData> specialSymbols = new();

    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        InitializeSymbols();
        StoreSlotPositions();
    }

    private void OnDestroy()
    {
        StopAllSlotSequences();
    }


    #region  Pre Loading Assets
    private void InitializeSymbols()
    {
        highSymbols.Clear();
        lowSymbols.Clear();
        specialSymbols.Clear();

        foreach (var s in symbolDatabase.symbols)
        {
            switch (s.category)
            {
                case SymbolCategory.High:
                    highSymbols.Add(s);
                    break;
                case SymbolCategory.Low:
                    lowSymbols.Add(s);
                    break;
                case SymbolCategory.Special:
                    specialSymbols.Add(s);
                    break;
            }
        }
        AssignRandomSymbols();
    }
    private void AssignRandomSymbols()
    {
        foreach (var reel in SlotManagers)
        {
            foreach (var slot in reel.Slots)
            {
                SymbolData symbol = RandomSymbol();
                slot.SetSlotData(symbol.symbolSprite, symbol);
            }
        }
    }
    private void StoreSlotPositions()
    {
        SlotPositions.Clear();
        foreach (var reel in SlotManagers)
            foreach (var slot in reel.Slots)
                SlotPositions[slot] = slot.GetComponent<RectTransform>().anchoredPosition;
    }

    #endregion

    #region Spin Logic Animation and Set Slot Datas
    public void StartSpin()
    {
        if (isSpinning)
            return;

        isSpinning = true;
        reelsSpinning = SlotManagers.Length;

        StopAllSlotSequences();

        for (int i = 0; i < SlotManagers.Length; i++)
        {
            float delay = i * 0.15f;
            StartCoroutine(SpinReelWithDelay(i, delay));
        }
    }

    private IEnumerator SpinReelWithDelay(int reelIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpinReel(SlotManagers[reelIndex], reelIndex);
    }

    private void SpinReel(SlotManagers reel, int reelIndex)
    {
        float totalTime = spinDuration + reelIndex * reelStopDelay;

        Sequence seq = DOTween.Sequence();

        int fastCycles = Mathf.FloorToInt(spinCycles * 0.8f);
        AppendCycles(seq, reel, fastCycles, totalTime * 0.8f / fastCycles);

        int slowCycles = Mathf.FloorToInt(spinCycles * 0.15f);
        AppendCycles(seq, reel, slowCycles, totalTime * 0.15f / slowCycles, slowEase);

        var finalSymbols = GetFinalSymbols(reel);
        seq.AppendCallback(() => ApplyFinalSymbols(reel, finalSymbols));
        seq.Append(FinalLandingAnimation(reel, totalTime * 0.05f));

        seq.OnComplete(() => FinalResult());
        activeSequences.Add(seq);
    }

    private void AppendCycles(Sequence seq, SlotManagers reel, int count, float cycleTime, Ease ease = Ease.Linear)
    {
        for (int i = 0; i < count; i++)
        {
            seq.Append(MoveReel(reel, cycleTime * 0.7f, ease));
            seq.AppendCallback(() => ResetAndRandomizeSymbols(reel));
            seq.AppendInterval(cycleTime * 0.3f);
        }
    }

    private Tween MoveReel(SlotManagers reel, float duration, Ease ease)
    {
        Sequence s = DOTween.Sequence();
        foreach (var slot in reel.Slots)
        {
            var rect = slot.GetComponent<RectTransform>();
            Vector2 target = rect.anchoredPosition - new Vector2(0, symbolSpacing);
            s.Join(rect.DOAnchorPos(target, duration).SetEase(ease));
        }
        return s;
    }

    private void ResetAndRandomizeSymbols(SlotManagers reel)
    {
        foreach (var slot in reel.Slots)
        {
            var rect = slot.GetComponent<RectTransform>();
            rect.anchoredPosition = SlotPositions[slot];

            SymbolData randomSymbol = RandomSymbol();
            slot.SetSlotData(randomSymbol.symbolSprite, randomSymbol);
        }
    }
    private Tween FinalLandingAnimation(SlotManagers reel, float duration)
    {
        Sequence landing = DOTween.Sequence();
        for (int i = 0; i < reel.Slots.Count; i++)
        {
            var slot = reel.Slots[i];
            var rect = slot.GetComponent<RectTransform>();
            Vector2 start = SlotPositions[slot] + new Vector2(0, 20f);
            rect.anchoredPosition = start;

            landing.Join(rect.DOAnchorPos(SlotPositions[slot], duration)
                .SetEase(stopEase)
                .SetDelay(i * 0.03f));
        }
        return landing;
    }
    #endregion

    #region Symbol Helpers
    private SymbolData RandomSymbol()
    {
        float random = Random.value;

        if (random < 0.05f)
            return specialSymbols[Random.Range(0, specialSymbols.Count)];
        else if (random < 0.35f)
            return highSymbols[Random.Range(0, highSymbols.Count)];
        else
            return lowSymbols[Random.Range(0, lowSymbols.Count)];
    }

    private List<SymbolData> GetFinalSymbols(SlotManagers reel)
    {
        List<SymbolData> result = new();
        foreach (var _ in reel.Slots)
        {
            float r = Random.value;
            if (r < 0.6f)
                result.Add(lowSymbols[Random.Range(0, lowSymbols.Count)]);

            else if (r < 0.85f)
                result.Add(highSymbols[Random.Range(0, highSymbols.Count)]);

            else
                result.Add(specialSymbols[Random.Range(0, specialSymbols.Count)]);
        }
        return result;
    }

    private void ApplyFinalSymbols(SlotManagers reel, List<SymbolData> symbols)
    {
        for (int i = 0; i < symbols.Count && i < reel.Slots.Count; i++)
        {
            reel.Slots[i].SetSlotData(symbols[i].symbolSprite, symbols[i]);
        }
    }

    #endregion

    #region Reel Completion
    private void FinalResult()
    {
        reelsSpinning--;
        if (reelsSpinning <= 0)
        {
            isSpinning = false;
            Debug.Log("Checking for wins...");
            AudioController.Instance.PlayReelStops();
            gameManager.CheckForWins(SlotManagers);
        }
    }
    public void StopAllReels()
    {
        if (!isSpinning) return;
        StopAllSlotSequences();
        isSpinning = false;
        reelsSpinning = 0;
    }

    private void StopAllSlotSequences()
    {
        foreach (var seq in activeSequences)
            seq.Kill();
        activeSequences.Clear();
    }

    #endregion
}
