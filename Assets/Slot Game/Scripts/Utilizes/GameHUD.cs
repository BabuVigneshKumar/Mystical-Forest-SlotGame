using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [Header(" In Game Panels")]
    public GameObject WelcomePanel;
    public GameObject InfoPanel;

    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject SplashPanel;
    [SerializeField] private GameObject ResultPanel;


    [Space(5)]
    [Header("Info Panels")]
    public RectTransform[] InfoPanels;

    [Space(5)]
    [Header("Buttons")]
    public Button SpinBtn;
    public Button stopBtn;
    public Button PlusBtn;
    public Button MinusBtn;
    public Button PreviousBtn;
    public Button InfoBtn;
    public Button NextBtn;
    public Button CloseBtn;
    public Button ContinueBtn;

    [Space(5)]
    [Header("Texts")]
    [SerializeField] private TMP_Text playerBalanceTxt;
    [SerializeField] private TMP_Text winAmountTxt;
    [SerializeField] private TMP_Text betAmountTxt;

    [Space(5)]
    [Header("Images")]
    [SerializeField] private Image fillImage;

    [Space(5)]
    [Header("Particles")]
    [SerializeField] private ParticleSystem snowfallEfx;

    [Header("Toggles")]
    [SerializeField] public Toggle WelcomeToggle;

    public void SetPlayerBalance(float value) => playerBalanceTxt.text = $"$ {value.ToString()}";
    public void SetWinAmount(float value) => winAmountTxt.text = $"$ {value.ToString()}";
    public void SetBetAmount(float value) => betAmountTxt.text = $"$ {value.ToString("F2")}";

    public void ShowInGamePanel()
    {
        inGamePanel.SetActive(true);
        SplashPanel.SetActive(false);
    }
    public void HideInGamePanel()
    {
        inGamePanel.SetActive(false);
        SplashPanel.SetActive(true);
    }

    public void ShowResult() => ResultPanel.SetActive(true);
    public void HideResult() => ResultPanel.SetActive(false);

    //Loading Section
    public void ShowLoadingPanel() => loadingPanel.SetActive(true);
    public void HideLoadingPanel() => loadingPanel.SetActive(false);

    //Particles Section
    public void PlaySnowfall()
    {
        snowfallEfx.gameObject.SetActive(true);
        snowfallEfx.Play();
    }
    public void StopSnowfall()
    {
        snowfallEfx.gameObject.SetActive(false);
        snowfallEfx.Stop();
    }



}
