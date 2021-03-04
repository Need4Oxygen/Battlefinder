using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TJson.Serialization;

public class Audio : MonoBehaviour
{
    public static Audio Instance { get; private set; }
    public static AudioSettings Settings = null;

    public class AudioSettings
    {
        public float master = 0.5f;
        public float music = 0.5f;
        public float ambient = 0.5f;
        public float effects = 0.5f;
    }

    [Serializable]
    public struct Wrapper
    {
        public string key;
        public AudioClip clip;
    }

    [SerializeField] private AudioSource music = null;
    [SerializeField] private AudioSource ambient = null;
    [SerializeField] private AudioSource effects = null;

    [SerializeField] private List<Wrapper> musicList = new List<Wrapper>();
    [SerializeField] private List<Wrapper> ambientList = new List<Wrapper>();
    [SerializeField] private List<Wrapper> effectsList = new List<Wrapper>();

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(transform);
            Instance = this;
        }
        else
            Destroy(transform.gameObject);
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("AudioSettings"))
        {
            string str = PlayerPrefs.GetString("AudioSettings");
            Settings = Deserialize<AudioSettings>(str);
        }
        else
        {
            Settings = new AudioSettings();
            PlayerPrefs.SetString("AudioSettings", TJson.Serialization.Serialize(Settings));
        }

        UpdateAllAssVolume(false);
    }

    //-------------------------------------VOLUMES-------------------------------------

    public void SetVolume_Master(float volume)
    {
        Settings.master = Mathf.Clamp01(volume);
        UpdateAllAssVolume(true);
    }
    public void SetVolume_Music(float volume)
    {
        Settings.music = Mathf.Clamp01(volume);
        music.volume = Settings.master * Settings.music;
        SaveVolumes();
    }
    public void SetVolume_Ambient(float volume)
    {
        Settings.ambient = Mathf.Clamp01(volume);
        ambient.volume = Settings.master * Settings.ambient;
        SaveVolumes();
    }
    public void SetVolume_Effects(float volume)
    {
        Settings.effects = Mathf.Clamp01(volume);
        effects.volume = Settings.master * Settings.effects;
        SaveVolumes();
    }

    private void UpdateAllAssVolume(bool save)
    {
        music.volume = Settings.master * Settings.music;
        ambient.volume = Settings.master * Settings.ambient;
        effects.volume = Settings.master * Settings.effects;
        if (save)
            SaveVolumes();
    }

    private void SaveVolumes()
    {
        PlayerPrefs.SetString("AudioSettings", Serialize(Settings));
    }


    //-------------------------------------MUSIC-------------------------------------

    private Coroutine playMusicThread = null;

    public void Play_Music(string key, float delay, float blend)
    {
        AudioClip clip = null;
        clip = musicList.Find(ctx => ctx.key == key).clip;

        if (clip != null)
            if (playMusicThread != null) // If already changing music
            {
                StopCoroutine(playMusicThread);
                playMusicThread = null;
            }

        playMusicThread = StartCoroutine(PlayMusicCorou(clip, delay, blend));
    }

    private IEnumerator PlayMusicCorou(AudioClip clip, float delay, float blend)
    {
        yield return new WaitForSecondsRealtime(delay);
        float timer = 0;

        // Blend out previous music
        if (music.isPlaying)
        {
            while (timer < blend / 2)
            {
                music.volume = Settings.master * Settings.music * (1 - (timer / (blend / 2)));
                timer += Time.deltaTime;
                Debug.Log(timer);
                yield return null;
            }
            music.Stop();
        }

        timer = 0;
        music.clip = clip;

        // Blend in new music
        music.Play();
        while (timer < blend / 2)
        {
            music.volume = Settings.master * Settings.music * (timer / (blend / 2));
            timer += Time.deltaTime;
            Debug.Log(timer);
            yield return null;
        }

        playMusicThread = null;
    }

    private Coroutine stopMusicThread = null;

    public void Stop_Music(float delay, float blend)
    {
        if (stopMusicThread != null) // If already stoping music
        {
            StopCoroutine(stopMusicThread);
            stopMusicThread = null;
        }

        stopMusicThread = StartCoroutine(StopMusicCorou(delay, blend));
    }

    private IEnumerator StopMusicCorou(float delay, float blend)
    {
        yield return new WaitForSecondsRealtime(delay);

        float timer = 0;

        // Blend out
        if (music.isPlaying)
        {
            while (timer < blend / 2)
            {
                music.volume = Settings.master * Settings.music * (1 - (timer / (blend / 2)));
                timer += Time.deltaTime;
                yield return null;
            }
            music.Stop();
        }

        music.clip = null;
        stopMusicThread = null;
    }


    //-------------------------------------AMBIENT-------------------------------------

    Coroutine playAmbientThread = null;

    public void Play_Ambient(string key, float delay, float blend)
    {
        AudioClip clip = null;
        clip = ambientList.Find(ctx => ctx.key == key).clip;

        if (clip != null)
            if (playAmbientThread != null) // If already changing ambient
            {
                StopCoroutine(playAmbientThread);
                playAmbientThread = null;
            }

        playAmbientThread = StartCoroutine(PlayAmbientCorou(clip, delay, blend));
    }

    IEnumerator PlayAmbientCorou(AudioClip clip, float delay, float blend)
    {
        yield return new WaitForSecondsRealtime(delay);

        float timer = 0;

        // Blend out previous ambient
        if (ambient.isPlaying)
        {
            while (timer < blend / 2)
            {
                ambient.volume = Settings.master * Settings.ambient * (1 - (timer / (blend / 2)));
                timer += Time.deltaTime;
                yield return null;
            }
            ambient.Stop();
        }

        timer = 0;
        ambient.clip = clip;

        // Blend in new ambient
        ambient.Play();
        while (timer < blend / 2)
        {
            ambient.volume = Settings.master * Settings.ambient * (timer / (blend / 2));
            timer += Time.deltaTime;
            yield return null;
        }

        playAmbientThread = null;
    }

    Coroutine stopAmbientThread = null;

    public void Stop_Ambient(float delay, float blend)
    {
        if (stopAmbientThread != null) // If already stoping ambient
        {
            StopCoroutine(stopAmbientThread);
            stopAmbientThread = null;
        }

        stopAmbientThread = StartCoroutine(StopMusicCorou(delay, blend));
    }

    IEnumerator StopAmbientCorou(float delay, float blend)
    {
        yield return new WaitForSecondsRealtime(delay);

        float timer = 0;

        // Blend out
        if (ambient.isPlaying)
        {
            while (timer < blend / 2)
            {
                ambient.volume = Settings.master * Settings.ambient * (1 - (timer / (blend / 2)));
                timer += Time.deltaTime;
                yield return null;
            }
            ambient.Stop();
        }

        ambient.clip = null;
        stopAmbientThread = null;
    }


    //-------------------------------------AMBIENT-------------------------------------

    public void Play_Effect(string key, float delay)
    {
        AudioClip clip = null;
        clip = effectsList.Find(ctx => ctx.key == key).clip;

        if (clip != null)
            StartCoroutine(PlayEffectCorou(clip, delay));
    }

    IEnumerator PlayEffectCorou(AudioClip clip, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        effects.PlayOneShot(clip);
    }

}
