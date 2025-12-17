
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject visualsRoot;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardMatchableImage;
    [SerializeField] private Button cardButton;

    [Header("Timings")]
    [SerializeField, Min(0.02f)] private float flipHalfDuration = 0.08f;

    private Color32 _cardUpColor;
    private Color32 _cardDownColor;
    private Color32 _matchableColor;
    private Coroutine _flipRoutine;



    //------------- Public API's -----------
    public void Config(bool matched, Color32 cardUpColor, Color32 cardDownColor, Color32 matchableColor, float initialRevealDuratoin, Action onCardClicked)
    {
        if (matched)
        {
            DisableVisuals();
            return;
        }
        _cardUpColor = cardUpColor;
        _cardDownColor = cardDownColor;
        _matchableColor = matchableColor;

        cardButton.onClick.RemoveAllListeners();
        if (onCardClicked != null)
            cardButton.onClick.AddListener(() => onCardClicked.Invoke());

        SetCardUpVisual();
        SetInteractable(false);
        StartCoroutine(AutoHideAfterDelay(initialRevealDuratoin));
    }

    public void PlayFlipUpAnimation(Action onComplete = null)
    {
        SetInteractable(false);
        StopFlip();
        _flipRoutine = StartCoroutine(FlipRoutine(() =>
        {
            SetCardUpVisual();
        }, () =>
        {
            onComplete?.Invoke();
            _flipRoutine = null;
        }));
    }

    public void PlayFlipDownAnimation(Action onComplete = null)
    {
        StopFlip();
        _flipRoutine = StartCoroutine(FlipRoutine(() =>
        {
            SetCardDownVisual();
        }, () =>
        {
            SetInteractable(true);
            onComplete?.Invoke();
            _flipRoutine = null;
        }));
    }

    public void PlayMatchAnimation()
    {
        // to do : matching aniation
        DisableVisuals();
    }


    //------------- Private methodes -----------
    private IEnumerator AutoHideAfterDelay(float autoHideDelay, Action onFlipDownComplete = null)
    {
        if (autoHideDelay > 0f)
            yield return new WaitForSeconds(autoHideDelay);

        PlayFlipDownAnimation(onFlipDownComplete);
    }
    private IEnumerator FlipRoutine(Action onMidFlip, Action onComplete)
    {
        // Phase 1: scale X from 1 -> 0
        yield return ScaleX(1f, 0f, flipHalfDuration);

        // Mid flip: swap visuals at "thin" moment
        onMidFlip?.Invoke();

        // Phase 2: scale X from 0 -> 1
        yield return ScaleX(0f, 1f, flipHalfDuration);

        _flipRoutine = null;
        onComplete?.Invoke();
    }
    private IEnumerator ScaleX(float from, float to, float duration)
    {
        if (duration <= 0f)
        {
            SetScaleX(to);
            yield break;
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / duration);
            float x = Mathf.Lerp(from, to, a);
            SetScaleX(x);
            yield return null;
        }

        SetScaleX(to);
    }
    private void SetScaleX(float x)
    {
        var s = transform.localScale;
        s.x = x;
        transform.localScale = s;
    }
    private void SetCardUpVisual()
    {
        cardImage.color = _cardUpColor;
        cardMatchableImage.color = _matchableColor;
        cardMatchableImage.enabled = true;
    }
    private void SetCardDownVisual()
    {
        cardImage.color = _cardDownColor;
        cardMatchableImage.enabled = false;
    }
    private void SetInteractable(bool interactable)
    {
        cardButton.interactable = interactable;
    }
    private void StopFlip()
    {
        if (_flipRoutine != null)
        {
            StopCoroutine(_flipRoutine);
            _flipRoutine = null;
        }
    }
    private void DisableVisuals()
    {
        visualsRoot.SetActive(false);
    }
}
