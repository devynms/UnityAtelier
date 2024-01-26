using UnityEngine;
using System.Collections.Generic;

namespace Atelier.Audio {

    [CreateAssetMenu(fileName = "NewPanel", menuName = "Atelier/Audio Panel")]
    public class AudioPanel : ScriptableObject {

        [SerializeField]
        private AudioClip[] clips;

        public int Count => this.clips.Length;

        public AudioClip this[int index] => this.clips[index];

        public IEnumerable<AudioClip> Clips => this.clips;

    }

}

