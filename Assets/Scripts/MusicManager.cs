using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Single Tracks")]
    public AudioClip titleMusic;
    public AudioClip homeBaseMusic;
    public AudioClip metaUpgradeMusic;

    [Header("Arena Action Tracks")]
    public AudioClip[] arenaActionMusic;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        PlaySceneMusic(SceneManager.GetActiveScene().name);
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
        PlaySceneMusic(scene.name);
    }

    public void PlaySceneMusic(string sceneName)
    {
        AudioClip targetClip = null;

        switch (sceneName)
        {
            case "TitleScene":
                targetClip = titleMusic;
                break;

            case "HomeBase":
                targetClip = homeBaseMusic;
                break;

            case "MetaUpgradeScene":
                targetClip = metaUpgradeMusic;
                break;

            default:
                if (IsArenaScene(sceneName))
                {
                    targetClip = GetRandomArenaTrack();
                }
                break;
        }

        if (targetClip != null)
        {
            PlayTrack(targetClip);
        }
    }

    bool IsArenaScene(string sceneName)
    {
        return sceneName.Contains("Arena");
    }

    AudioClip GetRandomArenaTrack()
    {
        if (arenaActionMusic == null || arenaActionMusic.Length == 0)
            return null;

        return arenaActionMusic[
            Random.Range(0, arenaActionMusic.Length)
        ];
    }

    void PlayTrack(AudioClip clip)
    {
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}