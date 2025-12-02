using UnityEngine;
using DG.Tweening;

public class RippleScaleEffect : MonoBehaviour
{
    [Header("Ripple Targets")]
    public Transform[] objects;

    [Header("Effect Settings")]
    public float scaleDuration = 0.3f;
    public float rippleDelay = 0.1f;
    public float loopDelay = 1.5f;          // Time to wait before repeating the ripple
    public Ease easing = Ease.OutBack;

    private Vector3 originalScale = Vector3.one;
    private Sequence rippleSequence;

    void OnEnable()
    {
        PlayRippleLoop();
    }

    void PlayRippleLoop()
    {
        rippleSequence = DOTween.Sequence();

        for (int i = 0; i < objects.Length; i++)
        {
            Transform t = objects[i];
            t.localScale = Vector3.zero;

            rippleSequence.Insert(i * rippleDelay, t.DOScale(originalScale, scaleDuration).SetEase(easing));
        }

        rippleSequence.AppendInterval(loopDelay);
        rippleSequence.SetLoops(-1, LoopType.Restart);
    }

    void OnDisable()
    {
        if (rippleSequence != null && rippleSequence.IsActive())
        {
            rippleSequence.Kill();
        }

        foreach (var t in objects)
        {
            if (t != null) t.localScale = originalScale;
        }
    }
}
