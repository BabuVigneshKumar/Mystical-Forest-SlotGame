using UnityEngine;
using UnityEngine.UI;

public class SlotData : MonoBehaviour
{
    [SerializeField] private Image SlotImage;
    private RectTransform rectTransform;

    // Add reference to symbol data
    [SerializeField] private SymbolData symbolData;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetSlotData(Sprite sprite, SymbolData symbolData = null)
    {
        SlotImage.sprite = sprite;
        this.symbolData = symbolData;
    }
    public SymbolData SymbolData => symbolData;

    public SlotRank GetRank()
    {
        return symbolData?.symbolType ?? SlotRank.TEN;
    }

    public RectTransform GetRectTransform()
    {
        return rectTransform;
    }
}