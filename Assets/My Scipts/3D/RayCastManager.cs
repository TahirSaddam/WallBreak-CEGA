using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class RayCastManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private float ingameDelay = 0.2f;

    private Camera playerCamera;

    private InputAction clickAction;
    private InputAction pKeyAction;

    private bool isReadyForNextClick = true;
    private void Start()
    {
        playerCamera = Camera.main;
    }



    private void Awake()
    {
        clickAction = new InputAction();
        clickAction.AddBinding("<Mouse>/leftButton")
            .WithInteractions("press()");
        clickAction.performed += OnMouseClick;


        pKeyAction = new InputAction();
        pKeyAction.AddBinding("<Keyboard>/p")
            .WithInteractions("press()");
        pKeyAction.performed += OnPKeyPressed;
    }

    private void OnEnable()
    {
        clickAction.Enable();
        pKeyAction.Enable();
    }

    private void OnDisable()
    {
        clickAction.Disable();
        pKeyAction.Disable();
    }

    private void OnMouseClick(CallbackContext ctx)
    {
        if (isReadyForNextClick)
        {
            if (playerCamera == null) playerCamera = Camera.main;
            Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.TryGetComponent(out IOption _Opt))
            {
                _Opt.OnClicked(hit.point);
            }
        }
    }

    private void OnPKeyPressed(CallbackContext context)
    {
        uiManager.PauseGame();
    }

    IEnumerator DelayForNextClick()
    {
        isReadyForNextClick = false;
        yield return new WaitForSeconds(ingameDelay);
        isReadyForNextClick = true;
    }

}
