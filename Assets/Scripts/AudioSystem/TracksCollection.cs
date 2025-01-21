using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    [CreateAssetMenu(fileName = "Music Tracks Collection", menuName = "Audio/Music Tracks Collection")]
    public class TracksCollection : ScriptableObject
    {
        [System.Serializable]
        public class TrackSounds
        {
            public Track name;
            public List<IDSound<Music>> sounds;
        }
    
        public List<TrackSounds> tracks;

        public AudioClip GetRandomClip(Track track)
        {
            var trackSounds = tracks.Find(t => t.name == track);
            if (trackSounds == null || trackSounds.sounds == null || trackSounds.sounds.Count == 0)
            {
                Debug.LogWarning($"No sounds found for the track: {track.ToString()}");
                return null;
            }

            var randomSound = trackSounds.sounds[Random.Range(0, trackSounds.sounds.Count)];
            return randomSound.clip;
        }
    }
}