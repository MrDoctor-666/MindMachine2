using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class TempPanel : MonoBehaviour
{
    [SerializeField] float secondsOpen = 3f;
    float fadeSpeed = 0.05f;
    bool isOpened;
    Text text;
    CanvasGroup canvasGroup;

    void Awake()
    {
        EventAggregator.TempPanelOpened.Subscribe(OnTempPanelOpened);
        text = GetComponentInChildren<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        isOpened = false;
        //gameObject.SetActive(false);
    }

    public void OnTempPanelOpened(string panelText)
    {
        if (isOpened)
        {
            //StopCoroutine(PanelOpen());
            StopAllCoroutines();
            isOpened = false;
        }
        Debug.Log("Temp Panel Opened");
        isOpened = true;
        text.text = panelText;
        canvasGroup.alpha = 1;
        StartCoroutine(PanelOpen());

    }

    IEnumerator PanelOpen()
    {
        yield return new WaitForSeconds(secondsOpen - 1);
        while (canvasGroup.alpha > 0)
        {
            Debug.Log(canvasGroup.alpha);
            canvasGroup.alpha -= fadeSpeed;
            yield return new WaitForSeconds(fadeSpeed);
        }
        isOpened = false;
        canvasGroup.alpha = 0;
    }
}
