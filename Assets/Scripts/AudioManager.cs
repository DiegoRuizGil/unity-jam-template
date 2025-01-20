using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
    ButtonClick
}

[System.Serializable]
public class IDSound
{
    public SFX type;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    [Header("Audios")]
    [SerializeField] private List<IDSound> _sfxSounds;

    [Header("Audio Pool")]
    [SerializeField] private int _sfxSourceAmount;
    [SerializeField] private AudioSource _sfxSourcePrefab;
    [SerializeField] private Transform _sfxPoolParent;

    private List<AudioSource> _sfxSources;
    
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        InitializePools();
    }

    private void InitializePools()
    {
        _sfxSources = new List<AudioSource>();
        for (int i = 0; i < _sfxSourceAmount; i++)
        {
            var audioSource = Instantiate(_sfxSourcePrefab, _sfxPoolParent);
            _sfxSources.Add(audioSource);
        }
    }

    #region SFX
    public void PlaySFX(SFX type, float volume = 1f)
    {
        AudioSource audioSource = FindSFXSourceAvailable();
        if (!audioSource) return;

        AudioClip clip = GetSFXByType(type);
        if (clip == null) return;

        audioSource.clip = clip;
        audioSource.volume = volume;
    }

    private AudioSource FindSFXSourceAvailable()
    {
        return _sfxSources.Find(source => !source.isPlaying);
    }

    private AudioClip GetSFXByType(SFX type)
    {
        return _sfxSounds.Find(sound => sound.type == type)?.clip;
    }
    #endregion
}
