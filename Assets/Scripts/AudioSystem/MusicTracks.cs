using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AudioSystem
{
    public class MusicTracks
    {
        enum TrackState
        {
            Active, Inactive, Transitioning
        }
        
        private readonly TracksCollection _tracksCollection;
        
        private readonly Dictionary<Track, AudioSource> _tracks;
        private readonly Dictionary<Track, TrackState> _tracksState;

        public MusicTracks(TracksCollection tracksCollection)
        {
            _tracksCollection = tracksCollection;
            _tracks = new Dictionary<Track, AudioSource>();
            _tracksState = new Dictionary<Track, TrackState>();
        }

        public void CheckTracks()
        {
            foreach (var (track, audioSource) in _tracks)
            {
                if (_tracksState[track] != TrackState.Active)
                    continue;
                if (audioSource.isPlaying)
                    continue;
                
                audioSource.clip = _tracksCollection.GetRandomClip(track);
                audioSource.Play();
            }
        }

        public void FadeIn(Track track, float duration, Ease easeType)
        {
            var audioSource = _tracks[track];

            _tracksState[track] = TrackState.Transitioning;
            audioSource.DOFade(1f, duration).SetEase(easeType)
                .OnComplete(() =>
                {
                    _tracksState[track] = TrackState.Active;
                });
        }
        
        public void FadeOut(Track track, float duration, Ease easeType)
        {
            var audioSource = _tracks[track];
            
            _tracksState[track] = TrackState.Transitioning;
            audioSource.DOFade(0f, duration).SetEase(easeType)
                .OnComplete(() =>
                {
                    _tracksState[track] = TrackState.Inactive;
                });
        }
        
        public void AddTrack(Track track, AudioSource audioSource)
        {
            _tracks.Add(track, audioSource);
            _tracksState.Add(track, TrackState.Inactive);
        }
    }
}
