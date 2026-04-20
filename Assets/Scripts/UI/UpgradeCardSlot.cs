using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UpgradeCardSlot : MonoBehaviour
{
    [SerializeField] TMP_Text categoryText;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] Image iconImage;
    [SerializeField] Button selectButton;

    void Awake()
    {
        // CacheReferences();
    }

    // void CacheReferences()
    // {
    //     categoryText = transform.Find("CategoryText").GetComponent<TMP_Text>();
    //     titleText = transform.Find("TitleText").GetComponent<TMP_Text>();
    //     descriptionText = transform.Find("DescriptionText").GetComponent<TMP_Text>();
    //     iconImage = transform.Find("Icon").GetComponent<Image>();
    //     selectButton = transform.Find("SelectButton").GetComponent<Button>();
    //     buttonText = transform.Find("SelectButton/ButtonText").GetComponent<TMP_Text>();
    // }

    public void Bind(UpgradeManager.UpgradeChoice choice, System.Action<UpgradeManager.UpgradeChoice> onSelected)
    {
        // Re-cache if needed
        if (categoryText == null || titleText == null || descriptionText == null || selectButton == null)
        {
            Debug.LogWarning("Rebinding UpgradeCardSlot UI...");
            //CacheReferences();
        }

        // If still broken -> real hierarchy issue
        if (categoryText == null || titleText == null || descriptionText == null || selectButton == null)
        {
            Debug.LogError("UpgradeCardSlot references missing!");
            return;
        }

        gameObject.SetActive(true);

        categoryText.text = choice.category;
        titleText.text = choice.title;
        descriptionText.text = choice.description;

        if (buttonText != null)
        {
            buttonText.text = "Take Upgrade";
        }

        if (iconImage != null)
        {
            iconImage.sprite = choice.icon;
            iconImage.enabled = choice.icon != null;
        }

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => onSelected?.Invoke(choice));
    }
}
