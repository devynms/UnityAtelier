using UnityEngine;

namespace Atelier.Audio {

    [AddComponentMenu("Atelier/Audio/Panel Player")]
    public class PanelPlayer : MonoBehaviour {

        [SerializeField]
        private AudioPanel panel;

        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float volumeScale = 1.0f;

        private AudioSource source;

        private AudioClip lastClip;

        public AudioPanel Panel { 
            get => this.panel; 
            set => this.panel = value; 
        }

        private void Awake() {
            this.source = GetComponentInParent<AudioSource>();
        }

        public void PlayClip() {
            if (this.panel.Count > 0) {
                this.PlayRandomClip();
            }
        }

        private void PlayRandomClip() {
            int index = Random.Range(0, this.panel.Count);
            AudioClip clip = this.panel[index];
            while (!this.PanelHasSingleClip() && this.ClipIsRepeat(clip)) {
                index = Random.Range(0, this.panel.Count);
                clip = this.panel[index];
            }
            this.source.PlayOneShot(clip, this.volumeScale);
            this.lastClip = clip;
        }

        private bool PanelHasSingleClip() {
            return this.panel.Count == 1;
        }

        private bool ClipIsRepeat(AudioClip clip) {
            return this.lastClip != null && this.lastClip == clip;
        }

    }

}

