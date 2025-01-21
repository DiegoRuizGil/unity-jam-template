using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AudioSystem
{
    [Serializable]
    public class IDSound<T> where T : Enum
    {
        public T name;
        public AudioClip clip;
    }

    public class AudioManager : MonoBehaviour
    {
        [Header("Audios")]
        [SerializeField] private SFXCollection _sfxCollection;
        [SerializeField] private TracksCollection _tracksCollection;

        [Header("SFX Pool")] [SerializeField] private int _sfxSourceAmount;
        [SerializeField] private AudioSource _sfxSourcePrefab;
        [SerializeField] private Transform _sfxPoolParent;

        [Header("Music Pool")] [SerializeField]
        private AudioSource _musicSourcePrefab;

        [SerializeField] private Transform _musicPoolParent;

        private List<AudioSource> _sfxSources;
        private MusicTracks _musicTracks;

        private static AudioManager _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }

            InitializeSFXPool();
            InitializeMusicTracks();
        }

        private void Update()
        {
            if (_musicTracks != null)
                _musicTracks.CheckTracks();
        }

        #region SFX
        public static void PlaySFX(SFX type, float volume = 1f)
        {
            AudioSource audioSource = _instance.FindSFXSourceAvailable();
            if (!audioSource) return;

            AudioClip clip = _instance._sfxCollection.GetSFXOfType(type);
            if (clip == null) return;

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }

        private void InitializeSFXPool()
        {
            _sfxSources = new List<AudioSource>();
            for (int i = 0; i < _sfxSourceAmount; i++)
            {
                var audioSource = Instantiate(_sfxSourcePrefab, _sfxPoolParent);
                _sfxSources.Add(audioSource);
            }
        }

        private AudioSource FindSFXSourceAvailable()
        {
            return _sfxSources.Find(source => !source.isPlaying);
        }

        #endregion

        #region MUSIC
        public static void FadeInMusic(Track track, float duration, Ease easeType)
        {
            _instance._musicTracks.FadeIn(track, duration, easeType);
        }
        
        public static void FadeOutMusic(Track track, float duration, Ease easeType)
        {
            _instance._musicTracks.FadeOut(track, duration, easeType);
        }

        private void InitializeMusicTracks()
        {
            _musicTracks = new MusicTracks(_tracksCollection);
            foreach (var track in _tracksCollection.tracks)
            {
                var audioSource = Instantiate(_musicSourcePrefab, _musicPoolParent);
                _musicTracks.AddTrack(track.name, audioSource);
            }
            _musicTracks.Init();
        }
        #endregion
    }
}
