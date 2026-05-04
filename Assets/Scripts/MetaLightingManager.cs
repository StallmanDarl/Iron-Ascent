using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaLightingManager : MonoBehaviour
{
    public static MetaLightingManager Instance;

    [Header("Skyboxes")]
    public Material[] metaSkyboxes;

    [Header("Directional Light")]
    public Light directionalLight;

    [Header("Tier Light Colors")]
    public Color[] lightColors;

    [Header("Tier Intensities")]
    public float[] lightIntensities;

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

    void Start()
    {
        ApplyLighting();
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindDirectionalLight();
        ApplyLighting();
    }

    void FindDirectionalLight()
    {
        Light[] lights = FindObjectsOfType<Light>();

        foreach (Light l in lights)
        {
            if (l.type == LightType.Directional)
            {
                directionalLight = l;
                return;
            }
        }
    }

    public void ApplyLighting()
    {
        if (RunManager.Instance == null) return;

        int tier = Mathf.Clamp(
            RunManager.Instance.metaTier,
            0,
            metaSkyboxes.Length - 1
        );

        // Apply skybox
        if (metaSkyboxes[tier] != null)
        {
            RenderSettings.skybox = metaSkyboxes[tier];
        }

        // Apply directional light
        if (directionalLight != null)
        {
            directionalLight.color = lightColors[tier];
            directionalLight.intensity = lightIntensities[tier];
        }

        DynamicGI.UpdateEnvironment();
    }
}