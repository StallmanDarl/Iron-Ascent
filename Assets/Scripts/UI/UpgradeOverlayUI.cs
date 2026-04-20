using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeOverlayUI : MonoBehaviour
{
    [SerializeField] TMP_Text headingText;
    [SerializeField] TMP_Text subtitleText;
    [SerializeField] UpgradeCardSlot[] cardSlots;

    void Awake()
    {
        //CacheReferences();
    }

    // void CacheReferences()
    // {
    //     headingText = transform.Find("Panel/HeadingText")?.GetComponent<TMP_Text>();
    //     subtitleText = transform.Find("Panel/SubtitleText")?.GetComponent<TMP_Text>();
    //     cardSlots = transform.Find("Panel/CardsRow")?.GetComponentsInChildren<UpgradeCardSlot>(true);
    // }

    public void Show(
        string heading,
        string subtitle,
        List<UpgradeManager.UpgradeChoice> choices,
        Action<UpgradeManager.UpgradeChoice> onSelected)
    {

        if (headingText == null || subtitleText == null || cardSlots == null)
        {
            Debug.LogWarning("Rebinding UI references...");
            //CacheReferences();
        }

        // If STILL null -> real hierarchy issue
        if (headingText == null || subtitleText == null || cardSlots == null)
        {
            Debug.LogError("UI references missing after rebinding!");
            return;
        }

        gameObject.SetActive(true);

        headingText.text = heading;
        subtitleText.text = subtitle;

        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (i < choices.Count)
            {
                cardSlots[i].gameObject.SetActive(true);
                cardSlots[i].Bind(choices[i], onSelected);
            }
            else
            {
                cardSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void HideInstant()
    {
        Hide();
    }
}
