using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultHandler : MonoBehaviour
{
    [Header("Result Variables ************* ")]
    [SerializeField] private List<Sprite> Numbers;
    [SerializeField] private Image[] WinAmountImages;
    [SerializeField] private GameObject Popup;

    [Header("Script Reference ************ ")]
    [SerializeField] private GameManager _GameManager;

    private void OnEnable()
    {
        _GameManager.WinAmountAction += ShowWinAmount;
        OnShowMe();
    }

    private void OnShowMe()
    {
        Popup.transform.localScale = new Vector3(1, 0, 1);
        Popup.transform?.DOScaleY(1, 0.5f).SetEase(Ease.InOutExpo);
    }
    private void ShowWinAmount(float amount)
    {
        Debug.Log($"Win amount is : {amount}");
        string amountString = amount.ToString("F2");
        foreach (Image image in WinAmountImages)
        {
            image.gameObject.SetActive(false);
        }

        for (int i = 0; i < amountString.Length; i++)
        {
            char c = amountString[i];
            WinAmountImages[i].sprite = c == '.' ? Numbers.Find(x => x.name == "Dot") : Numbers.Find(x => x.name == c.ToString());
            WinAmountImages[i].gameObject.SetActive(true);
        }
    }
}
