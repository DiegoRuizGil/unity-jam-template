using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    [CreateAssetMenu(fileName = "SFX Collection", menuName = "Audio/SFX Collection")]
    public class SFXCollection : ScriptableObject
    {
        public List<IDSound<SFX>> sounds;

        public AudioClip GetSFXOfType(SFX type)
        {
            return sounds.Find(sound => sound.name == type)?.clip;
        }
    }
}