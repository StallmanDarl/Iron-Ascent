using UnityEngine;

public class MetaUpgrade : MonoBehaviour
{
    public void ChooseHealthUpgrade()
    {
        UpgradeManager.EnsureExists();
        UpgradeManager.Instance.ShowMetaUpgradeChoices();
    }

    public void ChooseDamageUpgrade()
    {
        UpgradeManager.EnsureExists();
        UpgradeManager.Instance.ShowMetaUpgradeChoices();
    }
}
