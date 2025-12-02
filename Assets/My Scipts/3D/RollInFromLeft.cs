using UnityEngine;
using DG.Tweening;

public class RollInFromLeft : MonoBehaviour
{
    [Header("Roll-In Settings")]
    public float startOffsetX = 10f;       // How far to the left it starts
    public float moveDuration = 1f;        // How long the roll-in takes
    public float rotationSpeed = 360f;     // Degrees per second (adjust for realism)

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Awake()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void OnEnable()
    {
        // Start from the left
        transform.position = targetPosition - new Vector3(startOffsetX, 0f, 0f);
        transform.rotation = targetRotation;

        // Movement tween
        transform.DOMoveX(targetPosition.x, moveDuration).SetEase(Ease.OutQuad);

        // Rotation tween (like a wheel rolling)
        float totalRotation = rotationSpeed * moveDuration;
        transform.DORotate(
            new Vector3(0f, 0f, totalRotation),
            moveDuration,
            RotateMode.FastBeyond360
        ).SetEase(Ease.Linear)
         .OnComplete(() =>
         {
             // Ensure it's clean at the end
             transform.rotation = targetRotation;
         });
    }

    void OnDisable()
    {
        // Kill tweens and reset transform
        transform.DOKill();
        transform.SetPositionAndRotation(targetPosition, targetRotation);
    }
}

