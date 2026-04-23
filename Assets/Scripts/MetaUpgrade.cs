using UnityEngine;

public class MetaUpgrade : MonoBehaviour
{
    public void ChooseHealthUpgrade()
    {
        GameManager.Instance.ApplyMetaUpgrade("Health");
        RunManager.Instance.MetaUpgradeCollected();
    }

    public void ChooseDamageUpgrade()
    {
        GameManager.Instance.ApplyMetaUpgrade("Damage");
        RunManager.Instance.MetaUpgradeCollected();
    }
}
