using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour
{

    [Header("Loading References ")]
    [SerializeField] private Image gameLogo;
    [SerializeField] private Image fillImage;
    [SerializeField] private GameHUD gameHUD;

    [SerializeField] private RectTransform loadingPanelPos;

    [Header("Animation Settings ")]
    [SerializeField] private float logoFadeIn = 0.5f;
    [SerializeField] private float logoFadeOut = 2f;
    [SerializeField] private float loadingDuration = 5f;
    private Coroutine loadingRoutine;

    private void Start()
    {
        ResetAll();
        ShowSequence();
    }

    private void ResetAll()
    {
        gameLogo.gameObject.SetActive(true);
        gameHUD.HideInGamePanel();
        gameHUD.HideLoadingPanel();

        fillImage.fillAmount = 0f;
    }

    private void ShowSequence()
    {
        gameLogo?.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Append(gameLogo.DOFade(1f, logoFadeIn));
        seq.Append(gameLogo.DOFade(0f, logoFadeOut));
        seq.AppendCallback(() => gameLogo.gameObject.SetActive(false));

        seq.AppendCallback(() =>
        {
            gameHUD.PlaySnowfall();
            gameHUD.ShowLoadingPanel();
        });

        seq.OnComplete(() =>
        {
            var target = loadingPanelPos;
            target.DOScale(Vector3.one * 1.05f, 1.2f)
                  .SetEase(Ease.InOutSine)
                  .SetLoops(-1, LoopType.Yoyo);


            if (loadingRoutine != null)
                StopCoroutine(loadingRoutine);

            loadingRoutine = StartCoroutine(InitLoading());
        });
    }

    private IEnumerator InitLoading()
    {
        float timeLeft = loadingDuration;

        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            SetFillValue(timeLeft);
            yield return null;
        }
        
        SetFillValue(0f);
        gameHUD.StopSnowfall();
        gameHUD.ShowInGamePanel();
    }

    private void SetFillValue(float value)
    {
        fillImage.fillAmount = Mathf.InverseLerp(loadingDuration, 0f, value);
    }
}
