using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyboxManager : MonoBehaviour
{
    public static SkyboxManager Instance;

    public Material defaultSkybox;
    public Material[] metaSkyboxes;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySkybox();
    }

    public void ApplySkybox()
    {
        if (RunManager.Instance == null)
        {
            RenderSettings.skybox = defaultSkybox;
            return;
        }

        int tier = RunManager.Instance.metaTier;

        if (tier <= 0 || metaSkyboxes.Length == 0)
        {
            RenderSettings.skybox = defaultSkybox;
        }
        else
        {
            int index = Mathf.Clamp(tier - 1, 0, metaSkyboxes.Length - 1);
            RenderSettings.skybox = metaSkyboxes[index];
        }

        DynamicGI.UpdateEnvironment(); // refresh lighting
    }
}