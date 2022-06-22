using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickupDisplay : MonoBehaviour
{
    [SerializeField] private GameObject newItemPrefab;
    [SerializeField] private Transform container;

    private VerticalLayoutGroup _layoutGroup;
    private CanvasGroup _canvasGroup;

    private const float FadeOutDuration = 0.5f;

    public void Awake()
    {
        _layoutGroup = GetComponent<VerticalLayoutGroup>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowItemPickup(ItemPickupDictionary items, float duration)
    {
        StopAllCoroutines();
        _canvasGroup.alpha = 1f;

        foreach (Item item in items.Keys)
        {
            uint quantity = items[item];
            string text = quantity > 1 ? $"{quantity} {item.itemNamePlural}" : $"{item.itemName}";

            GameObject obj = Instantiate(newItemPrefab, container);
            obj.GetComponentInChildren<Image>().sprite = item.icon;
            obj.GetComponentInChildren<TMP_Text>().text = text;
        }

        // Forcibly update layout group
        Canvas.ForceUpdateCanvases();
        _layoutGroup.enabled = false;
        _layoutGroup.enabled = true;

        StartCoroutine(Hide(duration));
    }

    private IEnumerator Hide(float timeout)
    {
        yield return new WaitForSeconds(timeout);
        while (_canvasGroup.alpha > 0)
        {
            _canvasGroup.alpha -= Time.deltaTime / FadeOutDuration;
            yield return null;
        }

        for (int i = 0; i < container.childCount; ++i)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }
}
