using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaUpgrade : MonoBehaviour
{
    public void ChooseHealthUpgrade()
    {
        GameManager.Instance.ApplyMetaUpgrade("Health");
        FinishUpgrade();
    }

    public void ChooseDamageUpgrade()
    {
        GameManager.Instance.ApplyMetaUpgrade("Damage");
        FinishUpgrade();
    }

    void FinishUpgrade()
    {
        RunManager.Instance.ResetRunProgress();
        SceneManager.LoadScene("Arena_01");
    }
}