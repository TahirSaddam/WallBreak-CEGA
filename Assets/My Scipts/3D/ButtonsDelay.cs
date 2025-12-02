using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsDelay : MonoBehaviour
{
    public Button[] buttons;
    public float delay = 1f;

    private void OnEnable()
    {
        foreach (Button b in buttons)
        {
            b.onClick.AddListener(() => StartCoroutine(DisableButtons()));
        }

    }

    private void OnDisable()
    {
        foreach (Button b in buttons)
        {
            b.onClick.RemoveListener(() => StartCoroutine(DisableButtons()));
        }
    }

    IEnumerator DisableButtons()
    {
        foreach (Button b in buttons)
        {
            b.interactable = false;
        }

        yield return new WaitForSeconds(delay);

        foreach (Button b in buttons)
        {
            b.interactable = true;
        }
    }
}
