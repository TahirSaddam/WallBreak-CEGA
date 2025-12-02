using UnityEngine;
using DG.Tweening;

public class RiseAndBounce : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("How far below the target position the object starts (in units).")]
    public float riseDistance = 5f;
    public float riseDuration = 0.5f;
    public float bounceHeight = 1f;
    public float bounceDuration = 0.3f;
    [Tooltip("How many full up-and-down bounces after the initial rise.")]
    public int bounceCount = 1;

    [Header("Impact Effect")]
    [Tooltip("Scale to squash to on impact; X/Z wider, Y shorter.")]
    public Vector3 impactScale = new Vector3(1.1f, 0.9f, 1.1f);
    public float scaleDuration = 0.15f;

    Vector3 targetPosition;   // where the object finally rests (its “true” position)
    Vector3 originalScale;    // cached starting scale

    void Awake()
    {
        targetPosition = transform.position;
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        // Put the object below its true spot to start the rise
        transform.position = targetPosition - new Vector3(0, riseDistance, 0);

        PlayRiseAndBounce();
    }

    void PlayRiseAndBounce()
    {
        transform.DOMoveY(targetPosition.y, riseDuration)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() =>
                 {
                     Sequence bounceSeq = DOTween.Sequence();

                     for (int i = 0; i < bounceCount; i++)
                     {
                         // Squash as it pushes off
                         bounceSeq.Append(transform.DOScale(impactScale, scaleDuration).SetEase(Ease.OutQuad));
                         // Move up
                         bounceSeq.Join(transform.DOMoveY(targetPosition.y + bounceHeight, bounceDuration).SetEase(Ease.OutQuad));

                         // Stretch back to normal while falling
                         bounceSeq.Append(transform.DOScale(originalScale, scaleDuration).SetEase(Ease.InQuad));
                         bounceSeq.Join(transform.DOMoveY(targetPosition.y, bounceDuration).SetEase(Ease.InQuad));
                     }

                     bounceSeq.OnComplete(() => transform.localScale = originalScale);
                 });
    }

    void OnDisable()
    {
        transform.DOKill();                 // stop any tweens still running
        transform.position = targetPosition; // snap to true spot
        transform.localScale = originalScale;
    }
}
