using UnityEngine;
using DG.Tweening;
using TMPro;

public class FallAndBounce : MonoBehaviour
{
    [Space(20)]
    [Header("Animation Settings")]
    public float fallDistance = 5f;
    public float fallDuration = 0.5f;
    public float bounceHeight = 1f;
    public float bounceDuration = 0.3f;
    public int bounceCount = 1;

    [Space(15)]
    [Header("Impact Effect")]
    public Vector3 impactScale = new(1.1f, 0.9f, 1.1f);
    public float scaleDuration = 0.15f;

    private Vector3 startPosition;
    private Vector3 originalScale;

    void Awake()
    {
        startPosition = transform.position;
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        PlayFallAndBounce();
    }

    void PlayFallAndBounce()
    {
        // Fall down
        Vector3 fallTarget = startPosition - new Vector3(0, fallDistance, 0);
        transform.DOMoveY(fallTarget.y, fallDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                // Bounce with squash effect
                Sequence bounceSequence = DOTween.Sequence();

                for (int i = 0; i < bounceCount; i++)
                {
                    bounceSequence.Append(transform.DOScale(impactScale, scaleDuration).SetEase(Ease.OutQuad));
                    bounceSequence.Join(transform.DOMoveY(fallTarget.y + bounceHeight, bounceDuration).SetEase(Ease.OutQuad));
                    bounceSequence.Append(transform.DOScale(originalScale, scaleDuration).SetEase(Ease.InQuad));
                    bounceSequence.Join(transform.DOMoveY(fallTarget.y, bounceDuration).SetEase(Ease.InQuad));
                }

                bounceSequence.OnComplete(() =>
                {
                    // Ensure final scale is reset
                    transform.localScale = originalScale;
                });
            });
    }

    void OnDisable()
    {
        // Kill all tweens
        transform.DOKill();
        // Reset position and scale
        transform.position = startPosition;
        transform.localScale = originalScale;
    }
}
